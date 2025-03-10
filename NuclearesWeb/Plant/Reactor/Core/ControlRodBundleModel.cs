using System.Text.Json.Serialization;

namespace LibNuclearesWeb.NuclearesWeb.Plant.Reactor.Core;

public class ControlRodBundleModel
{
    [JsonIgnore]
    private readonly NuclearesWeb? _nuclearesWeb;

    public ControlRodBundleModel() { }

    internal ControlRodBundleModel(NuclearesWeb nuclearesWeb)
    {
        _nuclearesWeb = nuclearesWeb;
    }
}
