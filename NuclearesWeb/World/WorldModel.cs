using System.Text.Json.Serialization;
using LibNuclearesWeb.BaseClasses;

namespace LibNuclearesWeb.NuclearesWeb.World;

/// <summary>
/// Essentially just the time and timestamp, AKA number of days since day 0.
/// </summary>
public class WorldModel : MinObservableObject
{
    private NuclearesWeb? _nuclearesWeb;

    #region Notifiable Properties.
    private string _time = string.Empty;
    
    [JsonInclude]
    public string Time
    {
        get => _time;
        private set => SetPropertyAndNotify(ref _time, value);
    }

    private string _timeStamp = string.Empty;

    [JsonInclude]
    public string TimeStamp
    {
        get => _timeStamp;
        private set => SetPropertyAndNotify(ref _timeStamp, value);
    }
    #endregion
    
    /// <summary>
    /// Empty constructor for deserialization. Call Init() afterwords!
    /// </summary>
    public WorldModel() { }

    internal WorldModel(NuclearesWeb nuclearesWeb)
    {
        _nuclearesWeb = nuclearesWeb;
        if (_nuclearesWeb.AutoRefresh)
            RefreshAllData();
    }

    /// <summary>
    /// Initialize dependencies. Synchronous, prefer InitAsync instead.
    /// </summary>
    /// <param name="nuclearesWeb">NuclearesWeb dependency.</param>
    /// <returns>An initialized WorldModel.</returns>
    public WorldModel Init(NuclearesWeb nuclearesWeb) =>
        InitAsync(nuclearesWeb).GetAwaiter().GetResult();

    /// <summary>
    /// Initialize the NuclearesWeb object after deserialization.
    /// </summary>
    /// <param name="nuclearesWeb">NuclearesWeb dependency.</param>
    /// <param name="cancellationToken">CancellationToken, optional.</param>
    /// <returns>An initialized WorldModel.</returns>
    public Task<WorldModel> InitAsync(
        NuclearesWeb nuclearesWeb,
        CancellationToken cancellationToken = default
    )
    {
        _nuclearesWeb = nuclearesWeb;
        return _nuclearesWeb.AutoRefresh ? RefreshAllDataAsync(cancellationToken) : Task.FromResult(this);
    }

    /// <summary>
    /// Refresh all data. Synchronous. <br/>
    /// Prefer RefreshAllDataAsync() where possible!
    /// </summary>
    /// <returns>A WorldModel with all data and all child object data updated.</returns>
    public WorldModel RefreshAllData() =>
        RefreshAllDataAsync().GetAwaiter().GetResult();
    
    /// <summary>
    /// Refresh all data.
    /// </summary>
    /// <param name="cancellationToken">CancellationToken, optional.</param>
    /// <returns>A Task with a refreshed WorldModel as the Result.</returns>
    /// <exception cref="InvalidOperationException">Run Init()</exception>
    public async Task<WorldModel> RefreshAllDataAsync(CancellationToken cancellationToken = default)
    {
        if (_nuclearesWeb == null)
            throw new InvalidOperationException("NuclearesWeb object not set");
        Time = await _nuclearesWeb.GetDataFromGameAsync("TIME", cancellationToken);
        TimeStamp = await _nuclearesWeb.GetDataFromGameAsync("TIME_STAMP", cancellationToken);
        return this;
    }
}
