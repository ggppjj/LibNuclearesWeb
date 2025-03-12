using System.Text.Json.Serialization;
using LibNuclearesWeb.BaseClasses;

namespace LibNuclearesWeb.NuclearesWeb.Plant;

public class SteamGeneratorModel : MinObservableObject
{
    [JsonIgnore]
    private NuclearesWeb? _nucleares;

    [JsonInclude]
    public int GeneratorId { get; private set; } = -1;

    private string _activePowerKW = string.Empty;

    [JsonInclude]
    public string ActivePowerKW
    {
        get => _activePowerKW;
        private set => SetPropertyAndNotify(ref _activePowerKW, value);
    }

    private string _activePowerV = string.Empty;

    [JsonInclude]
    public string ActivePowerV
    {
        get => _activePowerV;
        private set => SetPropertyAndNotify(ref _activePowerV, value);
    }

    private string _activePowerA = string.Empty;

    [JsonInclude]
    public string ActivePowerA
    {
        get => _activePowerA;
        private set => SetPropertyAndNotify(ref _activePowerA, value);
    }

    private string _activePowerHz = string.Empty;

    [JsonInclude]
    public string ActivePowerHz
    {
        get => _activePowerHz;
        private set => SetPropertyAndNotify(ref _activePowerHz, value);
    }

    private string _breakerStatus = string.Empty;

    [JsonInclude]
    public string BreakerStatus
    {
        get => _breakerStatus;
        private set => SetPropertyAndNotify(ref _breakerStatus, value);
    }

    public static async Task<SteamGeneratorModel> CreateAsync(
        NuclearesWeb nucleares,
        int generatorId,
        CancellationToken cancellationToken = default
    )
    {
        var instance = new SteamGeneratorModel(nucleares, generatorId);
        return await instance.RefreshAllDataAsync(cancellationToken);
    }

    public SteamGeneratorModel() { }

    internal SteamGeneratorModel(int generatorId) => GeneratorId = generatorId;

    internal SteamGeneratorModel(NuclearesWeb nucleares, int generatorId)
    {
        _nucleares = nucleares;
        GeneratorId = generatorId;
        if (_nucleares.AutoRefresh)
            RefreshAllData();
    }

    private SteamGeneratorModel SetAllData(string kw, string v, string a, string hz, string breaker)
    {
        ActivePowerKW = kw;
        ActivePowerV = v;
        ActivePowerA = a;
        ActivePowerHz = hz;
        BreakerStatus = breaker;
        return this;
    }

    public SteamGeneratorModel Init(NuclearesWeb nucleares) =>
        InitAsync(nucleares).GetAwaiter().GetResult();

    public Task<SteamGeneratorModel> InitAsync(
        NuclearesWeb nucleares,
        CancellationToken cancellationToken = default
    )
    {
        _nucleares = nucleares;
        if (_nucleares.AutoRefresh)
            return RefreshAllDataAsync(cancellationToken);
        return Task.FromResult(this);
    }

    public SteamGeneratorModel RefreshAllData(CancellationToken cancellationToken = default) =>
        RefreshAllDataAsync(cancellationToken).ConfigureAwait(false).GetAwaiter().GetResult();

    public async Task<SteamGeneratorModel> RefreshAllDataAsync(
        CancellationToken cancellationToken = default
    )
    {
        if (_nucleares == null)
            throw new InvalidOperationException(
                "NuclearesWeb object is null. Run Init with a valid NuclearesWeb instance first!"
            );
        var kwTask = _nucleares.GetDataFromGameAsync(
            $"GENERATOR_{GeneratorId}_KW",
            cancellationToken
        );
        var vTask = _nucleares.GetDataFromGameAsync(
            $"GENERATOR_{GeneratorId}_V",
            cancellationToken
        );
        var aTask = _nucleares.GetDataFromGameAsync(
            $"GENERATOR_{GeneratorId}_A",
            cancellationToken
        );
        var hzTask = _nucleares.GetDataFromGameAsync(
            $"GENERATOR_{GeneratorId}_HERTZ",
            cancellationToken
        );
        var breakerTask = _nucleares.GetDataFromGameAsync(
            $"GENERATOR_{GeneratorId}_BREAKER",
            cancellationToken
        );
        await Task.WhenAll(kwTask, vTask, aTask, hzTask, breakerTask).ConfigureAwait(false);
        SetAllData(kwTask.Result, vTask.Result, aTask.Result, hzTask.Result, breakerTask.Result);
        return this;
    }
}
