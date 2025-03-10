using System.Text.Json.Serialization;

namespace LibNuclearesWeb.NuclearesWeb.Plant.Reactor.Core.Coolant;

public class PumpModel
{
    [JsonIgnore]
    private readonly NuclearesWeb? _nuclearesWeb;

    public PumpModel() { }

    internal PumpModel(NuclearesWeb nuclearesWeb)
    {
        _nuclearesWeb = nuclearesWeb;
    }
}
