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

#region

using System.Collections.Generic;
using System.Windows;
using LiveCharts.Core.Charts;
using LiveCharts.Core.Dimensions;

#endregion

namespace LiveCharts.Wpf
{
    /// <summary>
    /// Generates pie charts.
    /// </summary>
    /// <seealso cref="LiveCharts.Wpf.Chart" />
    /// <seealso cref="IPieChartView" />
    public class PieChart : Chart, IPieChartView
    {
        private readonly IList<IList<Plane>> _planes;

        /// <summary>
        /// Initializes the <see cref="PieChart"/> class.
        /// </summary>
        static PieChart()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(PieChart),
                new FrameworkPropertyMetadata(typeof(PieChart)));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PieChart"/> class.
        /// </summary>
        public PieChart()
        {
            Model = new PieChartModel(this);
            _planes = new List<IList<Plane>>
            {
                new List<Plane> {new Axis()},
                new List<Plane> {new Axis()}
            };
        }

        #region Dependency properties

        /// <summary>
        /// The starting rotation angle property.
        /// </summary>
        public static readonly DependencyProperty StartingRotationAngleProperty = DependencyProperty.Register(
            nameof(StartingRotationAngle), typeof(double), typeof(PieChart), new PropertyMetadata(default(double)));

        /// <summary>
        /// The inner radius property.
        /// </summary>
        public static readonly DependencyProperty InnerRadiusProperty = DependencyProperty.Register(
            nameof(InnerRadius), typeof(double), typeof(PieChart), new PropertyMetadata(default(double)));

        #endregion

        #region Properties

        /// <inheritdoc />
        public double StartingRotationAngle
        {
            get => (double) GetValue(StartingRotationAngleProperty);
            set => SetValue(StartingRotationAngleProperty, value);
        }

        /// <inheritdoc />
        public double InnerRadius
        {
            get => (double) GetValue(InnerRadiusProperty);
            set => SetValue(InnerRadiusProperty, value);
        }

        #endregion

        protected override IList<IList<Plane>> GetOrderedDimensions()
        {
            return _planes;
        }
    }
}