using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using LibNuclearesWeb.BaseClasses;
using LibNuclearesWeb.Extensions;
using LibNuclearesWeb.NuclearesWeb.Plant;
using LibNuclearesWeb.NuclearesWeb.World;

namespace LibNuclearesWeb.NuclearesWeb;

/// <summary>
/// The main class for the NuclearesWeb library. This class is the main entry point for the library.
/// </summary>
[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
[SuppressMessage(
    "CodeQuality",
    "IDE0079:Remove unnecessary suppression",
    Justification = "ReSharper needs it to not complain about this class not being used directly."
)]
public class NuclearesWeb : MinObservableObject, IDisposable
{
    private const int DefaultPort = 8785;
    private const string DefaultNetworkLocation = "localhost";

    private HttpClient _httpClient;
    private readonly bool _disposeHttpClient;
    private SemaphoreSlim _semaphore = new(10, 10);
    private CancellationTokenSource _cts = new();
    private readonly TimeSpan _defaultRefreshInterval = TimeSpan.FromSeconds(1);
    private Task? _refreshTask;
    private bool _disposed;

    /// <summary>
    /// The main plant model.
    /// </summary>
    public PlantModel MainPlant { get; }

    /// <summary>
    /// The main world model.
    /// </summary>
    public WorldModel MainWorld { get; }

    [JsonIgnore]
    public ObservableStopwatch RefreshTime { get; } = new();

    [JsonIgnore]
    public ObservableStopwatch TimeSinceLastRefresh { get; } = new();

    private string _gameVersion;
    public string GameVersion => _gameVersion ??= "Test";

    private string _networkLocation = DefaultNetworkLocation;

    /// <summary>
    /// The network location (URL formatted) of the server.
    /// </summary>
    public string NetworkLocation
    {
        get => _networkLocation;
        private set => SetPropertyAndNotify(ref _networkLocation, value);
    }

    /// <summary>
    /// The port number of the running server, if that ever changes.
    /// </summary>
    public int Port { get; private set; } = DefaultPort;

    /// <summary>
    /// Whether to autoload and refresh data in the background on a default 5 second timer.
    /// </summary>
    public bool AutoRefresh { get; private set; }

    /// <summary>
    /// Constructor for NuclearesWeb.
    /// </summary>
    /// <param name="networkLocation">The network location (URL formatted) of the server.</param>
    /// <param name="port">The port number of the running server, if that ever changes.</param>
    /// <param name="autoRefresh">Whether to autoload and refresh data in the background on a default 5 second timer.</param>
    /// <param name="client">HttpClient to use, optional</param>
    public NuclearesWeb(
        string networkLocation = DefaultNetworkLocation,
        int port = DefaultPort,
        bool autoRefresh = false,
        HttpClient? client = null
    )
    {
        NetworkLocation = networkLocation;
        Port = port;
        _disposeHttpClient = client == null;
        _httpClient = client ?? new();
        MainWorld = new(this);
        MainPlant = new(this);

        if (!autoRefresh)
            return;
        EnableAutoRefresh();
        _ = RefreshAllData();
    }

    /// <summary>
    /// Initialize the runtime dependencies for the NuclearesWeb object.
    /// </summary>
    /// <returns>A fully initialized NuclearesWeb Object.</returns>
    public NuclearesWeb InitializeRuntimeDependencies()
    {
        _httpClient = new HttpClient();
        _semaphore = new SemaphoreSlim(10, 10);
        _cts = new CancellationTokenSource();
        // Reinitialize sub-models with the new runtime context.
        _ = (MainPlant?.Init(this));
        _ = (MainWorld?.Init(this));
        return this;
    }

    /// <summary>
    /// Enable the AutoRefresh feature.
    /// </summary>
    /// <param name="interval">TimeSpan interval to refresh.</param>
    public void EnableAutoRefresh(TimeSpan? interval = null)
    {
        interval ??= _defaultRefreshInterval;
        if (_refreshTask is { IsCompleted: false })
            return;
        AutoRefresh = true;
        _refreshTask = Task.Run(() => EnableAutoRefreshAsync(interval.Value, _cts.Token));
    }

    /// <summary>
    /// Enable the AutoRefresh feature.
    /// </summary>
    /// <param name="interval">TimeSpan interval to refresh.</param>
    /// <param name="cancellationToken">CancellationToken, optional.</param>
    /// <returns></returns>
    private async Task EnableAutoRefreshAsync(
        TimeSpan? interval = null,
        CancellationToken cancellationToken = default
    )
    {
        interval ??= _defaultRefreshInterval;

        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                _ = await RefreshAllDataAsync(cancellationToken);
                await Task.Delay(interval.Value, cancellationToken).ConfigureAwait(false);
            }
        }
        catch (TaskCanceledException) { }
    }

    /// <summary>
    /// Disable the AutoRefresh feature.
    /// </summary>
    public void DisableAutoRefresh() => DisableAutoRefreshAsync().GetAwaiter().GetResult();

    /// <summary>
    /// Disable the AutoRefresh feature.
    /// </summary>
    /// <returns></returns>
    private async Task DisableAutoRefreshAsync()
    {
        if (!AutoRefresh)
            return;
        AutoRefresh = false;
        await _cts.CancelAsync();
        try
        {
            if (_refreshTask != null)
                await _refreshTask;
        }
        catch (OperationCanceledException) { }
        finally
        {
            _cts.Dispose();
            _cts = new CancellationTokenSource();
            _refreshTask = null;
        }
    }

    /// <summary>
    /// Refreshes all plant and world data synchronously from the configured server. Prefer async methods where possible.
    /// </summary>
    /// <returns>This object post-refresh.</returns>
    public NuclearesWeb RefreshAllData() => RefreshAllDataAsync().GetAwaiter().GetResult();

    /// <summary>
    /// Refreshes all plant and world data asynchronously from the configured server.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A Task representing the async operation, with an instance of NuclearesWeb upon completion.</returns>
    public async Task<NuclearesWeb> RefreshAllDataAsync(
        CancellationToken cancellationToken = default
    )
    {
        if (AutoRefresh && _refreshTask != null && _refreshTask.IsCompleted)
            TimeSinceLastRefresh.Stop();
        RefreshTime.Restart();
        _ = await MainPlant.RefreshAllDataAsync(cancellationToken);
        _ = await MainWorld.RefreshAllDataAsync(cancellationToken);
        RefreshTime.Stop();
        TimeSinceLastRefresh.Restart();
        return this;
    }

    /// <summary>
    /// Load data from the game server. Synchronous wrapper for easy compatibility.
    /// </summary>
    /// <param name="valueName">The name of the value to query.</param>
    /// <returns>String data from the game.</returns>
    public string GetDataFromGame(string valueName) =>
        GetDataFromGameAsync(valueName).GetAwaiter().GetResult();

    /// <summary>
    /// Load data from the game server.
    /// </summary>
    /// <param name="valueName">Name of the parameter on the server to query.</param>
    /// <param name="cancellationToken">CancellationToken, optional.</param>
    /// <returns>A Task with the string result from the webserver.</returns>
    public async Task<string> GetDataFromGameAsync(
        string valueName,
        CancellationToken cancellationToken = default
    )
    {
        await _semaphore.WaitAsync(cancellationToken);
        try
        {
            var url =
                $"http://{NetworkLocation}:{Port}/?Variable={Uri.EscapeDataString(valueName)}";
            var response = await _httpClient.GetAsync(url, cancellationToken).ConfigureAwait(false);
            _ = response.EnsureSuccessStatusCode();
            return await response
                .Content.ReadAsStringAsync(cancellationToken)
                .ConfigureAwait(false);
        }
        finally
        {
            _ = _semaphore.Release();
        }
    }

    /// <summary>
    /// Set data on the game server.
    /// </summary>
    /// <param name="valueName">The name of the value to set.</param>
    /// <param name="data">The value to set.</param>
    /// <param name="cancellationToken">CancellationToken, optional.</param>
    /// <returns>A Task representing the completion of the request.</returns>
    public async Task SetDataToGameAsync(
        string valueName,
        string data,
        CancellationToken cancellationToken = default
    )
    {
        await _semaphore.WaitAsync(cancellationToken);
        try
        {
            var url =
                $"http://{NetworkLocation}:{Port}/?Variable={Uri.EscapeDataString(valueName)}&Value={data}";
            var response = await _httpClient
                .PostAsync(url, null, cancellationToken)
                .ConfigureAwait(false);
            _ = response.EnsureSuccessStatusCode();
        }
        finally
        {
            _ = _semaphore.Release();
        }
    }

    /// <summary>
    /// Dispose of the NuclearesWeb object.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Releases all resources used by the current instance of the <see cref="NuclearesWeb"/> class.
    /// </summary>
    /// <param name="disposing"></param>
    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
            return;
        if (disposing)
        {
            if (_disposeHttpClient)
                _httpClient.Dispose();
            RefreshTime.Dispose();
            _semaphore.Dispose();
            _cts.Cancel();
            _cts.Dispose();
        }
        _disposed = true;
    }
}
