using System.Text.Json.Serialization;
using LibNuclearesWeb.BaseClasses;

namespace LibNuclearesWeb.NuclearesWeb.Plant.Reactor.Core.Coolant;

public class CoolantModel : MinObservableObject
{
    private NuclearesWeb? _nuclearesWeb;

    [JsonInclude]
    public List<PumpModel> PumpList { get; private set; } = [];

    #region Notifiable Properties.
    private string _coreState = string.Empty;

    [JsonInclude]
    public string CoreState
    {
        get => _coreState;
        set => SetPropertyAndNotify(ref _coreState, value);
    }

    private string _corePressure = string.Empty;

    [JsonInclude]
    public string CorePressure
    {
        get => _corePressure;
        set => SetPropertyAndNotify(ref _corePressure, value);
    }

    private string _coreMaxPressure = string.Empty;

    [JsonInclude]
    public string CoreMaxPressure
    {
        get => _coreMaxPressure;
        set => SetPropertyAndNotify(ref _coreMaxPressure, value);
    }

    private string _coreVesselTemperature = string.Empty;

    [JsonInclude]
    public string CoreVesselTemperature
    {
        get => _coreVesselTemperature;
        set => SetPropertyAndNotify(ref _coreVesselTemperature, value);
    }

    private string _coreQuantityInVessel = string.Empty;

    [JsonInclude]
    public string CoreQuantityInVessel
    {
        get => _coreQuantityInVessel;
        set => SetPropertyAndNotify(ref _coreQuantityInVessel, value);
    }

    private string _corePrimaryLoopLevel = string.Empty;

    [JsonInclude]
    public string CorePrimaryLoopLevel
    {
        get => _corePrimaryLoopLevel;
        set => SetPropertyAndNotify(ref _corePrimaryLoopLevel, value);
    }

    private string _coreFlowSpeed = string.Empty;

    [JsonInclude]
    public string CoreFlowSpeed
    {
        get => _coreFlowSpeed;
        set => SetPropertyAndNotify(ref _coreFlowSpeed, value);
    }

    private string _coreFlowPercentOrdered = string.Empty;

    [JsonInclude]
    public string CoreFlowPercentOrdered
    {
        get => _coreFlowPercentOrdered;
        set => SetPropertyAndNotify(ref _coreFlowPercentOrdered, value);
    }

    private string _coreFlowPercentReached = string.Empty;

    [JsonInclude]
    public string CoreFlowPercentReached
    {
        get => _coreFlowPercentReached;
        set => SetPropertyAndNotify(ref _coreFlowPercentReached, value);
    }
    #endregion

    public CoolantModel()
    {
        PumpList.Add(new(0));
        PumpList.Add(new(1));
        PumpList.Add(new(2));
    }

    internal CoolantModel(NuclearesWeb nuclearesWeb)
    {
        _nuclearesWeb = nuclearesWeb;
        PumpList.Add(new(0, _nuclearesWeb));
        PumpList.Add(new(1, _nuclearesWeb));
        PumpList.Add(new(2, _nuclearesWeb));
    }

    public CoolantModel Init(NuclearesWeb nuclearesWeb)
    {
        _nuclearesWeb = nuclearesWeb;
        foreach (var pump in PumpList)
            _ = pump.Init(nuclearesWeb);
        return this;
    }

    /// <summary>
    /// Refreshes all coolant-related data synchronously. Prefer RefreshAllDataAsync() where possible!
    /// </summary>
    /// <returns>The updated instance of the CoolantModel.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the NuclearesWeb dependency is null.</exception>
    public CoolantModel RefreshAllData() =>
        RefreshAllDataAsync().ConfigureAwait(false).GetAwaiter().GetResult();

    /// <summary>
    /// Refreshes all coolant-related data asynchronously.
    /// </summary>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    /// <returns>The updated instance of the CoolantModel.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the NuclearesWeb dependency is null.</exception>
    public async Task<CoolantModel> RefreshAllDataAsync(
        CancellationToken cancellationToken = default
    )
    {
        if (_nuclearesWeb == null)
            throw new InvalidOperationException("NuclearesWeb object is null");
        #region Tasks.
        var coreStateTask = _nuclearesWeb.GetDataFromGameAsync(
            "COOLANT_CORE_STATE",
            cancellationToken
        );
        var corePressureTask = _nuclearesWeb.GetDataFromGameAsync(
            "COOLANT_CORE_PRESSURE",
            cancellationToken
        );
        var coreMaxPressureTask = _nuclearesWeb.GetDataFromGameAsync(
            "COOLANT_CORE_MAX_PRESSURE",
            cancellationToken
        );
        var coreVesselTemperatureTask = _nuclearesWeb.GetDataFromGameAsync(
            "COOLANT_CORE_VESSEL_TEMPERATURE",
            cancellationToken
        );
        var coreQuantityInVesselTask = _nuclearesWeb.GetDataFromGameAsync(
            "COOLANT_CORE_QUANTITY_IN_VESSEL",
            cancellationToken
        );
        var corePrimaryLoopLevelTask = _nuclearesWeb.GetDataFromGameAsync(
            "COOLANT_CORE_PRIMARY_LOOP_LEVEL",
            cancellationToken
        );
        var coreFlowSpeedTask = _nuclearesWeb.GetDataFromGameAsync(
            "COOLANT_CORE_FLOW_SPEED",
            cancellationToken
        );
        var coreFlowOrderedTask = _nuclearesWeb.GetDataFromGameAsync(
            "COOLANT_CORE_FLOW_ORDERED_SPEED",
            cancellationToken
        );
        var coreFlowReachedTask = _nuclearesWeb.GetDataFromGameAsync(
            "COOLANT_CORE_FLOW_REACHED_SPEED",
            cancellationToken
        );
        #endregion
        var pumpTasks = PumpList.Select(pump => pump.RefreshAllDataAsync(cancellationToken));
        await Task.WhenAll(
                coreStateTask,
                corePressureTask,
                coreMaxPressureTask,
                coreVesselTemperatureTask,
                coreQuantityInVesselTask,
                corePrimaryLoopLevelTask,
                coreFlowSpeedTask,
                coreFlowOrderedTask,
                coreFlowReachedTask,
                Task.WhenAll(pumpTasks)
            )
            .ConfigureAwait(false);
        return SetAllData(
            coreStateTask.Result,
            corePressureTask.Result,
            coreMaxPressureTask.Result,
            coreVesselTemperatureTask.Result,
            coreQuantityInVesselTask.Result,
            corePrimaryLoopLevelTask.Result,
            coreFlowSpeedTask.Result,
            coreFlowOrderedTask.Result,
            coreFlowReachedTask.Result
        );
    }

    /// <summary>
    /// Sets all coolant-related data into the model properties.
    /// </summary>
    /// <param name="coreState">The state of the coolant core.</param>
    /// <param name="corePressure">The current pressure of the coolant core.</param>
    /// <param name="coreMaxPressure">The maximum pressure of the coolant core.</param>
    /// <param name="coreVesselTemperature">The vessel temperature of the coolant core.</param>
    /// <param name="coreQuantityInVessel">The coolant quantity in the core vessel.</param>
    /// <param name="corePrimaryLoopLevel">The fill level of the primary coolant loop.</param>
    /// <param name="coreFlowSpeed">The current coolant flow speed.</param>
    /// <param name="coreFlowOrdered">The ordered (target) coolant flow speed.</param>
    /// <param name="coreFlowReached">The reached coolant flow speed.</param>
    /// <returns>The updated instance of the CoolantModel.</returns>
    private CoolantModel SetAllData(
        string coreState,
        string corePressure,
        string coreMaxPressure,
        string coreVesselTemperature,
        string coreQuantityInVessel,
        string corePrimaryLoopLevel,
        string coreFlowSpeed,
        string coreFlowOrdered,
        string coreFlowReached
    )
    {
        CoreState = coreState;
        CorePressure = corePressure;
        CoreMaxPressure = coreMaxPressure;
        CoreVesselTemperature = coreVesselTemperature;
        CoreQuantityInVessel = coreQuantityInVessel;
        CorePrimaryLoopLevel = corePrimaryLoopLevel;
        CoreFlowSpeed = coreFlowSpeed;
        CoreFlowPercentOrdered = coreFlowOrdered;
        CoreFlowPercentReached = coreFlowReached;
        return this;
    }
}
