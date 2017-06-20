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
using System.Runtime.CompilerServices;

namespace LiveCharts.Defaults
{
    /// <summary>
    /// An already configured chart point with a date time and a double properties, this class notifies the chart to update every time a property changes
    /// </summary>
    public class DateTimePoint : INotifyPropertyChanged
    {
        private DateTime _dateTime;
        private double _value;

        /// <summary>
        /// Initializes a new instance of DateTimePoint class
        /// </summary>
        public DateTimePoint()
        {
            
        }

        /// <summary>
        /// Initializes a new instance of DateTimePoint class, giving date time and value
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="value"></param>
        public DateTimePoint(DateTime dateTime, double value)
        {
            _dateTime = dateTime;
            _value = value;
        }

        /// <summary>
        /// DateTime Property
        /// </summary>
        public DateTime DateTime
        {
            get { return _dateTime; }
            set
            {
                _dateTime = value;
                OnPropertyChanged("DateTime");
            }
        }

        /// <summary>
        /// The Value property
        /// </summary>
        public double Value
        {
            get { return _value; }
            set
            {
                _value = value;
                OnPropertyChanged("Value");
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
