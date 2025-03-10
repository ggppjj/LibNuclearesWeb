using System.Text.Json.Serialization;

namespace LibNuclearesWeb.NuclearesWeb.World;

public class WorldModel
{
    [JsonIgnore]
    private readonly NuclearesWeb _nuclearesWeb;

    public string Time { get; private set; }
    public string TimeStamp { get; private set; }

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

    public WorldModel RefreshAllData(CancellationToken cancellationToken = default) =>
        Task.Run(() => RefreshAllDataAsync(cancellationToken)).GetAwaiter().GetResult();

    public async Task<WorldModel> RefreshAllDataAsync(CancellationToken cancellationToken = default)
    {
        Time = await _nuclearesWeb.LoadDataFromGameAsync("TIME", cancellationToken);
        TimeStamp = await _nuclearesWeb.LoadDataFromGameAsync("TIME_STAMP", cancellationToken);
        return this;
    }
}
