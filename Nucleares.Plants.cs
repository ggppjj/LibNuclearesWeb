using System.Text.Json.Serialization;

namespace LibNuclearesWeb;

public partial class Nucleares
{
    public partial class Plants(Nucleares nucleares)
    {
        [JsonIgnore]
        private readonly Nucleares _nucleares = nucleares;
        public Reactors MainReactor { get; } = new(nucleares);
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
