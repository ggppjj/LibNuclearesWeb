using System.Text.Json.Serialization;

namespace LibNuclearesWeb;

public partial class NuclearesWeb
{
    public partial class Plants
    {
        public partial class Reactors
        {
            public partial class Cores
            {
                public class Coolant
                {
                    [JsonIgnore]
                    private readonly NuclearesWeb? _nucleares;

                    public Coolant() { }

                    internal Coolant(NuclearesWeb nucleares)
                    {
                        _nucleares = nucleares;
                    }
                }
            }
        }
    }
}
