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

    public ReactorModel Init(NuclearesWeb nuclearesWeb) =>
        InitAsync(nuclearesWeb).GetAwaiter().GetResult();

    public async Task<ReactorModel> InitAsync(
        NuclearesWeb nucleares,
        CancellationToken cancellationToken = default
    )
    {
        _nuclearesWeb = nucleares;
        await MainCore.InitAsync(nucleares, cancellationToken);
        return this;
    }

    public async Task<ReactorModel> RefreshAllDataAsync(
        CancellationToken cancellationToken = default
    )
    {
        await MainCore.RefreshAllDataAsync(cancellationToken);
        return this;
    }

    public ReactorModel RefreshAllData(CancellationToken cancellationToken = default) =>
        Task.Run(() => RefreshAllDataAsync(cancellationToken)).GetAwaiter().GetResult();
}
