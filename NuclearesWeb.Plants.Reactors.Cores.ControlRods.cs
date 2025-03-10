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
                public class ControlRods(NuclearesWeb nucleares)
                {
                    [JsonIgnore]
                    private readonly NuclearesWeb _nucleares = nucleares;
                }
            }
        }
    }
}
