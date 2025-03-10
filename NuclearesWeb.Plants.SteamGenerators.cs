using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace LibNuclearesWeb;

public partial class NuclearesWeb
{
    public partial class Plants
    {
        public class SteamGenerators : INotifyPropertyChanged
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
                private set
                {
                    if (_activePowerKW != value)
                    {
                        _activePowerKW = value;
                        OnPropertyChanged();
                    }
                }
            }

            private string _activePowerV = string.Empty;

            [JsonInclude]
            public string ActivePowerV
            {
                get => _activePowerV;
                private set
                {
                    if (_activePowerV != value)
                    {
                        _activePowerV = value;
                        OnPropertyChanged();
                    }
                }
            }

            private string _activePowerA = string.Empty;

            [JsonInclude]
            public string ActivePowerA
            {
                get => _activePowerA;
                private set
                {
                    if (_activePowerA != value)
                    {
                        _activePowerA = value;
                        OnPropertyChanged();
                    }
                }
            }

            private string _activePowerHz = string.Empty;

            [JsonInclude]
            public string ActivePowerHz
            {
                get => _activePowerHz;
                private set
                {
                    if (_activePowerHz != value)
                    {
                        _activePowerHz = value;
                        OnPropertyChanged();
                    }
                }
            }

            private string _breakerStatus = string.Empty;

            [JsonInclude]
            public string BreakerStatus
            {
                get => _breakerStatus;
                private set
                {
                    if (_breakerStatus != value)
                    {
                        _breakerStatus = value;
                        OnPropertyChanged();
                    }
                }
            }

            public event PropertyChangedEventHandler? PropertyChanged;

            protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }

            public static async Task<SteamGenerators> CreateAsync(
                NuclearesWeb nucleares,
                int generatorId,
                CancellationToken cancellationToken = default
            )
            {
                var instance = new SteamGenerators(nucleares, generatorId);
                return await instance.RefreshAllDataAsync(cancellationToken);
            }

            public SteamGenerators() { }

            internal SteamGenerators(NuclearesWeb nucleares, int generatorId)
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

            public async Task<SteamGenerators> RefreshAllDataAsync(
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

            public SteamGenerators RefreshAllData(CancellationToken cancellationToken = default) =>
                Task.Run(() => RefreshAllDataAsync(cancellationToken)).GetAwaiter().GetResult();
        }
    }
}
