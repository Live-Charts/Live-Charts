#region License
// The MIT License (MIT)
// 
// Copyright (c) 2016 Alberto Rodríguez Orozco & LiveCharts contributors
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy 
// of this software and associated documentation files (the "Software"), to deal 
// in the Software without restriction, including without limitation the rights to 
// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies 
// of the Software, and to permit persons to whom the Software is furnished to 
// do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all 
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, 
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES 
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND 
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT 
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, 
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING 
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR 
// OTHER DEALINGS IN THE SOFTWARE.
#endregion
namespace LiveCharts.Core.Defaults
{
    /// <summary>
    /// Defines an observable weighted object, this object notifies the chart to update when any property change.
    /// </summary>
    /// <seealso cref="ObservablePointModel" />
    public class WeightedModel : ObservablePointModel
    {
        private double _weight;

        /// <summary>
        /// Initializes a new instance of the <see cref="WeightedModel"/> class.
        /// </summary>
        public WeightedModel()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WeightedModel"/> class.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="weight">The weight.</param>
        public WeightedModel(float x, float y, float weight)
            : base(x, y)
        {
            Weight = weight;
        }

        /// <summary>
        /// Gets or sets the weight value.
        /// </summary>
        /// <value>
        /// The weight.
        /// </value>
        public double Weight
        {
            get => _weight;
            set
            {
                _weight = value;
                OnPropertyChanged();
            }
        }
    }
}