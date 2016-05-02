//copyright(c) 2016 Alberto Rodriguez

//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.

using System;
using System.Threading;
using System.Threading.Tasks;

namespace LiveChartsCore
{
    public class LvcTimer
    {
        public LvcTimer()
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