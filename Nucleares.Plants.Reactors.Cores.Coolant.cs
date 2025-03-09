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
                public class Coolant(Nucleares nucleares)
                {
                    [JsonIgnore]
                    private readonly Nucleares _nucleares = nucleares;
                }
            }
        }
    }
}
