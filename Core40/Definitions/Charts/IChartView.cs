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
using System.Collections.Generic;
using LiveCharts.Charts;
using LiveCharts.Definitions.Series;
using LiveCharts.Dtos;
using LiveCharts.Events;

namespace LiveCharts.Definitions.Charts
{
    public interface IChartView
    {
        ChartCore Model { get; }

        event DataClickHandler DataClick;

        bool IsMocked { get; set; }
        SeriesCollection Series { get; set; }
        IEnumerable<ISeriesView> ActualSeries { get; }
        TimeSpan TooltipTimeout { get; set; }
        ZoomingOptions Zoom { get; set; }
        double ZoomingSpeed { get; set; }
        LegendLocation LegendLocation { get; set; }
        bool DisableAnimations { get; set; }
        TimeSpan AnimationsSpeed { get; set; }
        UpdaterState UpdaterState { get; set; }

        bool HasTooltip { get; }
        bool HasDataClickEventAttached { get; }
        bool Hoverable { get; }
        bool IsControlLoaded { get; }
        bool IsInDesignMode { get; }

        void SetDrawMarginTop(double value);
        void SetDrawMarginLeft(double value);
        void SetDrawMarginHeight(double value);
        void SetDrawMarginWidth(double value);

        void AddToView(object element);
        void AddToDrawMargin(object element);
        void RemoveFromView(object element);
        void RemoveFromDrawMargin(object element);
        void EnsureElementBelongsToCurrentView(object element);
        void EnsureElementBelongsToCurrentDrawMargin(object element);

        void HideTooltip();
        void ShowLegend(CorePoint at);
        void HideLegend();

        CoreSize LoadLegend();
        List<AxisCore> MapXAxes(ChartCore chart);
        List<AxisCore> MapYAxes(ChartCore chart);
    }
}