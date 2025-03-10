using System.Text.Json.Serialization;

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

    public void Init(NuclearesWeb nuclearesWeb)
    {
        _nuclearesWeb = nuclearesWeb;
        MainCore.Init(nuclearesWeb);
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
