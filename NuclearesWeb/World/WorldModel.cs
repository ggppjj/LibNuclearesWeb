using System.Globalization;
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
    private TimeOnly _time = new();

    [JsonInclude]
    public TimeOnly Time
    {
        get => _time;
        private set => SetPropertyAndNotify(ref _time, value);
    }

    private int _timeStamp = 0;

    [JsonInclude]
    public int TimeStamp
    {
        get => _timeStamp;
        private set
        {
            var updated = SetPropertyAndNotify(ref _timeStamp, value);
            if (updated)
                OnPropertyChanged(nameof(CurrentDay));
        }
    }

    [JsonIgnore]
    public int CurrentDay => _timeStamp / 60 / 24;

    #endregion

    /// <summary>
    /// Empty constructor for deserialization. Call Init() afterwards!
    /// </summary>
    public WorldModel() { }

    internal WorldModel(NuclearesWeb nuclearesWeb)
    {
        _nuclearesWeb = nuclearesWeb;
        if (_nuclearesWeb.AutoRefresh)
            _ = RefreshAllData();
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
        return _nuclearesWeb.AutoRefresh
            ? RefreshAllDataAsync(cancellationToken)
            : Task.FromResult(this);
    }

    /// <summary>
    /// Refresh all data. Synchronous. <br/>
    /// Prefer RefreshAllDataAsync() where possible!
    /// </summary>
    /// <returns>A WorldModel with all data and all child object data updated.</returns>
    public WorldModel RefreshAllData() => RefreshAllDataAsync().GetAwaiter().GetResult();

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
        Time = TimeOnly.ParseExact(
            await _nuclearesWeb.GetDataFromGameAsync("TIME", cancellationToken),
            "HH:mm",
            CultureInfo.InvariantCulture
        );
        TimeStamp = int.Parse(
            await _nuclearesWeb.GetDataFromGameAsync("TIME_STAMP", cancellationToken)
        );
        return this;
    }
}
