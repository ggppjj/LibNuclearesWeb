using System.Text.Json.Serialization;

namespace LibNuclearesWeb;

public partial class NuclearesWeb
{
    public partial class Plants
    {
        public partial class Reactors
        {
            [JsonIgnore]
            private readonly NuclearesWeb? _nucleares;
            public Cores Core { get; } = new();

            public Reactors() { }

            internal Reactors(NuclearesWeb nucleares)
            {
                _nucleares = nucleares;
            }
        }
    }
}
