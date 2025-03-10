using System.Text.Json.Serialization;

namespace LibNuclearesWeb;

public partial class NuclearesWeb
{
    public partial class Plant
    {
        [JsonIgnore]
        private NuclearesWeb? _nuclearesWeb;

        [JsonInclude]
        public Reactor MainReactor { get; } = new();

        [JsonInclude]
        public List<SteamGenerator> SteamGeneratorList { get; } = [];

        public Plant()
        {
            SteamGeneratorList.Add(new(0));
            SteamGeneratorList.Add(new(1));
            SteamGeneratorList.Add(new(2));
        }

        public void Init(NuclearesWeb nuclearesWeb)
        {
            _nuclearesWeb = nuclearesWeb;
            MainReactor.Init(nuclearesWeb);
            foreach (var generator in SteamGeneratorList)
                generator.Init(nuclearesWeb);
        }

        internal Plant(NuclearesWeb nuclearesWeb)
        {
            _nuclearesWeb = nuclearesWeb;
            MainReactor = new(nuclearesWeb);
            SteamGeneratorList.Add(new(nuclearesWeb, 0));
            SteamGeneratorList.Add(new(nuclearesWeb, 1));
            SteamGeneratorList.Add(new(nuclearesWeb, 2));
        }

        public Plant RefreshAllData(CancellationToken cancellationToken = default) =>
            Task.Run(() => RefreshAllDataAsync(cancellationToken)).GetAwaiter().GetResult();

        public async Task<Plant> RefreshAllDataAsync(CancellationToken cancellationToken = default)
        {
            await MainReactor.RefreshAllDataAsync(cancellationToken);
            foreach (var generator in SteamGeneratorList)
                await generator.RefreshAllDataAsync(cancellationToken);
            return this;
        }
    }
}
