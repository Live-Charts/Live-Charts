//The MIT License(MIT)

//Copyright(c) 2016 Alberto Rodriguez & LiveCharts Contributors

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
using System.ComponentModel;

namespace LiveCharts.Defaults
{
    /// <summary>
    /// An already configured chart point, this class notifies the chart to update every time a property changes
    /// </summary>
    public class OhlcPoint : INotifyPropertyChanged
    {
        private double _open;
        private double _high;
        private double _low;
        private double _close;

        /// <summary>
        /// Initializes a new instance of OhclPoint class
        /// </summary>
        public OhlcPoint()
        {
            
        }

        /// <summary>
        /// Initializes a new instance o OhclPointc class, giving open, high, low and close values
        /// </summary>
        /// <param name="open"></param>
        /// <param name="high"></param>
        /// <param name="low"></param>
        /// <param name="close"></param>
        public OhlcPoint(double open, double high, double low, double close)
        {
            Open = open;
            High = high;
            Low = low;
            Close = close;
        }

        /// <summary>
        /// The open value i the chart
        /// </summary>
        public double Open
        {
            get { return _open; }
            set
            {
                _open = value;
                OnPropertyChanged("Open");
            }
        }

        /// <summary>
        /// The high value in the chart
        /// </summary>
        public double High
        {
            get { return _high; }
            set
            {
                _high = value;
                OnPropertyChanged("High");
            }
        }

        /// <summary>
        /// The low value in the chart
        /// </summary>
        public double Low
        {
            get { return _low; }
            set
            {
                _low = value;
                OnPropertyChanged("Low");
            }
        }

        /// <summary>
        /// The close value in the chart
        /// </summary>
        public double Close
        {
            get { return _close; }
            set
            {
                _close = value;
                OnPropertyChanged("Close");
            }
        }

        #region INotifyPropertyChangedImplementation

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Called when [property changed].
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            if (PropertyChanged != null) PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
