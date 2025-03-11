using System.Threading.Tasks;
using LibNuclearesWeb.BaseClasses;
using LibNuclearesWeb.NuclearesWeb.Plant;
using LibNuclearesWeb.NuclearesWeb.World;

namespace LibNuclearesWeb.NuclearesWeb;

public partial class NuclearesWeb : MinObservableObject
{
    private static readonly HttpClient httpClient = new();
    private readonly SemaphoreSlim semaphore = new(10, 10);
    private CancellationTokenSource cts = new();
    private Task? refreshTask;

    public PlantModel MainPlant { get; }
    public WorldModel MainWorld { get; }
    private string _networkLocation = "127.0.0.1";
    public string NetworkLocation
    {
        get => _networkLocation;
        set => SetPropertyAndNotify(ref _networkLocation, value);
    }

    public int Port { get; set; } = 8785;

    public bool AutoRefresh { get; set; }

    public NuclearesWeb(string? networkLocation = null, int? port = null, bool? autorefresh = null)
    {
        MainPlant = new(this);
        MainWorld = new(this);
        if (autorefresh != null)
            AutoRefresh = autorefresh.Value;
        if (networkLocation != null)
            NetworkLocation = networkLocation;
        if (port != null)
            Port = port.Value;
        if (AutoRefresh)
            RefreshAllData();
    }

    public void EnableAutoRefresh(TimeSpan? interval = null) =>
        EnableAutoRefreshAsync(interval).GetAwaiter().GetResult();

    public async Task EnableAutoRefreshAsync(
        TimeSpan? interval = null,
        CancellationToken cancellationToken = default
    )
    {
        if (interval == null)
            interval = TimeSpan.FromSeconds(5);
        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await RefreshAllDataAsync(cancellationToken);
                await Task.Delay(interval.Value, cancellationToken).ConfigureAwait(false);
            }
        }
        catch (TaskCanceledException) { }
    }

    public void DisableAutoRefresh() => DisableAutoRefreshAsync().GetAwaiter().GetResult();

    public async Task DisableAutoRefreshAsync()
    {
        if (AutoRefresh)
        {
            AutoRefresh = false;
            cts.Cancel();
            try
            {
                if (refreshTask != null)
                    await refreshTask;
            }
            catch (OperationCanceledException) { }
            finally
            {
                cts.Dispose();
                cts = new CancellationTokenSource();
                refreshTask = null;
            }
        }
    }

    public NuclearesWeb RefreshAllData(CancellationToken cancellationToken = default) =>
        RefreshAllDataAsync(cancellationToken).GetAwaiter().GetResult();

    public async Task<NuclearesWeb> RefreshAllDataAsync(
        CancellationToken cancellationToken = default
    )
    {
        await MainPlant.RefreshAllDataAsync(cancellationToken);
        await MainWorld.RefreshAllDataAsync(cancellationToken);
        return this;
    }

    public string LoadDataFromGame(
        string valueName,
        CancellationToken cancellationToken = default
    ) => LoadDataFromGameAsync(valueName, cancellationToken).GetAwaiter().GetResult();

    public async Task<string> LoadDataFromGameAsync(
        string valueName,
        CancellationToken cancellationToken = default
    )
    {
        await semaphore.WaitAsync(cancellationToken);
        try
        {
            var url =
                $"http://{NetworkLocation}:{Port}/?Variable={Uri.EscapeDataString(valueName)}";
            var response = await httpClient.GetAsync(url, cancellationToken).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            return await response
                .Content.ReadAsStringAsync(cancellationToken)
                .ConfigureAwait(false);
        }
        finally
        {
            semaphore.Release();
        }
    }
}
