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
                public class ControlRodBundle
                {
                    [JsonIgnore]
                    private readonly NuclearesWeb? _nuclearesWeb;

                    public ControlRodBundle() { }

                    internal ControlRodBundle(NuclearesWeb nuclearesWeb)
                    {
                        _nuclearesWeb = nuclearesWeb;
                    }
                }
            }
        }
    }
}
