//The MIT License(MIT)

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

namespace LiveCharts.Defaults
{
    public class OhlcPoint : IObservableChartPoint
    {
        private double _open;
        private double _high;
        private double _low;
        private double _close;

        public OhlcPoint()
        {
            
        }

        public OhlcPoint(double open, double high, double low, double close)
        {
            Open = open;
            High = high;
            Low = low;
            Close = close;
        }

        public double Open
        {
            get { return _open; }
            set
            {
                _open = value;
                OnPointChanged();
            }
        }

        public double High
        {
            get { return _high; }
            set
            {
                _high = value;
                OnPointChanged();
            }
        }

        public double Low
        {
            get { return _low; }
            set
            {
                _low = value;
                OnPointChanged();
            }
        }

        public double Close
        {
            get { return _close; }
            set
            {
                _close = value;
                OnPointChanged();
            }
        }

        public event Action PointChanged;

        protected virtual void OnPointChanged()
        {
            if (PointChanged != null) PointChanged.Invoke();
        }
    }
}
