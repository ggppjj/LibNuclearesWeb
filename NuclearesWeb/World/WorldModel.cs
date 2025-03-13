using System.Text.Json.Serialization;
using LibNuclearesWeb.BaseClasses;

namespace LibNuclearesWeb.NuclearesWeb.World;

public class WorldModel : MinObservableObject
{
    [JsonIgnore]
    private NuclearesWeb? _nuclearesWeb;

    private string _time = string.Empty;

    [JsonInclude]
    public string Time
    {
        get => _time;
        private set => SetPropertyAndNotify(ref _time, value);
    }

    private string _timeStamp = string.Empty;

    [JsonInclude]
    public string TimeStamp
    {
        get => _timeStamp;
        private set => SetPropertyAndNotify(ref _timeStamp, value);
    }

    public WorldModel() { }

    internal WorldModel(NuclearesWeb nuclearesWeb)
    {
        _nuclearesWeb = nuclearesWeb;
        if (_nuclearesWeb.AutoRefresh)
        {
            Time = _nuclearesWeb.GetDataFromGame("TIME");
            TimeStamp = _nuclearesWeb.GetDataFromGame("TIME_STAMP");
        }
        else
        {
            Time = "";
            TimeStamp = "";
        }
    }

    public WorldModel Init(NuclearesWeb nuclearesWeb) =>
        InitAsync(nuclearesWeb).GetAwaiter().GetResult();

    public Task<WorldModel> InitAsync(
        NuclearesWeb nuclearesWeb,
        CancellationToken cancellationToken = default
    )
    {
        _nuclearesWeb = nuclearesWeb;
        if (_nuclearesWeb.AutoRefresh)
            return RefreshAllDataAsync(cancellationToken);
        return Task.FromResult(this);
    }

    public WorldModel RefreshAllData(CancellationToken cancellationToken = default) =>
        Task.Run(() => RefreshAllDataAsync(cancellationToken)).GetAwaiter().GetResult();

    public async Task<WorldModel> RefreshAllDataAsync(CancellationToken cancellationToken = default)
    {
        if (_nuclearesWeb == null)
            throw new InvalidOperationException("NuclearesWeb object not set");
        Time = await _nuclearesWeb.GetDataFromGameAsync("TIME", cancellationToken);
        TimeStamp = await _nuclearesWeb.GetDataFromGameAsync("TIME_STAMP", cancellationToken);
        return this;
    }
}
