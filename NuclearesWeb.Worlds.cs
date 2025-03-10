using System.Text.Json.Serialization;

namespace LibNuclearesWeb;

public partial class NuclearesWeb
{
    public class Worlds
    {
        [JsonIgnore]
        private readonly NuclearesWeb _nucleares;

        public string Time { get; private set; }
        public string TimeStamp { get; private set; }

        internal Worlds(NuclearesWeb nucleares)
        {
            _nucleares = nucleares;
            if (_nucleares.AutoRefresh)
            {
                Time = _nucleares.LoadDataFromGame("TIME");
                TimeStamp = _nucleares.LoadDataFromGame("TIME_STAMP");
            }
            else
            {
                Time = "";
                TimeStamp = "";
            }
        }

        public void RefreshAllData()
        {
            Time = _nucleares.LoadDataFromGame("TIME");
            TimeStamp = _nucleares.LoadDataFromGame("TIME_STAMP");
        }

        public async Task RefreshAllDataAsync(CancellationToken cancellationToken = default)
        {
            Time = await _nucleares.LoadDataFromGameAsync("TIME", cancellationToken);
            TimeStamp = await _nucleares.LoadDataFromGameAsync("TIME_STAMP", cancellationToken);
        }
    }
}
