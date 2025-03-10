using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace LibNuclearesWeb;

public partial class NuclearesWeb
{
    public partial class Plants
    {
        public partial class Reactors
        {
            public partial class Cores : INotifyPropertyChanged
            {
                [JsonIgnore]
                private readonly NuclearesWeb? _nucleares;

                [JsonInclude]
                public ControlRods ControlRodBundles { get; private set; } = new();

                [JsonInclude]
                public Coolant CoolantStatus { get; private set; } = new();

                public string _temperature = string.Empty;

                [JsonInclude]
                public string Temperature
                {
                    get => _temperature;
                    private set => SetProperty(ref _temperature, value);
                }

                private string _operativeTemperature = string.Empty;

                [JsonInclude]
                public string OperativeTemperature
                {
                    get => _operativeTemperature;
                    private set => SetProperty(ref _operativeTemperature, value);
                }

                private string _maxTemperature = string.Empty;

                [JsonInclude]
                public string MaxTemperature
                {
                    get => _maxTemperature;
                    private set => SetProperty(ref _maxTemperature, value);
                }

                private string _minTemperature = string.Empty;

                [JsonInclude]
                public string MinTemperature
                {
                    get => _minTemperature;
                    private set => SetProperty(ref _minTemperature, value);
                }

                private string _isResidual = string.Empty;

                [JsonInclude]
                public string IsResidual
                {
                    get => _isResidual;
                    private set => SetProperty(ref _isResidual, value);
                }

                private string _pressure = string.Empty;

                [JsonInclude]
                public string Pressure
                {
                    get => _pressure;
                    private set => SetProperty(ref _pressure, value);
                }

                private string _operativePressure = string.Empty;

                [JsonInclude]
                public string OperativePressure
                {
                    get => _operativePressure;
                    private set => SetProperty(ref _operativePressure, value);
                }

                private string _maxPressure = string.Empty;

                [JsonInclude]
                public string MaxPressure
                {
                    get => _maxPressure;
                    private set => SetProperty(ref _maxPressure, value);
                }

                private string _integrity = string.Empty;

                [JsonInclude]
                public string Integrety
                {
                    get => _integrity;
                    private set => SetProperty(ref _integrity, value);
                }

                private string _wear = string.Empty;

                [JsonInclude]
                public string Wear
                {
                    get => _wear;
                    private set => SetProperty(ref _wear, value);
                }

                private string _state = string.Empty;

                [JsonInclude]
                public string State
                {
                    get => _state;
                    private set => SetProperty(ref _state, value);
                }

                private string _stateCriticality = string.Empty;

                [JsonInclude]
                public string StateCriticality
                {
                    get => _stateCriticality;
                    private set => SetProperty(ref _stateCriticality, value);
                }

                private string _criticalMassReached = string.Empty;

                [JsonInclude]
                public string CriticalMassReached
                {
                    get => _criticalMassReached;
                    private set => SetProperty(ref _criticalMassReached, value);
                }

                private string _criticalMassReachedCounter = string.Empty;

                [JsonInclude]
                public string CriticalMassReachedCounter
                {
                    get => _criticalMassReachedCounter;
                    private set => SetProperty(ref _criticalMassReachedCounter, value);
                }

                private string _imminentFusion = string.Empty;

                [JsonInclude]
                public string ImminentFusion
                {
                    get => _imminentFusion;
                    private set => SetProperty(ref _imminentFusion, value);
                }

                private string _readyForStart = string.Empty;

                [JsonInclude]
                public string ReadyForStart
                {
                    get => _readyForStart;
                    private set => SetProperty(ref _readyForStart, value);
                }

                private string _steamPresent = string.Empty;

                [JsonInclude]
                public string SteamPresent
                {
                    get => _steamPresent;
                    private set => SetProperty(ref _steamPresent, value);
                }

                private string _highSteamPresent = string.Empty;

                [JsonInclude]
                public string HighSteamPresent
                {
                    get => _highSteamPresent;
                    private set => SetProperty(ref _highSteamPresent, value);
                }

                protected bool SetProperty<T>(
                    ref T field,
                    T value,
                    [CallerMemberName] string? propertyName = null
                )
                {
                    if (EqualityComparer<T>.Default.Equals(field, value))
                        return false;
                    field = value;
                    OnPropertyChanged(propertyName);
                    return true;
                }

                public event PropertyChangedEventHandler? PropertyChanged;

                protected virtual void OnPropertyChanged(
                    [CallerMemberName] string? propertyName = null
                ) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

                public Cores() { }

                internal Cores(NuclearesWeb nucleares)
                {
                    _nucleares = nucleares;
                    ControlRodBundles = new(nucleares);
                    CoolantStatus = new(nucleares);
                }

                private void SetAllData(
                    string temp,
                    string opTemp,
                    string maxTemp,
                    string minTemp,
                    string isResidual,
                    string pressure,
                    string maxPressure,
                    string opPressure,
                    string integrety,
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
                    Integrety = integrety;
                    Wear = wear;
                    State = state;
                    StateCriticality = stateCriticality;
                    CriticalMassReached = criticalMassReached;
                    ImminentFusion = imminentFusion;
                    ReadyForStart = readyForStart;
                    SteamPresent = steamPresent;
                    HighSteamPresent = highSteamPresent;
                }

                public async Task<Cores> RefreshAllDataAsync(
                    CancellationToken cancellationToken = default
                )
                {
                    if (_nucleares == null)
                        throw new InvalidOperationException("NuclearesWeb object is null");
                    var tempTask = _nucleares.LoadDataFromGameAsync("CORE_TEMP", cancellationToken);
                    var opTempTask = _nucleares.LoadDataFromGameAsync(
                        "CORE_TEMP_OPERATIVE",
                        cancellationToken
                    );
                    var maxTempTask = _nucleares.LoadDataFromGameAsync(
                        "CORE_TEMP_MAX",
                        cancellationToken
                    );
                    var minTempTask = _nucleares.LoadDataFromGameAsync(
                        "CORE_TEMP_MIN",
                        cancellationToken
                    );
                    var isResidualTask = _nucleares.LoadDataFromGameAsync(
                        "CORE_TEMP_RESIDUAL",
                        cancellationToken
                    );
                    var pressureTask = _nucleares.LoadDataFromGameAsync(
                        "CORE_PRESSURE",
                        cancellationToken
                    );
                    var maxPressureTask = _nucleares.LoadDataFromGameAsync(
                        "CORE_PRESSURE_MAX",
                        cancellationToken
                    );
                    var opPressureTask = _nucleares.LoadDataFromGameAsync(
                        "CORE_PRESSURE_OPERATIVE",
                        cancellationToken
                    );
                    var integrityTask = _nucleares.LoadDataFromGameAsync(
                        "CORE_INTEGRITY",
                        cancellationToken
                    );
                    var wearTask = _nucleares.LoadDataFromGameAsync("CORE_WEAR", cancellationToken);
                    var stateTask = _nucleares.LoadDataFromGameAsync(
                        "CORE_STATE",
                        cancellationToken
                    );
                    var stateCriticalityTask = _nucleares.LoadDataFromGameAsync(
                        "CORE_STATE_CRITICALITY",
                        cancellationToken
                    );
                    var criticalMassReachedTask = _nucleares.LoadDataFromGameAsync(
                        "CORE_CRITICAL_MASS_REACHED",
                        cancellationToken
                    );
                    var criticalMassReachedCounterTask = _nucleares.LoadDataFromGameAsync(
                        "CORE_CRITICAL_MASS_REACHED_COUNTER",
                        cancellationToken
                    );
                    var imminentFusionTask = _nucleares.LoadDataFromGameAsync(
                        "CORE_IMMINENT_FUSION",
                        cancellationToken
                    );
                    var readyForStartTask = _nucleares.LoadDataFromGameAsync(
                        "CORE_READY_FOR_START",
                        cancellationToken
                    );
                    var steamPresentTask = _nucleares.LoadDataFromGameAsync(
                        "CORE_STEAM_PRESENT",
                        cancellationToken
                    );
                    var highSteamPresentTask = _nucleares.LoadDataFromGameAsync(
                        "CORE_HIGH_STEAM_PRESENT",
                        cancellationToken
                    );
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
                    );
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
                    return this;
                }

                public Cores RefreshAllData(CancellationToken cancellationToken = default) =>
                    Task.Run(() => RefreshAllDataAsync(cancellationToken)).GetAwaiter().GetResult();
            }
        }
    }
}
