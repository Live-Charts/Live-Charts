using System;
using System.Threading;
using System.Threading.Tasks;

namespace LiveChartsCore
{
    public class Timer
    {
        public Timer()
        {
            Interval = TimeSpan.FromMilliseconds(50);
            _cancellationToken = new CancellationTokenSource();
        }

        public event Action Tick; 
        public TimeSpan Interval { get; set; }

        private readonly CancellationTokenSource _cancellationToken;

        public void Start()
        {
            Task.Delay(Interval, _cancellationToken.Token).ContinueWith(t =>
            {
                if (t.IsCanceled) return;
                if (t.IsCompleted && Tick != null) Tick.Invoke();
            }, _cancellationToken.Token);
        }

        public void Stop()
        {
            _cancellationToken.Cancel();
        }

        public void Restart()
        {
            Stop();
            Start();
        }
    }
}