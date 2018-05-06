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

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using LiveCharts.Core.Animations;
using LiveCharts.Core.Charts;
using LiveCharts.Core.Coordinates;
using LiveCharts.Core.DataSeries;
using LiveCharts.Core.Drawing.Styles;
using LiveCharts.Core.Interaction.Events;
using LiveCharts.Core.Interaction.Points;
using FontFamily = System.Windows.Media.FontFamily;
using Frame = LiveCharts.Core.Animations.Frame;

#endregion

namespace LiveCharts.Wpf.Views
{
    public abstract class PointView<TModel, TCoordinate, TViewModel, TSeries, TShape>
        : IPointView<TModel, TCoordinate, TViewModel, TSeries>
        where TCoordinate : ICoordinate
        where TSeries : ISeries
        where TShape : FrameworkElement, new()
    {
        /// <summary>
        /// Gets or sets the shape.
        /// </summary>
        /// <value>
        /// The shape.
        /// </value>
        public TShape Shape { get; protected set; }

        /// <summary>
        /// Gets or sets the label.
        /// </summary>
        /// <value>
        /// The label.
        /// </value>
        public Label Label { get; protected set; }

        /// <inheritdoc cref="IPointView{TModel,TCoordinate,TViewModel,TSeries}.DrawShape"/>
        protected abstract void OnDraw(
            ChartPoint<TModel, TCoordinate, TViewModel, TSeries> chartPoint,
            ChartPoint<TModel, TCoordinate, TViewModel, TSeries> previous,
            TimeLine timeLine);

        /// <summary>
        /// Places the label.
        /// </summary>
        /// <param name="chartPoint">The point.</param>
        /// <param name="labelSize">The label size.</param>
        protected abstract void PlaceLabel(
            ChartPoint<TModel, TCoordinate, TViewModel, TSeries> chartPoint,
            SizeF labelSize);

        /// <inheritdoc cref="IPointView{TModel,TCoordinate,TViewModel,TSeries}.DrawLabel" />
        protected virtual void OnDrawLabel(
            ChartPoint<TModel, TCoordinate, TViewModel, TSeries> chartPoint,
            DataLabelsPosition position,
            LabelStyle style,
            TimeLine timeLine)
        {
            var chart = chartPoint.Chart;
            var isNew = Label == null;

            if (isNew)
            {
                Label = new Label();
                chart.Content.AddChild(Label, true);
            }

            Label.Content = chartPoint.Series.GetDataLabel(chartPoint.Coordinate);

            Label.Foreground = style.Foreground.AsWpf();
            Label.FontFamily = new FontFamily(style.Font.FamilyName);
            Label.FontSize = style.Font.Size;
            Label.FontStyle = style.Font.Style.AsWpf();
            Label.FontWeight = style.Font.Weight.AsWpf();

            var ft = new FormattedText(
                Label.Content.ToString(),
                CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                new Typeface(Label.FontFamily, Label.FontStyle, Label.FontWeight, Label.FontStretch),
                Label.FontSize,
                Label.Foreground);

            PlaceLabel(chartPoint, new SizeF((float) ft.Width, (float) ft.Height));
        }

        /// <inheritdoc cref="Dispose"/>
        protected abstract void OnDispose(IChartView chart);

        #region ResourceViewImplementation
        
        /// <inheritdoc />
        object IPointView<TModel, TCoordinate, TViewModel, TSeries>.VisualElement => Shape;

        /// <inheritdoc />
        object IPointView<TModel, TCoordinate, TViewModel, TSeries>.Label => Label;

        /// <inheritdoc />
        void IPointView<TModel, TCoordinate, TViewModel, TSeries>.DrawShape(
            ChartPoint<TModel, TCoordinate, TViewModel, TSeries> chartPoint,
            ChartPoint<TModel, TCoordinate, TViewModel, TSeries> previous, TimeLine timeLine)
        {
            OnDraw(chartPoint, previous, timeLine);
        }

        /// <inheritdoc />
        void IPointView<TModel, TCoordinate, TViewModel, TSeries>.DrawLabel(
            ChartPoint<TModel, TCoordinate, TViewModel, TSeries> chartPoint,
            DataLabelsPosition position,
            LabelStyle style, TimeLine timeLine)
        {
            OnDrawLabel(chartPoint, position, style, timeLine);
        }
        
        public event DisposingResourceHandler Disposed;

        object IResource.UpdateId { get; set; }

        /// <inheritdoc />
        public void Dispose(IChartView view)
        {
            OnDispose(view);
            Disposed?.Invoke(view, this);
        }

        #endregion
    }
}