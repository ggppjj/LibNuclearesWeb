using System.Text.Json.Serialization;

namespace LibNuclearesWeb;

public partial class NuclearesWeb
{
    public partial class Plants
    {
        [JsonIgnore]
        private NuclearesWeb? _nuclearesWeb;

        [JsonInclude]
        public Reactors MainReactor { get; } = new();

        [JsonInclude]
        public List<SteamGenerators> SteamGeneratorList { get; } = [];

        public Plants() { }

        public void Init(NuclearesWeb nuclearesWeb)
        {
            _nuclearesWeb = nuclearesWeb;
            MainReactor.Init(nuclearesWeb);
            foreach (var generator in SteamGeneratorList)
                generator.Init(nuclearesWeb);
        }

        internal Plants(NuclearesWeb nuclearesWeb)
        {
            _nuclearesWeb = nuclearesWeb;
            MainReactor = new(nuclearesWeb);
            SteamGeneratorList.Add(new(nuclearesWeb, 0));
            SteamGeneratorList.Add(new(nuclearesWeb, 1));
            SteamGeneratorList.Add(new(nuclearesWeb, 2));
        }

        public Plants RefreshAllData(CancellationToken cancellationToken = default) =>
            Task.Run(() => RefreshAllDataAsync(cancellationToken)).GetAwaiter().GetResult();

        public async Task<Plants> RefreshAllDataAsync(CancellationToken cancellationToken = default)
        {
            await MainReactor.RefreshAllDataAsync(cancellationToken);
            foreach (var generator in SteamGeneratorList)
                await generator.RefreshAllDataAsync(cancellationToken);
            return this;
        }
    }
}
