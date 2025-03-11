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
            Time = _nuclearesWeb.LoadDataFromGame("TIME");
            TimeStamp = _nuclearesWeb.LoadDataFromGame("TIME_STAMP");
        }
        else
        {
            Time = "";
            TimeStamp = "";
        }
    }

    public void Init(NuclearesWeb nuclearesWeb) => InitAsync(nuclearesWeb).GetAwaiter().GetResult();

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
        Time = await _nuclearesWeb.LoadDataFromGameAsync("TIME", cancellationToken);
        TimeStamp = await _nuclearesWeb.LoadDataFromGameAsync("TIME_STAMP", cancellationToken);
        return this;
    }
}
