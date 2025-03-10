namespace LibNuclearesWeb;

public partial class NuclearesWeb
{
    private static readonly HttpClient httpClient = new();
    private readonly SemaphoreSlim semaphore = new(10, 10);

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

    public Plants Plant { get; }
    public Worlds World { get; }

    public NuclearesWeb(string? networkLocation = null, int? port = null, bool? autorefresh = null)
    {
        Plant = new(this);
        World = new(this);
        if (autorefresh != null)
            AutoRefresh = autorefresh.Value;
        if (networkLocation != null)
            NetworkLocation = networkLocation;
        if (port != null)
            Port = port.Value;
        if (AutoRefresh)
            RefreshAllData();
    }

    public void RefreshAllData()
    {
        Plant.RefreshAllData();
        World.RefreshAllData();
    }

    public async Task RefreshAllDataAsync(CancellationToken cancellationToken = default)
    {
        await Plant.RefreshAllDataAsync(cancellationToken);
        await World.RefreshAllDataAsync(cancellationToken);
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
