using System.Text.Json.Serialization;
using LibNuclearesWeb.NuclearesWeb.Plant.Reactor;

namespace LibNuclearesWeb.NuclearesWeb.Plant;

public class PlantModel
{
    public PlantModel()
    {
        SteamGeneratorList.Add(new(0));
        SteamGeneratorList.Add(new(1));
        SteamGeneratorList.Add(new(2));
    }

    internal PlantModel(NuclearesWeb nuclearesWeb)
    {
        _nuclearesWeb = nuclearesWeb;
        MainReactor = new(nuclearesWeb);
        SteamGeneratorList.Add(new(nuclearesWeb, 0));
        SteamGeneratorList.Add(new(nuclearesWeb, 1));
        SteamGeneratorList.Add(new(nuclearesWeb, 2));
    }

    [JsonIgnore]
    private NuclearesWeb? _nuclearesWeb;

    [JsonInclude]
    public ReactorModel MainReactor { get; } = new();

    [JsonInclude]
    public List<SteamGeneratorModel> SteamGeneratorList { get; } = [];

    public PlantModel Init(NuclearesWeb nuclearesWeb) =>
        InitAsync(nuclearesWeb).GetAwaiter().GetResult();

    public Task<PlantModel> InitAsync(
        NuclearesWeb nuclearesWeb,
        CancellationToken cancellationToken = default
    )
    {
        _nuclearesWeb = nuclearesWeb;
        foreach (var steamGenerator in SteamGeneratorList)
            steamGenerator.Init(nuclearesWeb);
        if (_nuclearesWeb.AutoRefresh)
            return RefreshAllDataAsync(cancellationToken);
        return Task.FromResult(this);
    }

    public PlantModel RefreshAllData(CancellationToken cancellationToken = default) =>
        RefreshAllDataAsync(cancellationToken).ConfigureAwait(false).GetAwaiter().GetResult();

    public async Task<PlantModel> RefreshAllDataAsync(CancellationToken cancellationToken = default)
    {
        List<Task> tasks = [];
        tasks.Add(MainReactor.RefreshAllDataAsync(cancellationToken));
        foreach (var generator in SteamGeneratorList)
            tasks.Add(generator.RefreshAllDataAsync(cancellationToken));
        await Task.WhenAll(tasks).ConfigureAwait(false);
        return this;
    }
}
