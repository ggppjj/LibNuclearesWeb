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

    public CoolantModel RefreshAllData() =>
        RefreshAllDataAsync().ConfigureAwait(false).GetAwaiter().GetResult();

    public async Task<CoolantModel> RefreshAllDataAsync(
        CancellationToken cancellationToken = default
    )
    {
        foreach (var pump in PumpList)
            await pump.RefreshAllDataAsync(cancellationToken);
        return this;
    }
}
