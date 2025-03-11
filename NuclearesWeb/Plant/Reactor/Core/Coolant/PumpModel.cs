using System.Text.Json.Serialization;
using LibNuclearesWeb.BaseClasses;

namespace LibNuclearesWeb.NuclearesWeb.Plant.Reactor.Core.Coolant;

public class PumpModel : MinObservableObject
{
    [JsonIgnore]
    private NuclearesWeb? _nuclearesWeb;

    public int Id { get; set; } = -1;

    public PumpModel() { }

    internal PumpModel(NuclearesWeb nuclearesWeb)
    {
        _nuclearesWeb = nuclearesWeb;
    }

    internal async Task<PumpModel> InitAsync(
        NuclearesWeb nuclearesWeb,
        CancellationToken cancellationToken
    )
    {
        _nuclearesWeb = nuclearesWeb;
        return this;
    }

    internal async Task<PumpModel> RefreshAllDataAsync(CancellationToken cancellationToken)
    {
        if (_nuclearesWeb == null)
            await Task.Delay(1000, cancellationToken);
        return this;
    }
}
