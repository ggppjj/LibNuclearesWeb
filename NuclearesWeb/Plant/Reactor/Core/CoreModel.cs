using System.Text.Json.Serialization;
using LibNuclearesWeb.BaseClasses;
using LibNuclearesWeb.NuclearesWeb.Plant.Reactor.Core.Coolant;

namespace LibNuclearesWeb.NuclearesWeb.Plant.Reactor.Core;

public class CoreModel : MinObservableObject
{
    /// <summary>
    /// NuclearesWeb dependency.
    /// </summary>
    [JsonIgnore]
    private NuclearesWeb? _nuclearesWeb;

    /// <summary>
    /// Control rod bundles.
    /// </summary>
    [JsonInclude]
    public ControlRodBundleModel ControlRodBundles { get; private set; } = new();

    /// <summary>
    /// Coolant.
    /// </summary>
    [JsonInclude]
    public CoolantModel Coolant { get; private set; } = new();

    #region Notifiable properties.
    public string _temperature = string.Empty;

    /// <summary>
    /// Temperature.
    /// </summary>
    [JsonInclude]
    public string Temperature
    {
        get => _temperature;
        private set => SetPropertyAndNotify(ref _temperature, value);
    }

    private string _operativeTemperature = string.Empty;

    /// <summary>
    /// Operative temperature.
    /// </summary>
    [JsonInclude]
    public string OperativeTemperature
    {
        get => _operativeTemperature;
        private set => SetPropertyAndNotify(ref _operativeTemperature, value);
    }

    private string _maxTemperature = string.Empty;

    /// <summary>
    /// Maximum temperature.
    /// </summary>
    [JsonInclude]
    public string MaxTemperature
    {
        get => _maxTemperature;
        private set => SetPropertyAndNotify(ref _maxTemperature, value);
    }

    private string _minTemperature = string.Empty;

    /// <summary>
    /// Minimum temperature.
    /// </summary>
    [JsonInclude]
    public string MinTemperature
    {
        get => _minTemperature;
        private set => SetPropertyAndNotify(ref _minTemperature, value);
    }

    private string _isResidual = string.Empty;

    /// <summary>
    /// (Core heat) is residual.
    /// </summary>
    [JsonInclude]
    public string IsResidual
    {
        get => _isResidual;
        private set => SetPropertyAndNotify(ref _isResidual, value);
    }

    private string _pressure = string.Empty;

    /// <summary>
    /// Pressure.
    /// </summary>
    [JsonInclude]
    public string Pressure
    {
        get => _pressure;
        private set => SetPropertyAndNotify(ref _pressure, value);
    }

    private string _operativePressure = string.Empty;

    /// <summary>
    /// Operative pressure.
    /// </summary>
    [JsonInclude]
    public string OperativePressure
    {
        get => _operativePressure;
        private set => SetPropertyAndNotify(ref _operativePressure, value);
    }

    private string _maxPressure = string.Empty;

    /// <summary>
    /// Max pressure.
    /// </summary>
    [JsonInclude]
    public string MaxPressure
    {
        get => _maxPressure;
        private set => SetPropertyAndNotify(ref _maxPressure, value);
    }

    private string _integrity = string.Empty;

    /// <summary>
    /// Integrity.
    /// </summary>
    [JsonInclude]
    public string Integrity
    {
        get => _integrity;
        private set => SetPropertyAndNotify(ref _integrity, value);
    }

    private string _wear = string.Empty;

    /// <summary>
    /// Wear.
    /// </summary>
    [JsonInclude]
    public string Wear
    {
        get => _wear;
        private set => SetPropertyAndNotify(ref _wear, value);
    }

    private string _state = string.Empty;

    /// <summary>
    /// State.
    /// </summary>
    [JsonInclude]
    public string State
    {
        get => _state;
        private set => SetPropertyAndNotify(ref _state, value);
    }

    private string _stateCriticality = string.Empty;

    /// <summary>
    /// State criticality.
    /// </summary>
    [JsonInclude]
    public string StateCriticality
    {
        get => _stateCriticality;
        private set => SetPropertyAndNotify(ref _stateCriticality, value);
    }

    private string _criticalMassReached = string.Empty;

    /// <summary>
    /// Critical mass reached.
    /// </summary>
    [JsonInclude]
    public string CriticalMassReached
    {
        get => _criticalMassReached;
        private set => SetPropertyAndNotify(ref _criticalMassReached, value);
    }

    private string _criticalMassReachedCounter = string.Empty;

    /// <summary>
    /// Critical mass reached counter.
    /// </summary>
    [JsonInclude]
    public string CriticalMassReachedCounter
    {
        get => _criticalMassReachedCounter;
        private set => SetPropertyAndNotify(ref _criticalMassReachedCounter, value);
    }

    private string _imminentFusion = string.Empty;

    /// <summary>
    /// Imminent fusion.
    /// </summary>
    [JsonInclude]
    public string ImminentFusion
    {
        get => _imminentFusion;
        private set => SetPropertyAndNotify(ref _imminentFusion, value);
    }

    private string _readyForStart = string.Empty;

    /// <summary>
    /// Ready for start.
    /// </summary>
    [JsonInclude]
    public string ReadyForStart
    {
        get => _readyForStart;
        private set => SetPropertyAndNotify(ref _readyForStart, value);
    }

    private string _steamPresent = string.Empty;

    /// <summary>
    /// Steam present.
    /// </summary>
    [JsonInclude]
    public string SteamPresent
    {
        get => _steamPresent;
        private set => SetPropertyAndNotify(ref _steamPresent, value);
    }

    private string _highSteamPresent = string.Empty;

    /// <summary>
    /// High steam present.
    /// </summary>
    [JsonInclude]
    public string HighSteamPresent
    {
        get => _highSteamPresent;
        private set => SetPropertyAndNotify(ref _highSteamPresent, value);
    }

    /// <summary>
    /// Empty constructor for CoreModel for serialization purposes. Do not use this constructor without then calling Init() to satisfy dependencies.
    /// </summary>
    public CoreModel() { }

    /// <summary>
    /// Constructor for CoreModel. Internal class.
    /// </summary>
    /// <param name="nuclearesWeb">NuclearesWeb dependency.</param>
    internal CoreModel(NuclearesWeb nuclearesWeb)
    {
        _nuclearesWeb = nuclearesWeb;
        ControlRodBundles = new(nuclearesWeb);
        Coolant = new(nuclearesWeb);
    }
    #endregion

    /// <summary>
    /// Sets all data in the CoreModel object. Private class.
    /// </summary>
    /// <param name="temp"></param>
    /// <param name="opTemp"></param>
    /// <param name="maxTemp"></param>
    /// <param name="minTemp"></param>
    /// <param name="isResidual"></param>
    /// <param name="pressure"></param>
    /// <param name="maxPressure"></param>
    /// <param name="opPressure"></param>
    /// <param name="integrity"></param>
    /// <param name="wear"></param>
    /// <param name="state"></param>
    /// <param name="stateCriticality"></param>
    /// <param name="criticalMassReached"></param>
    /// <param name="imminentFusion"></param>
    /// <param name="readyForStart"></param>
    /// <param name="steamPresent"></param>
    /// <param name="highSteamPresent"></param>
    private void SetAllData(
        string temp,
        string opTemp,
        string maxTemp,
        string minTemp,
        string isResidual,
        string pressure,
        string maxPressure,
        string opPressure,
        string integrity,
        string wear,
        string state,
        string stateCriticality,
        string criticalMassReached,
        string imminentFusion,
        string readyForStart,
        string steamPresent,
        string highSteamPresent
    )
    {
        Temperature = temp;
        OperativeTemperature = opTemp;
        MaxTemperature = maxTemp;
        MinTemperature = minTemp;
        IsResidual = isResidual;
        Pressure = pressure;
        MaxPressure = maxPressure;
        OperativePressure = opPressure;
        Integrity = integrity;
        Wear = wear;
        State = state;
        StateCriticality = stateCriticality;
        CriticalMassReached = criticalMassReached;
        ImminentFusion = imminentFusion;
        ReadyForStart = readyForStart;
        SteamPresent = steamPresent;
        HighSteamPresent = highSteamPresent;
    }

    /// <summary>
    /// Refreshes all data in the CoreModel object. This method is synchronous.<br/>
    /// Prefer the async version of this method if you are calling it from an async-appropriate context.
    /// </summary>
    /// <returns>This object after refreshing all data.</returns>
    public CoreModel RefreshAllData() => RefreshAllDataAsync().GetAwaiter().GetResult();

    /// <summary>
    /// Refreshes all data in the CoreModel object.
    /// </summary>
    /// <param name="cancellationToken">CancellationToken, optional.</param>
    /// <returns>A Task that returns this object post-refresh as a result.</returns>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task<CoreModel> RefreshAllDataAsync(CancellationToken cancellationToken = default)
    {
        if (_nuclearesWeb == null)
            throw new InvalidOperationException("NuclearesWeb object is null");
        #region Refresh tasks.
        var controlRodBundleTask = ControlRodBundles.RefreshAllDataAsync(cancellationToken);
        var coolantTask = Coolant.RefreshAllDataAsync(cancellationToken);
        var tempTask = _nuclearesWeb.GetDataFromGameAsync("CORE_TEMP", cancellationToken);
        var opTempTask = _nuclearesWeb.GetDataFromGameAsync(
            "CORE_TEMP_OPERATIVE",
            cancellationToken
        );
        var maxTempTask = _nuclearesWeb.GetDataFromGameAsync("CORE_TEMP_MAX", cancellationToken);
        var minTempTask = _nuclearesWeb.GetDataFromGameAsync("CORE_TEMP_MIN", cancellationToken);
        var isResidualTask = _nuclearesWeb.GetDataFromGameAsync(
            "CORE_TEMP_RESIDUAL",
            cancellationToken
        );
        var pressureTask = _nuclearesWeb.GetDataFromGameAsync("CORE_PRESSURE", cancellationToken);
        var maxPressureTask = _nuclearesWeb.GetDataFromGameAsync(
            "CORE_PRESSURE_MAX",
            cancellationToken
        );
        var opPressureTask = _nuclearesWeb.GetDataFromGameAsync(
            "CORE_PRESSURE_OPERATIVE",
            cancellationToken
        );
        var integrityTask = _nuclearesWeb.GetDataFromGameAsync("CORE_INTEGRITY", cancellationToken);
        var wearTask = _nuclearesWeb.GetDataFromGameAsync("CORE_WEAR", cancellationToken);
        var stateTask = _nuclearesWeb.GetDataFromGameAsync("CORE_STATE", cancellationToken);
        var stateCriticalityTask = _nuclearesWeb.GetDataFromGameAsync(
            "CORE_STATE_CRITICALITY",
            cancellationToken
        );
        var criticalMassReachedTask = _nuclearesWeb.GetDataFromGameAsync(
            "CORE_CRITICAL_MASS_REACHED",
            cancellationToken
        );
        var criticalMassReachedCounterTask = _nuclearesWeb.GetDataFromGameAsync(
            "CORE_CRITICAL_MASS_REACHED_COUNTER",
            cancellationToken
        );
        var imminentFusionTask = _nuclearesWeb.GetDataFromGameAsync(
            "CORE_IMMINENT_FUSION",
            cancellationToken
        );
        var readyForStartTask = _nuclearesWeb.GetDataFromGameAsync(
            "CORE_READY_FOR_START",
            cancellationToken
        );
        var steamPresentTask = _nuclearesWeb.GetDataFromGameAsync(
            "CORE_STEAM_PRESENT",
            cancellationToken
        );
        var highSteamPresentTask = _nuclearesWeb.GetDataFromGameAsync(
            "CORE_HIGH_STEAM_PRESENT",
            cancellationToken
        );
        #endregion
        await Task.WhenAll(
                tempTask,
                opTempTask,
                maxTempTask,
                minTempTask,
                isResidualTask,
                pressureTask,
                maxPressureTask,
                opPressureTask,
                integrityTask,
                wearTask,
                stateTask,
                stateCriticalityTask,
                criticalMassReachedTask,
                criticalMassReachedCounterTask,
                imminentFusionTask,
                readyForStartTask,
                steamPresentTask,
                highSteamPresentTask
            )
            .ConfigureAwait(false);
        SetAllData(
            tempTask.Result,
            opTempTask.Result,
            maxTempTask.Result,
            minTempTask.Result,
            isResidualTask.Result,
            pressureTask.Result,
            maxPressureTask.Result,
            opPressureTask.Result,
            integrityTask.Result,
            wearTask.Result,
            stateTask.Result,
            stateCriticalityTask.Result,
            criticalMassReachedTask.Result,
            imminentFusionTask.Result,
            readyForStartTask.Result,
            steamPresentTask.Result,
            highSteamPresentTask.Result
        );
        await Task.WhenAll(controlRodBundleTask, coolantTask);
        return this;
    }

    /// <summary>
    /// Initializes the CoreModel object. This method should be called after the object is created with an empty initializer.
    /// </summary>
    /// <param name="nuclearesWeb">NuclearesWeb base dependency.</param>
    /// <param name="cancellationToken">CancellationToken, optional.</param>
    /// <returns>A Task that returns this object post-init as a result.</returns>
    public CoreModel Init(NuclearesWeb nuclearesWeb)
    {
        _nuclearesWeb = nuclearesWeb;
        ControlRodBundles.Init(nuclearesWeb);
        Coolant.Init(nuclearesWeb);
        return this;
    }
}
