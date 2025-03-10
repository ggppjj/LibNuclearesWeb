using System.Text.Json.Serialization;

namespace LibNuclearesWeb.NuclearesWeb.Plant.Reactor.Core.Coolant;

public class Pump
{
    [JsonIgnore]
    private readonly NuclearesWeb? _nuclearesWeb;

    public Pump() { }

    internal Pump(NuclearesWeb nuclearesWeb)
    {
        _nuclearesWeb = nuclearesWeb;
    }
}
