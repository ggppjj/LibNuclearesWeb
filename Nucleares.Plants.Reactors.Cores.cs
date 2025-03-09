using System.Text.Json.Serialization;

namespace LibNuclearesWeb;

public partial class Nucleares
{
    public partial class Plants
    {
        public partial class Reactors
        {
            public partial class Cores
            {
                [JsonIgnore]
                private readonly Nucleares _nucleares;

                private ControlRods _controlRodBundles = new();

                [JsonInclude]
                public ControlRods ControlRodBundles
                {
                    get => _controlRodBundles;
                    private set => _controlRodBundles = value;
                }

                private Coolant _coolantStatus = new();

                [JsonInclude]
                public Coolant CoolantStatus
                {
                    get => _coolantStatus;
                    private set => _coolantStatus = value;
                }

                public Cores() { }

                internal Cores(Nucleares nucleares)
                {
                    _nucleares = nucleares;
                    ControlRodBundles = new(nucleares);
                    CoolantStatus = new(nucleares);
                }
            }
        }
    }
}
