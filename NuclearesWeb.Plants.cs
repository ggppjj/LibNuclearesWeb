using System.Text.Json.Serialization;

namespace LibNuclearesWeb;

public partial class NuclearesWeb
{
    public partial class Plants(NuclearesWeb nuclearesWeb)
    {
        [JsonIgnore]
        private readonly NuclearesWeb _nuclearesWeb = nuclearesWeb;
        public Reactors MainReactor { get; } = new();
        public List<SteamGenerators> SteamGeneratorList { get; } = [];

        public void RefreshAllData(CancellationToken cancellationToken = default)
        {
            MainReactor.RefreshAllData(cancellationToken);
            foreach (var steamGenerator in SteamGeneratorList)
                steamGenerator.RefreshAllData(cancellationToken);
        }

        public async Task RefreshAllDataAsync(CancellationToken cancellationToken = default)
        {
            await MainReactor.RefreshAllDataAsync(cancellationToken);
            foreach (var generator in SteamGeneratorList)
                await generator.RefreshAllDataAsync(cancellationToken);
        }
    }
}
