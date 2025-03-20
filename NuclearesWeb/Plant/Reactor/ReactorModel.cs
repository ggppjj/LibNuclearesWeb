using System.Text.Json.Serialization;
using LibNuclearesWeb.NuclearesWeb.Plant.Reactor.Core;

namespace LibNuclearesWeb.NuclearesWeb.Plant.Reactor;

/// <summary>
/// Mainly this class exists in the event of as-yet unforeseen-by-me expansions to the game.
/// It would be cool to need to expand this.
/// </summary>
public partial class ReactorModel
{
    /// <summary>
    /// The main and currently only core.
    /// </summary>
    [JsonInclude]
    public CoreModel MainCore { get; } = new();

    /// <summary>
    /// Empty constructor for deserialization. Run Init() afterwards!
    /// </summary>
    public ReactorModel() { }

    internal ReactorModel(NuclearesWeb nuclearesWeb) => Init(nuclearesWeb);

    /// <summary>
    /// Initialize the MainCore child object (and all of it's children) with the NuclearesWeb dependency after deserialization.
    /// </summary>
    /// <param name="nuclearesWeb">NuclearesWeb dependency</param>
    public ReactorModel Init(NuclearesWeb nuclearesWeb)
    {
        _ = MainCore.Init(nuclearesWeb);
        return this;
    }

    /// <summary>
    /// Refreshes all data in the MainCore and all child objects. Synchronous.<br/>
    /// Prefer RefreshAllDataAsync() where possible.
    /// </summary>
    /// <returns></returns>
    public ReactorModel RefreshAllData() => RefreshAllDataAsync().GetAwaiter().GetResult();

    /// <summary>
    /// Refreshes all data in the MainCore and all child objects.
    /// </summary>
    /// <param name="cancellationToken">CancellationToken, optional.</param>
    /// <returns>A ReactorModel with an updated MainCore.</returns>
    public async Task<ReactorModel> RefreshAllDataAsync(
        CancellationToken cancellationToken = default
    )
    {
        _ = await MainCore.RefreshAllDataAsync(cancellationToken);
        return this;
    }
}
