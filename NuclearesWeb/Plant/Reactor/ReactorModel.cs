using System.Text.Json.Serialization;
using LibNuclearesWeb.NuclearesWeb.Plant.Reactor.Core;

namespace LibNuclearesWeb.NuclearesWeb.Plant.Reactor;

public partial class ReactorModel
{
    [JsonInclude]
    public CoreModel MainCore { get; } = new();

    public ReactorModel() { }

    internal ReactorModel(NuclearesWeb nuclearesWeb) => Init(nuclearesWeb);

    public ReactorModel Init(NuclearesWeb nuclearesWeb)
    {
        _ = MainCore.Init(nuclearesWeb);
        return this;
    }

    public async Task<ReactorModel> RefreshAllDataAsync(
        CancellationToken cancellationToken = default
    )
    {
        _ = await MainCore.RefreshAllDataAsync(cancellationToken);
        return this;
    }

    public ReactorModel RefreshAllData() => RefreshAllDataAsync().GetAwaiter().GetResult();
}
