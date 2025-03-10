using System.Text.Json.Serialization;

namespace LibNuclearesWeb;

public partial class NuclearesWeb
{
    public partial class Plant
    {
        public partial class Reactor
        {
            public partial class Core
            {
                public partial class Coolant
                {
                    [JsonIgnore]
                    private readonly NuclearesWeb? _nuclearesWeb;

                    [JsonInclude]
                    public List<Pump> PumpList { get; private set; } = [];

                    public Coolant()
                    {
                        PumpList.Add(new());
                        PumpList.Add(new());
                        PumpList.Add(new());
                    }

                    internal Coolant(NuclearesWeb nuclearesWeb)
                    {
                        _nuclearesWeb = nuclearesWeb;
                        PumpList.Add(new(_nuclearesWeb));
                        PumpList.Add(new(_nuclearesWeb));
                        PumpList.Add(new(_nuclearesWeb));
                    }
                }
            }
        }
    }
}
