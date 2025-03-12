using System.Text.Json.Serialization;
using LibNuclearesWeb.BaseClasses;

namespace LibNuclearesWeb.NuclearesWeb.Plant.Reactor.Core.Coolant;

/// <summary>
/// The model for the coolant pumps in the reactor core.
/// </summary>
public class PumpModel : MinObservableObject
{
    private NuclearesWeb? _nuclearesWeb;

    /// <summary>
    /// The ID of the pump.
    /// </summary>
    [JsonInclude]
    public int Id { get; set; } = -1;

    #region Notifiable Properties.
    private string _status = string.Empty;

    [JsonInclude]
    public string Status
    {
        get => _status;
        set => SetPropertyAndNotify(ref _status, value);
    }
    private string _dryStatus = string.Empty;

    [JsonInclude]
    public string DryStatus
    {
        get => _dryStatus;
        set => SetPropertyAndNotify(ref _dryStatus, value);
    }

    private string _overloadStatus = string.Empty;

    [JsonInclude]
    public string OverloadStatus
    {
        get => _overloadStatus;
        set => SetPropertyAndNotify(ref _overloadStatus, value);
    }

    private string _orderedSpeed = string.Empty;

    [JsonInclude]
    public string OrderedSpeed
    {
        get => _orderedSpeed;
        set => SetPropertyAndNotify(ref _orderedSpeed, value);
    }

    private string _speed = string.Empty;

    [JsonInclude]
    public string Speed
    {
        get => _speed;
        set => SetPropertyAndNotify(ref _speed, value);
    }
    #endregion

    /// <summary>
    /// Empty constructor for PumpModel for serialization purposes. Do not use this constructor without then calling Init() to satisfy dependencies.
    /// </summary>
    public PumpModel() { }

    /// <summary>
    /// Constructor for PumpModel.
    /// </summary>
    /// <param name="id">The ID of the pump.</param>
    public PumpModel(int id) => Id = id;

    /// <summary>
    /// Constructor for PumpModel. Internal class.
    /// </summary>
    /// <param name="id">The ID of the pump.</param>
    /// <param name="nuclearesWeb">NuclearesWeb dependency.</param>
    internal PumpModel(int id, NuclearesWeb nuclearesWeb)
    {
        Id = id;
        _nuclearesWeb = nuclearesWeb;
        if (_nuclearesWeb.AutoRefresh)
            RefreshAllData();
    }

    /// <summary>
    /// Set all the data for the Pump.
    /// </summary>
    /// <param name="status"></param>
    /// <param name="dryStatus"></param>
    /// <param name="overloadStatus"></param>
    /// <param name="orderedSpeed"></param>
    /// <param name="speed"></param>
    private void SetAllData(
        string status,
        string dryStatus,
        string overloadStatus,
        string orderedSpeed,
        string speed
    )
    {
        Status = status;
        DryStatus = dryStatus;
        OverloadStatus = overloadStatus;
        OrderedSpeed = orderedSpeed;
        Speed = speed;
    }

    /// <summary>
    /// Initialize the PumpModel with the NuclearesWeb dependency.
    /// </summary>
    /// <param name="nuclearesWeb">An instance of NuclearesWeb.</param>
    /// <returns></returns>
    public PumpModel Init(NuclearesWeb nuclearesWeb)
    {
        _nuclearesWeb = nuclearesWeb;
        return this;
    }

    /// <summary>
    /// Refresh all the data for the PumpModel. This is a synchronous method. Use RefreshAllDataAsync() for asynchronous.
    /// </summary>
    /// <returns>A PumpModel post-refresh.</returns>
    public PumpModel RefreshAllData() => RefreshAllDataAsync().GetAwaiter().GetResult();

    /// <summary>
    /// Refresh all the data for the PumpModel.
    /// </summary>
    /// <param name="cancellationToken">CancellationToken, optional</param>
    /// <returns>A Task that returns the updated PumpModel.</returns>
    /// <exception cref="InvalidOperationException">Init() not called!</exception>
    public async Task<PumpModel> RefreshAllDataAsync(CancellationToken cancellationToken = default)
    {
        if (_nuclearesWeb == null)
            throw new InvalidOperationException("NuclearesWeb object is null");
        #region Tasks.
        var statusTask = _nuclearesWeb.GetDataFromGameAsync(
            $"COOLANT_CORE_CIRCULATION_PUMP_{Id}_STATUS",
            cancellationToken
        );
        var dryStatusTask = _nuclearesWeb.GetDataFromGameAsync(
            $"COOLANT_CORE_CIRCULATION_PUMP_{Id}_DRY_STATUS",
            cancellationToken
        );
        var overloadStatusTask = _nuclearesWeb.GetDataFromGameAsync(
            $"COOLANT_CORE_CIRCULATION_PUMP_{Id}_OVERLOAD_STATUS",
            cancellationToken
        );
        var orderedSpeedTask = _nuclearesWeb.GetDataFromGameAsync(
            $"COOLANT_CORE_CIRCULATION_PUMP_{Id}_ORDERED_SPEED",
            cancellationToken
        );
        var speedTask = _nuclearesWeb.GetDataFromGameAsync(
            $"COOLANT_CORE_CIRCULATION_PUMP_{Id}_SPEED",
            cancellationToken
        );
        #endregion
        await Task.WhenAll(
            statusTask,
            dryStatusTask,
            overloadStatusTask,
            orderedSpeedTask,
            speedTask
        );
        SetAllData(
            statusTask.Result,
            dryStatusTask.Result,
            overloadStatusTask.Result,
            orderedSpeedTask.Result,
            speedTask.Result
        );
        return this;
    }
}
