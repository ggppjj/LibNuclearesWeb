using LibNuclearesWeb.NuclearesWeb.Plant;
using LibNuclearesWeb.NuclearesWeb.World;

namespace LibNuclearesWeb.NuclearesWeb;

public partial class NuclearesWeb
{
    private static readonly HttpClient httpClient = new();
    private readonly SemaphoreSlim semaphore = new(10, 10);
    public PlantModel MainPlant { get; }
    public WorldModel MainWorld { get; }
    public bool AutoRefresh { get; set; } = true;

    private string _networkLocation = "127.0.0.1";
    public string NetworkLocation
    {
        get => _networkLocation;
        set
        {
            _networkLocation = value;
            if (AutoRefresh)
                RefreshAllData();
        }
    }

    private int _port = 8785;
    public int Port
    {
        get => _port;
        set
        {
            _port = value;
            if (AutoRefresh)
                RefreshAllData();
        }
    }

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

    public NuclearesWeb RefreshAllData(CancellationToken cancellationToken = default)
    {
        MainPlant.RefreshAllData(cancellationToken);
        MainWorld.RefreshAllData(cancellationToken);
        return this;
    }

    public async Task<NuclearesWeb> RefreshAllDataAsync(
        CancellationToken cancellationToken = default
    )
    {
        await MainPlant.RefreshAllDataAsync(cancellationToken);
        await MainWorld.RefreshAllDataAsync(cancellationToken);
        return this;
    }

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

    public string LoadDataFromGame(
        string valueName,
        CancellationToken cancellationToken = default
    ) => LoadDataFromGameAsync(valueName, cancellationToken).GetAwaiter().GetResult();
}
