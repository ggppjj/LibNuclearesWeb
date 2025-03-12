using System.Text.Json.Serialization;
using LibNuclearesWeb.NuclearesWeb.Plant.Reactor.Core;

namespace LibNuclearesWeb.NuclearesWeb.Plant.Reactor;

public partial class ReactorModel
{
    [JsonIgnore]
    private NuclearesWeb? _nuclearesWeb;
    public CoreModel MainCore { get; } = new();

    public ReactorModel() { }

    internal ReactorModel(NuclearesWeb nucleares)
    {
        _nuclearesWeb = nucleares;
    }

    public ReactorModel Init(NuclearesWeb nucleares)
    {
        _nuclearesWeb = nucleares;
        MainCore.Init(nucleares);
        return this;
    }

    public async Task<ReactorModel> RefreshAllDataAsync(
        CancellationToken cancellationToken = default
    )
    {
        await MainCore.RefreshAllDataAsync(cancellationToken);
        return this;
    }

    public ReactorModel RefreshAllData() => RefreshAllDataAsync().GetAwaiter().GetResult();
}
