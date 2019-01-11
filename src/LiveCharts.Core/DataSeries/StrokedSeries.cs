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

using LiveCharts.Charts;
using LiveCharts.Coordinates;
using LiveCharts.Drawing.Brushes;
using LiveCharts.Drawing.Shapes;
using System.Collections.Generic;
#if NET45 || NET46
using Brush = LiveCharts.Drawing.Brushes.Brush;
#endif

#endregion

namespace LiveCharts.DataSeries
{
    /// <summary>
    /// An <see cref="ISeries"/> that has <see cref="Stroke"/> and <see cref="Fill"/> properties.
    /// </summary>
    /// <typeparam name="TModel">The type of the model to plot.</typeparam>
    /// <typeparam name="TCoordinate">The type of the coordinate required by the series.</typeparam>
    /// <typeparam name="TPointShape">The type of the point shape in the UI.</typeparam>
    /// <seealso cref="Series{TModel, TCoordinate, TPointShape}" />
    /// <seealso cref="IStrokeSeries" />
    public abstract class StrokeSeries<TModel, TCoordinate, TPointShape>
        : Series<TModel, TCoordinate, TPointShape>, IStrokeSeries
        where TCoordinate : ICoordinate
        where TPointShape : class, IShape
    {
        private Brush? _stroke;
        private float _strokeThickness;
        private Brush? _fill;
        private IEnumerable<double>? _strokeDashArray;

        /// <inheritdoc />
        protected StrokeSeries()
        {
            Global.Settings.BuildFromTheme<IStrokeSeries>(this);
        }

        /// <inheritdoc />
        public Brush? Stroke
        {
            get => _stroke;
            set
            {
                _stroke = value;
                OnPropertyChanged(nameof(Stroke));
            }
        }

        /// <inheritdoc />
        public float StrokeThickness
        {
            get => _strokeThickness;
            set
            {
                _strokeThickness = value;
                OnPropertyChanged(nameof(StrokeThickness));
            }
        }

        /// <inheritdoc />
        public IEnumerable<double>? StrokeDashArray
        {
            get => _strokeDashArray;
            set
            {
                _strokeDashArray = value;
                OnPropertyChanged(nameof(StrokeDashArray));
            }
        }

        /// <inheritdoc />
        public Brush? Fill
        {
            get => _fill;
            set
            {
                _fill = value;
                OnPropertyChanged(nameof(Fill));
            }
        }

        /// <inheritdoc />
        protected override void SetDefaultColors(ChartModel chart)
        {
            if (!(Stroke == null || Fill == null))
            {
                return;
            }

            var nextColor = chart.GetNextColor();

            if (Stroke == null)
            {
                Stroke = new SolidColorBrush(
                    nextColor.A, nextColor.R, nextColor.G, nextColor.B);
            }

            if (Fill == null)
            {
                Fill = new SolidColorBrush(
                    (byte)(DefaultFillOpacity * 255), nextColor.R, nextColor.G, nextColor.B);
            }
        }
    }
}