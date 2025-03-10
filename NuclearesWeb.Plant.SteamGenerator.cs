using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace LibNuclearesWeb;

public partial class NuclearesWeb
{
    public partial class Plant
    {
        public class SteamGenerator : INotifyPropertyChanged
        {
            [JsonIgnore]
            private NuclearesWeb? _nucleares;

            [JsonInclude]
            public int GeneratorId { get; private set; } = -1;

            private string _activePowerKW = string.Empty;

            [JsonInclude]
            public string ActivePowerKW
            {
                get => _activePowerKW;
                private set => SetProperty(ref _activePowerKW, value);
            }

            private string _activePowerV = string.Empty;

            [JsonInclude]
            public string ActivePowerV
            {
                get => _activePowerV;
                private set => SetProperty(ref _activePowerV, value);
            }

            private string _activePowerA = string.Empty;

            [JsonInclude]
            public string ActivePowerA
            {
                get => _activePowerA;
                private set => SetProperty(ref _activePowerA, value);
            }

            private string _activePowerHz = string.Empty;

            [JsonInclude]
            public string ActivePowerHz
            {
                get => _activePowerHz;
                private set => SetProperty(ref _activePowerHz, value);
            }

            private string _breakerStatus = string.Empty;

            [JsonInclude]
            public string BreakerStatus
            {
                get => _breakerStatus;
                private set => SetProperty(ref _breakerStatus, value);
            }

            protected bool SetProperty<T>(
                ref T field,
                T value,
                [CallerMemberName] string? propertyName = null
            )
            {
                if (EqualityComparer<T>.Default.Equals(field, value))
                    return false;
                field = value;
                OnPropertyChanged(propertyName);
                return true;
            }

            public event PropertyChangedEventHandler? PropertyChanged;

            protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }

            public static async Task<SteamGenerator> CreateAsync(
                NuclearesWeb nucleares,
                int generatorId,
                CancellationToken cancellationToken = default
            )
            {
                var instance = new SteamGenerator(nucleares, generatorId);
                return await instance.RefreshAllDataAsync(cancellationToken);
            }

            public SteamGenerator() { }

            internal SteamGenerator(int generatorId)
            {
                GeneratorId = generatorId;
            }

            internal SteamGenerator(NuclearesWeb nucleares, int generatorId)
            {
                _nucleares = nucleares;
                GeneratorId = generatorId;
                if (_nucleares.AutoRefresh)
                    RefreshAllData();
            }

            private void SetAllData(string kw, string v, string a, string hz, string breaker)
            {
                ActivePowerKW = kw;
                ActivePowerV = v;
                ActivePowerA = a;
                ActivePowerHz = hz;
                BreakerStatus = breaker;
            }

            public void Init(NuclearesWeb nucleares)
            {
                _nucleares = nucleares;
                if (_nucleares.AutoRefresh)
                    RefreshAllData();
            }

            public async Task<SteamGenerator> RefreshAllDataAsync(
                CancellationToken cancellationToken = default
            )
            {
                if (_nucleares == null)
                    throw new InvalidOperationException("NuclearesWeb object is null");
                var kwTask = _nucleares.LoadDataFromGameAsync(
                    $"GENERATOR_{GeneratorId}_KW",
                    cancellationToken
                );
                var vTask = _nucleares.LoadDataFromGameAsync(
                    $"GENERATOR_{GeneratorId}_V",
                    cancellationToken
                );
                var aTask = _nucleares.LoadDataFromGameAsync(
                    $"GENERATOR_{GeneratorId}_A",
                    cancellationToken
                );
                var hzTask = _nucleares.LoadDataFromGameAsync(
                    $"GENERATOR_{GeneratorId}_HZ",
                    cancellationToken
                );
                var breakerTask = _nucleares.LoadDataFromGameAsync(
                    $"GENERATOR_{GeneratorId}_BREAKER",
                    cancellationToken
                );
                await Task.WhenAll(kwTask, vTask, aTask, hzTask, breakerTask).ConfigureAwait(false);
                SetAllData(
                    kwTask.Result,
                    vTask.Result,
                    aTask.Result,
                    hzTask.Result,
                    breakerTask.Result
                );
                return this;
            }

            public SteamGenerator RefreshAllData(CancellationToken cancellationToken = default) =>
                Task.Run(() => RefreshAllDataAsync(cancellationToken)).GetAwaiter().GetResult();
        }
    }
}
