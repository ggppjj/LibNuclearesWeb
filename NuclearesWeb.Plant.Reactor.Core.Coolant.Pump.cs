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
                }
            }
        }
    }
}
