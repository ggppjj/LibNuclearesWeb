using System.Text.Json.Serialization;
using LibNuclearesWeb.BaseClasses;
using LibNuclearesWeb.NuclearesWeb.Plant.Reactor;

namespace LibNuclearesWeb.NuclearesWeb.Plant;

public class PlantModel : MinObservableObject
{
    private NuclearesWeb? _nuclearesWeb;

    [JsonInclude]
    public ReactorModel MainReactor { get; } = new();

    [JsonInclude]
    public List<SteamGeneratorModel> SteamGeneratorList { get; } = [];

    private string _numberOfCoreCirculationPumps = string.Empty;

    [JsonInclude]
    public string NumberOfCoreCirculationPumps
    {
        get => _numberOfCoreCirculationPumps;
        set => SetPropertyAndNotify(ref _numberOfCoreCirculationPumps, value);
    }

    private string _numberOfFreightPumps = string.Empty;

    [JsonInclude]
    public string NumberOfFreightPumps
    {
        get => _numberOfFreightPumps;
        set => SetPropertyAndNotify(ref _numberOfFreightPumps, value);
    }

    private string _auxDivertSurplusFromKW = string.Empty;

    [JsonInclude]
    public string AuxDivertSurplusFromKW
    {
        get => _auxDivertSurplusFromKW;
        set => SetPropertyAndNotify(ref _auxDivertSurplusFromKW, value);
    }

    private string _auxEffectivelyDerivedEnergyKW = string.Empty;

    [JsonInclude]
    public string AuxEffectivelyDerivedEnergyKW
    {
        get => _auxEffectivelyDerivedEnergyKW;
        set => SetPropertyAndNotify(ref _auxEffectivelyDerivedEnergyKW, value);
    }

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

    public PlantModel SetAllData(
        string numCoolPump,
        string numUncoolPump,
        string divertKW,
        string derivedKw
    )
    {
        NumberOfCoreCirculationPumps = numCoolPump;
        NumberOfFreightPumps = numUncoolPump;
        AuxDivertSurplusFromKW = divertKW;
        AuxEffectivelyDerivedEnergyKW = derivedKw;
        return this;
    }

    public PlantModel Init(NuclearesWeb nuclearesWeb)
    {
        _nuclearesWeb = nuclearesWeb;
        foreach (var steamGenerator in SteamGeneratorList)
            _ = steamGenerator.Init(_nuclearesWeb);
        _ = MainReactor.Init(_nuclearesWeb);
        return this;
    }

    public PlantModel RefreshAllData() =>
        RefreshAllDataAsync().ConfigureAwait(false).GetAwaiter().GetResult();

    public async Task<PlantModel> RefreshAllDataAsync(CancellationToken cancellationToken = default)
    {
        if (_nuclearesWeb == null)
            throw new InvalidOperationException("NuclearesWeb object is null!");
        var coolPumpNumberTask = _nuclearesWeb.GetDataFromGameAsync(
            "COOLANT_CORE_QUANTITY_CIRCULATION_PUMPS_PRESENT",
            cancellationToken
        );
        var uncoolPumpNumberTask = _nuclearesWeb.GetDataFromGameAsync(
            "COOLANT_CORE_QUANTITY_FREIGHT_PUMPS_PRESENT",
            cancellationToken
        );
        var auxDivertTask = _nuclearesWeb.GetDataFromGameAsync(
            "AUX_DIVERT_SURPLUS_FROM_KW",
            cancellationToken
        );
        var auxDerivedTask = _nuclearesWeb.GetDataFromGameAsync(
            "AUX_EFFECTIVELY_DERIVED_ENERGY_KW",
            cancellationToken
        );
        List<Task> childTasks = [];
        childTasks.Add(MainReactor.RefreshAllDataAsync(cancellationToken));
        childTasks.AddRange(
            [coolPumpNumberTask, uncoolPumpNumberTask, auxDerivedTask, auxDivertTask]
        );
        foreach (var generator in SteamGeneratorList)
            childTasks.Add(generator.RefreshAllDataAsync(cancellationToken));
        await Task.WhenAll(childTasks).ConfigureAwait(false);
        return SetAllData(
            coolPumpNumberTask.Result,
            uncoolPumpNumberTask.Result,
            auxDivertTask.Result,
            auxDerivedTask.Result
        );
    }
}
