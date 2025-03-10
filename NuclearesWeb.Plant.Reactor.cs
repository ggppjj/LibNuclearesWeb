using System.Text.Json.Serialization;

namespace LibNuclearesWeb;

public partial class NuclearesWeb
{
    public partial class Plant
    {
        public partial class Reactor
        {
            [JsonIgnore]
            private NuclearesWeb? _nuclearesWeb;
            public Core MainCore { get; } = new();

            public Reactor() { }

            internal Reactor(NuclearesWeb nucleares)
            {
                _nuclearesWeb = nucleares;
            }

            public void Init(NuclearesWeb nuclearesWeb)
            {
                _nuclearesWeb = nuclearesWeb;
                MainCore.Init(nuclearesWeb);
            }

            public async Task<Reactor> RefreshAllDataAsync(
                CancellationToken cancellationToken = default
            )
            {
                await MainCore.RefreshAllDataAsync(cancellationToken);
                return this;
            }

            public Reactor RefreshAllData(CancellationToken cancellationToken = default) =>
                Task.Run(() => RefreshAllDataAsync(cancellationToken)).GetAwaiter().GetResult();
        }
    }
}
