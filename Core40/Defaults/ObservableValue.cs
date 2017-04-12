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
namespace LiveCharts.Defaults
{
    /// <summary>
    /// An already configured chart point, this class notifies the chart to update every time the value property changes
    /// </summary>
    public class ObservableValue : IObservableChartPoint
    {
        private double _value;

        /// <summary>
        /// Initializes a new instance of ObservableValue class
        /// </summary>
        public ObservableValue()
        {
            
        }

        /// <summary>
        /// Initializes a new instance of ObservableValue class with a given value
        /// </summary>
        /// <param name="value"></param>
        public ObservableValue(double value)
        {
            Value = value;
        }

        /// <summary>
        /// Point changed event
        /// </summary>
        public event Action PointChanged;

        /// <summary>
        /// Value in he chart
        /// </summary>
        public double Value
        {
            get { return _value; }
            set
            {
                _value = value;
                OnPointChanged();
            }
        }

        /// <summary>
        /// On point property changed event
        /// </summary>
        protected virtual  void OnPointChanged()
        {
            if (PointChanged != null) PointChanged.Invoke();
        }
    }
}