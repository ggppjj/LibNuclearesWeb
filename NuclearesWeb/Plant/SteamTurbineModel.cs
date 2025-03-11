using System.Text.Json.Serialization;
using LibNuclearesWeb.BaseClasses;

namespace LibNuclearesWeb.NuclearesWeb.Plant;

public partial class SteamTurbineModel : MinObservableObject
{
    [JsonIgnore]
    private NuclearesWeb? _nuclearesWeb;

    [JsonInclude]
    public int TurbineId { get; private set; } = -1;

    private string _rpm = string.Empty;

    [JsonInclude]
    public string Rpm
    {
        get => _rpm;
        private set => SetPropertyAndNotify(ref _rpm, value);
    }

    private string _temperature = string.Empty;

    [JsonInclude]
    public string Temperature
    {
        get => _temperature;
        private set => SetPropertyAndNotify(ref _temperature, value);
    }

    private string _pressure = string.Empty;

    [JsonInclude]
    public string Pressure
    {
        get => _pressure;
        private set => SetPropertyAndNotify(ref _pressure, value);
    }

    public SteamTurbineModel() { }

    public SteamTurbineModel(int turbineId) => TurbineId = turbineId;

    internal SteamTurbineModel(NuclearesWeb nuclearesWeb) => _nuclearesWeb = nuclearesWeb;

    private SteamTurbineModel SetAllData(string rpm, string temp, string pressure)
    {
        Rpm = rpm;
        Temperature = temp;
        Pressure = pressure;
        return this;
    }

    public SteamTurbineModel RefreshAllData(CancellationToken cancellationToken = default) =>
        RefreshAllDataAsync(cancellationToken).GetAwaiter().GetResult();

    public async Task<SteamTurbineModel> RefreshAllDataAsync(
        CancellationToken cancellationToken = default
    )
    {
        if (_nuclearesWeb == null)
        {
            throw new InvalidOperationException(
                "NuclearesWeb object is null. Run Init with a valid NuclearesWeb instance first!"
            );
        }

        var rpmTask = _nuclearesWeb.LoadDataFromGameAsync(
            $"STEAM_TURBINE_{TurbineId}_RPM",
            cancellationToken
        );
        var tempTask = _nuclearesWeb.LoadDataFromGameAsync(
            $"STEAM_TURBINE_{TurbineId}_TEMPERATURE",
            cancellationToken
        );
        var pressureTask = _nuclearesWeb.LoadDataFromGameAsync(
            $"STEAM_TURBINE_{TurbineId}_PRESSURE",
            cancellationToken
        );
        await Task.WhenAll(rpmTask, tempTask, pressureTask).ConfigureAwait(false);
        return SetAllData(rpmTask.Result, tempTask.Result, pressureTask.Result);
    }
}
