using System.Diagnostics;
using LibNuclearesWeb.BaseClasses;

namespace LibNuclearesWeb.Extensions;

public class ObservableStopwatch : MinObservableObject, IDisposable
{
    private readonly Stopwatch _stopwatch = new();
    private readonly Timer _timer;
    private readonly TimeSpan _updateInterval = TimeSpan.FromMilliseconds(100);

    public ObservableStopwatch() =>
        // Create a timer to update the Elapsed property periodically.
        _timer = new Timer(UpdateElapsed, null, _updateInterval, _updateInterval);

    /// <summary>
    /// The elapsed time of the stopwatch.
    /// </summary>
    public TimeSpan Elapsed => _stopwatch.Elapsed;

    /// <summary>
    /// Starts or resumes measuring elapsed time.
    /// </summary>
    public void Start()
    {
        _stopwatch.Start();
        OnPropertyChanged(nameof(Elapsed));
    }

    /// <summary>
    /// Stops measuring elapsed time.
    /// </summary>
    public void Stop()
    {
        _stopwatch.Stop();
        OnPropertyChanged(nameof(Elapsed));
    }

    /// <summary>
    /// Resets the elapsed time to zero and starts measuring elapsed time.
    /// </summary>
    public void Restart()
    {
        _stopwatch.Restart();
        OnPropertyChanged(nameof(Elapsed));
    }

    /// <summary>
    /// Periodically called by the timer to update the elapsed time.
    /// </summary>
    /// <param name="state"></param>
    private void UpdateElapsed(object? state)
    {
        if (_stopwatch.IsRunning)
            OnPropertyChanged(nameof(Elapsed));
    }

    public void Dispose()
    {
        _timer.Dispose();
        GC.SuppressFinalize(this);
    }
}
