using System.Text.Json.Serialization;
using LibNuclearesWeb.BaseClasses;
using LibNuclearesWeb.NuclearesWeb.Plant.Reactor;

namespace LibNuclearesWeb.NuclearesWeb.Plant;

/// <summary>
/// A conceptual overview of the plant as a whole.
/// </summary>
public class PlantModel : MinObservableObject
{
    private NuclearesWeb? _nuclearesWeb;

    /// <summary>
    /// "Main" reactor. Currently, only reactor.
    /// </summary>
    [JsonInclude]
    public ReactorModel MainReactor { get; }

    /// <summary>
    /// List of Steam generator objects, 0,1,2.
    /// </summary>
    [JsonInclude]
    public List<SteamGeneratorModel> SteamGeneratorList { get; } = [];

    private int _numberOfCoreCirculationPumps = 0;

    /// <summary>
    /// Number of core circulation pumps. Max (should be) 7.
    /// </summary>
    [JsonInclude]
    public int NumberOfCoreCirculationPumps
    {
        get => _numberOfCoreCirculationPumps;
        set => SetPropertyAndNotify(ref _numberOfCoreCirculationPumps, value);
    }

    private int _numberOfFreightPumps = 0;

    /// <summary>
    /// The number of installed freight pumps. Max (should be) 5.
    /// </summary>
    [JsonInclude]
    public int NumberOfFreightPumps
    {
        get => _numberOfFreightPumps;
        set => SetPropertyAndNotify(ref _numberOfFreightPumps, value);
    }

    private string _auxDivertSurplusFromKw = string.Empty;

    /// <summary>
    /// The amount of power shunted to the resistor bank in KW.
    /// </summary>
    [JsonInclude]
    public string AuxDivertSurplusFromKw
    {
        get => _auxDivertSurplusFromKw;
        set => SetPropertyAndNotify(ref _auxDivertSurplusFromKw, value);
    }

    private string _auxEffectivelyDerivedEnergyKw = string.Empty;

    /// <summary>
    ///
    /// </summary>
    [JsonInclude]
    public string AuxEffectivelyDerivedEnergyKw
    {
        get => _auxEffectivelyDerivedEnergyKw;
        set => SetPropertyAndNotify(ref _auxEffectivelyDerivedEnergyKw, value);
    }

    /// <summary>
    /// Empty constructor primarily for deserialization. Run Init() after creation before use!
    /// </summary>
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

    internal PlantModel SetAllData(
        string numCoolPump,
        string numUncoolPump,
        string divertKw,
        string derivedKw
    )
    {
        NumberOfCoreCirculationPumps = int.Parse(numCoolPump);
        NumberOfFreightPumps = int.Parse(numUncoolPump);
        AuxDivertSurplusFromKw = divertKw;
        AuxEffectivelyDerivedEnergyKw = derivedKw;
        return this;
    }

    /// <summary>
    /// Run after deserialization to initialize this and all child objects with the NuclearesWeb dependency.
    /// </summary>
    /// <param name="nuclearesWeb">NuclearesWeb dependency.</param>
    /// <returns>An initialized PlantModel.</returns>
    public PlantModel Init(NuclearesWeb nuclearesWeb)
    {
        _nuclearesWeb = nuclearesWeb;
        foreach (var steamGenerator in SteamGeneratorList)
            _ = steamGenerator.Init(_nuclearesWeb);
        _ = MainReactor.Init(_nuclearesWeb);
        return this;
    }

    /// <summary>
    /// Trigger a manual refresh of all data on this and all child objects.<br/>
    /// This method is synchronous and will block UI calls. Prefer RefreshAllDataAsync where possible.
    /// </summary>
    /// <returns>A PlantModel with all updated data.</returns>
    /// <exception cref="InvalidOperationException">You must run Init() first!</exception>
    public PlantModel RefreshAllData() =>
        RefreshAllDataAsync().ConfigureAwait(false).GetAwaiter().GetResult();

    /// <summary>
    /// Trigger a manual refresh of all data on this and all child objects.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns>A task with a PlantModel that has had all data updated from the game as a result.</returns>
    /// <exception cref="InvalidOperationException">You must run Init() first!</exception>
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
        List<Task> childTasks =
        [
            MainReactor.RefreshAllDataAsync(cancellationToken),
            coolPumpNumberTask,
            uncoolPumpNumberTask,
            auxDerivedTask,
            auxDivertTask,
        ];
        childTasks.AddRange(
            SteamGeneratorList
                .Select(generator => generator.RefreshAllDataAsync(cancellationToken))
                .Cast<Task>()
        );
        await Task.WhenAll(childTasks).ConfigureAwait(false);
        return SetAllData(
            coolPumpNumberTask.Result,
            uncoolPumpNumberTask.Result,
            auxDivertTask.Result,
            auxDerivedTask.Result
        );
    }
}
