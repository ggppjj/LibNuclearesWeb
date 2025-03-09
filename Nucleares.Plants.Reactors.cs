using System.Text.Json.Serialization;

namespace LibNuclearesWeb;

public partial class Nucleares
{
    public partial class Plants
    {
        public partial class Reactors
        {
            [JsonIgnore]
            private readonly Nucleares _nucleares;
            public Cores Core { get; } = new();

            internal Reactors(Nucleares nucleares)
            {
                _nucleares = nucleares;
            }
        }
    }
}
