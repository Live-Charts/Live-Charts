﻿#region License
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
using System.Linq;
using LiveCharts.Core.Charts;
using LiveCharts.Core.Coordinates;
using LiveCharts.Core.Drawing;

#endregion

namespace LiveCharts.Core.DataSeries
{
    /// <summary>
    /// An <see cref="ISeries"/> that has <see cref="Stroke"/> and <see cref="Fill"/> properties.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <typeparam name="TCoordinate">The type of the coordinate.</typeparam>
    /// <typeparam name="TViewModel">The type of the view model.</typeparam>
    /// <typeparam name="TSeries">The type of the series.</typeparam>
    /// <seealso cref="Series{TModel, TCoordinate, TViewModel, TSeries}" />
    /// <seealso cref="IStrokeSeries" />
    public abstract class StrokeSeries<TModel, TCoordinate, TViewModel, TSeries> 
        : Series<TModel, TCoordinate, TViewModel, TSeries>, IStrokeSeries
        where TCoordinate : ICoordinate
        where TSeries : class, ISeries
    {
        private Brush _stroke;
        private double _strokeThickness;
        private Brush _fill;
        private IEnumerable<double> _strokeDashArray;

        /// <inheritdoc />
        protected StrokeSeries()
        {
            Charting.BuildFromTheme<IStrokeSeries>(this);
        }

        /// <inheritdoc />
        public Brush Stroke
        {
            get => _stroke;
            set
            {
                _stroke = value;
                OnPropertyChanged();
            }
        }

        /// <inheritdoc />
        public double StrokeThickness
        {
            get => _strokeThickness;
            set
            {
                _strokeThickness = value;
                OnPropertyChanged();
            }
        }

        /// <inheritdoc />
        public IEnumerable<double> StrokeDashArray
        {
            get => _strokeDashArray;
            set
            {
                _strokeDashArray = value;
                OnPropertyChanged();
            }
        }

        /// <inheritdoc />
        public Brush Fill
        {
            get => _fill;
            set
            {
                _fill = value;
                OnPropertyChanged();
            }
        }

        /// <inheritdoc />
        public override SeriesStyle Style
        {
            get
            {
                return new SeriesStyle
                {
                    Fill = Fill,
                    Stroke = Stroke,
                    StrokeThickness = StrokeThickness,
                    StrokeDashArray = StrokeDashArray?.Select(x => (float) x)
                };
            }
        }

        /// <inheritdoc />
        protected override void SetDefaultColors(ChartModel chart)
        {
            if (!(Stroke == null || Fill == null)) return;

            var nextColor = chart.GetNextColor();

            if (Stroke == null)
            {
                Stroke = new SolidColorBrush(nextColor);
            }

            if (Fill == null)
            {
                Fill = new SolidColorBrush(nextColor.SetOpacity(DefaultFillOpacity));
            }
        }
    }
}