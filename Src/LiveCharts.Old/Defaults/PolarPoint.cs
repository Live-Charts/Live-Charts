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
    public class PolarPoint : INotifyPropertyChanged
    {
        private double _radius;
        private double _angle;

        /// <summary>
        /// Initializes a new instance of PolarPoint class
        /// </summary>
        public PolarPoint()
        {
            
        }

        /// <summary>
        /// Initializes a new instance of PolarPoint class, giving angle and radius
        /// </summary>
        /// <param name="radius"></param>
        /// <param name="angle"></param>
        public PolarPoint(double radius, double angle)
        {
            Radius = radius;
            Angle = angle;
        }

        /// <summary>
        /// The radius of the point
        /// </summary>
        public double Radius
        {
            get { return _radius; }
            set
            {
                _radius = value;
                OnPropertyChanged("Radius");
            }
        }

        /// <summary>
        /// The angle of the point
        /// </summary>
        public double Angle
        {
            get { return _angle; }
            set
            {
                _angle = value;
                OnPropertyChanged("Angle");
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
