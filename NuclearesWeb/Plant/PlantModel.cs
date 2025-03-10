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

    public void Init(NuclearesWeb nuclearesWeb)
    {
        _nuclearesWeb = nuclearesWeb;
        MainReactor.Init(nuclearesWeb);
        foreach (var generator in SteamGeneratorList)
            generator.Init(nuclearesWeb);
    }

    public PlantModel RefreshAllData(CancellationToken cancellationToken = default) =>
        Task.Run(() => RefreshAllDataAsync(cancellationToken)).GetAwaiter().GetResult();

    public async Task<PlantModel> RefreshAllDataAsync(CancellationToken cancellationToken = default)
    {
        await MainReactor.RefreshAllDataAsync(cancellationToken);
        foreach (var generator in SteamGeneratorList)
            await generator.RefreshAllDataAsync(cancellationToken);
        return this;
    }
}
