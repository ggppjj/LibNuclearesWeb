using System.Text.Json.Serialization;

namespace LibNuclearesWeb.NuclearesWeb.Plant.Reactor.Core.Coolant;

public partial class CoolantModel
{
    [JsonIgnore]
    private readonly NuclearesWeb? _nuclearesWeb;

    [JsonInclude]
    public List<Pump> PumpList { get; private set; } = [];

    public CoolantModel()
    {
        PumpList.Add(new());
        PumpList.Add(new());
        PumpList.Add(new());
    }

    internal CoolantModel(NuclearesWeb nuclearesWeb)
    {
        _nuclearesWeb = nuclearesWeb;
        PumpList.Add(new(_nuclearesWeb));
        PumpList.Add(new(_nuclearesWeb));
        PumpList.Add(new(_nuclearesWeb));
    }
}
