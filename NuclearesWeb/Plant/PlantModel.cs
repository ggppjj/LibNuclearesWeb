using System.Text.Json.Serialization;
using LibNuclearesWeb.NuclearesWeb.Plant.Reactor;

namespace LibNuclearesWeb.NuclearesWeb.Plant;

public class PlantModel
{
    private NuclearesWeb? _nuclearesWeb;

    [JsonInclude]
    public ReactorModel MainReactor { get; } = new();

    [JsonInclude]
    public List<SteamGeneratorModel> SteamGeneratorList { get; } = [];

    public PlantModel()
    {
        MainReactor = new();
        SteamGeneratorList.Add(new(0));
        SteamGeneratorList.Add(new(1));
        SteamGeneratorList.Add(new(2));
    }

    internal PlantModel(NuclearesWeb nuclearesWeb)
    {
        _nuclearesWeb = nuclearesWeb;
        MainReactor = new(_nuclearesWeb);
        SteamGeneratorList.Add(new(_nuclearesWeb, 0));
        SteamGeneratorList.Add(new(_nuclearesWeb, 1));
        SteamGeneratorList.Add(new(_nuclearesWeb, 2));
    }

    public PlantModel Init(NuclearesWeb nuclearesWeb)
    {
        _nuclearesWeb = nuclearesWeb;
        foreach (var steamGenerator in SteamGeneratorList)
            steamGenerator.Init(_nuclearesWeb);
        MainReactor.Init(_nuclearesWeb);
        return this;
    }

    public PlantModel RefreshAllData() =>
        RefreshAllDataAsync().ConfigureAwait(false).GetAwaiter().GetResult();

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
