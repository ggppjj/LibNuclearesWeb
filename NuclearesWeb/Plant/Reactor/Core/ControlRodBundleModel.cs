using System.Text.Json.Serialization;
using LibNuclearesWeb.BaseClasses;

namespace LibNuclearesWeb.NuclearesWeb.Plant.Reactor.Core;

public class ControlRodBundleModel : MinObservableObject
{
    private NuclearesWeb? _nuclearesWeb;
    private bool _userHasSetPosition = false;

    #region Notifiable Properties.
    private string _status = string.Empty;

    [JsonInclude]
    public string Status
    {
        get => _status;
        private set => SetPropertyAndNotify(ref _status, value);
    }

    private string _movementSpeed = string.Empty;

    [JsonInclude]
    public string MovementSpeed
    {
        get => _movementSpeed;
        private set => SetPropertyAndNotify(ref _movementSpeed, value);
    }

    private string _movementSpeedDecreasedHighTemperature = string.Empty;

    [JsonInclude]
    public string MovementSpeedDecreasedHighTemperature
    {
        get => _movementSpeedDecreasedHighTemperature;
        private set => SetPropertyAndNotify(ref _movementSpeedDecreasedHighTemperature, value);
    }

    private string _deformed = string.Empty;

    [JsonInclude]
    public string Deformed
    {
        get => _deformed;
        private set => SetPropertyAndNotify(ref _deformed, value);
    }

    private string _temperature = string.Empty;

    [JsonInclude]
    public string Temperature
    {
        get => _temperature;
        private set => SetPropertyAndNotify(ref _temperature, value);
    }

    private string _maxTemperature = string.Empty;

    [JsonInclude]
    public string MaxTemperature
    {
        get => _maxTemperature;
        private set => SetPropertyAndNotify(ref _maxTemperature, value);
    }

    private string _orderedPosition = string.Empty;

    [JsonInclude]
    public string OrderedPosition
    {
        get => _userHasSetPosition ? _orderedPosition : _actualPosition;
        set
        {
            var valueChanged = false;
            if (!String.IsNullOrWhiteSpace(value))
                valueChanged = SetPropertyAndNotify(ref _orderedPosition, value);
            if (_nuclearesWeb == null)
                throw new InvalidDataException("Run Init() first!");
            if (valueChanged && _orderedPosition != string.Empty)
            {
                _ = _nuclearesWeb.SetDataToGameAsync("RODS_POS_ORDERED", value);
                if (!_userHasSetPosition)
                    _userHasSetPosition = true;
            }
        }
    }

    private string _actualPosition = string.Empty;

    [JsonInclude]
    public string ActualPosition
    {
        get => _actualPosition;
        private set
        {
            if (SetPropertyAndNotify(ref _actualPosition, value))
            {
                if (!_userHasSetPosition)
                    OnPropertyChanged(nameof(OrderedPosition));
            }
        }
    }

    private string _reachedPosition = string.Empty;

    [JsonInclude]
    public string ReachedPosition
    {
        get => _reachedPosition;
        private set => SetPropertyAndNotify(ref _reachedPosition, value);
    }

    private string _quantity = string.Empty;

    [JsonInclude]
    public string Quantity
    {
        get => _quantity;
        private set => SetPropertyAndNotify(ref _quantity, value);
    }

    private string _aligned = string.Empty;

    [JsonInclude]
    public string Aligned
    {
        get => _aligned;
        private set => SetPropertyAndNotify(ref _aligned, value);
    }
    #endregion

    public ControlRodBundleModel() { }

    internal ControlRodBundleModel(NuclearesWeb nuclearesWeb) => _nuclearesWeb = nuclearesWeb;

    public ControlRodBundleModel LoadAllData(
        string status,
        string movementSpeed,
        string movementSpeedDecreasedHighTemperature,
        string deformed,
        string temperature,
        string maxTemperature,
        string orderedPosition,
        string actualPosition,
        string reachedPosition,
        string quantity,
        string aligned
    )
    {
        Status = status;
        MovementSpeed = movementSpeed;
        MovementSpeedDecreasedHighTemperature = movementSpeedDecreasedHighTemperature;
        Deformed = deformed;
        Temperature = temperature;
        MaxTemperature = maxTemperature;
        OrderedPosition = orderedPosition;
        ActualPosition = actualPosition;
        ReachedPosition = reachedPosition;
        Quantity = quantity;
        Aligned = aligned;
        return this;
    }

    public ControlRodBundleModel Init(NuclearesWeb nuclearesWeb)
    {
        _nuclearesWeb = nuclearesWeb;
        return this;
    }

    public ControlRodBundleModel RefreshAllData() =>
        RefreshAllDataAsync().ConfigureAwait(false).GetAwaiter().GetResult();

    public async Task<ControlRodBundleModel> RefreshAllDataAsync(
        CancellationToken cancellationToken = default
    )
    {
        if (_nuclearesWeb == null)
            throw new InvalidOperationException("NuclearesWeb is not initialized");
        #region Tasks.
        var statusTask = _nuclearesWeb.GetDataFromGameAsync("RODS_STATUS", cancellationToken);
        var movementSpeedTask = _nuclearesWeb.GetDataFromGameAsync(
            "RODS_MOVEMENT_SPEED",
            cancellationToken
        );
        var movementSpeedDecreasedHighTemperatureTask = _nuclearesWeb.GetDataFromGameAsync(
            "RODS_MOVEMENT_SPEED_DECREASED_HIGH_TEMPERATURE",
            cancellationToken
        );
        var deformedTask = _nuclearesWeb.GetDataFromGameAsync("RODS_DEFORMED", cancellationToken);
        var temperatureTask = _nuclearesWeb.GetDataFromGameAsync(
            "RODS_TEMPERATURE",
            cancellationToken
        );
        var maxTemperatureTask = _nuclearesWeb.GetDataFromGameAsync(
            "RODS_MAX_TEMPERATURE",
            cancellationToken
        );
        var orderedPositionTask = _nuclearesWeb.GetDataFromGameAsync(
            "RODS_POS_ORDERED",
            cancellationToken
        );
        var actualPositionTask = _nuclearesWeb.GetDataFromGameAsync(
            "RODS_POS_ACTUAL",
            cancellationToken
        );
        var reachedPositionTask = _nuclearesWeb.GetDataFromGameAsync(
            "RODS_POS_REACHED",
            cancellationToken
        );
        var quantityTask = _nuclearesWeb.GetDataFromGameAsync("RODS_QUANTITY", cancellationToken);
        var alignedTask = _nuclearesWeb.GetDataFromGameAsync("RODS_ALIGNED", cancellationToken);
        #endregion
        _ = await Task.WhenAll(
                statusTask,
                movementSpeedTask,
                movementSpeedDecreasedHighTemperatureTask,
                deformedTask,
                temperatureTask,
                maxTemperatureTask,
                orderedPositionTask,
                actualPositionTask,
                reachedPositionTask,
                quantityTask,
                alignedTask
            )
            .ConfigureAwait(false);
        return LoadAllData(
            statusTask.Result,
            movementSpeedTask.Result,
            movementSpeedDecreasedHighTemperatureTask.Result,
            deformedTask.Result,
            temperatureTask.Result,
            maxTemperatureTask.Result,
            orderedPositionTask.Result,
            actualPositionTask.Result,
            reachedPositionTask.Result,
            quantityTask.Result,
            alignedTask.Result
        );
    }
}
