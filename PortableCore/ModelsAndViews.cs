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

namespace LiveChartsCore
{
    public interface ISeriesView
    {
        SeriesCore Model { get; set; }
        IChartValues Values { get; set; }
        int ScalesXAt { get; set; }
        int ScalesYAt { get; set; }

        IChartPointView RenderPoint(IChartPointView view);
        void RemovePointView(object view);
        void InitializeView();
        void Erase();
        void CloseView();
    }

    public interface IAxisView
    {
        ISeparatorCacheView NewSeparator();
        AxisCore Model { get; set; }
        ISeparatorView Separator { get; set; }
        double LabelsReference { get; set; }
        double UnitWidth { get; set; }
        LvcSize UpdateTitle(ChartCore chart, double rotationAngle = 0);
        void SetTitleTop(double value);
        void SetTitleLeft(double value);
        double GetTitleLeft();
        double GetTileTop();
        LvcSize GetLabelSize();
    }

    public interface ISeparatorView
    {
        ChartCore Chart { get; set; }
        /// <summary>
        /// Gets or sets if separators are enabled (will be drawn)
        /// </summary>
        bool IsEnabled { get; set; }
        /// <summary>
        /// Gets or sets separators thickness
        /// </summary>
        int StrokeThickness { get; set; }
        /// <summary>
        /// Gets or sets sepator step, this means the value between each line, use null for auto.
        /// </summary>
        double? Step { get; set; }
    }

    public interface ISeparatorCacheView
    {
        bool IsNew { get; set; }
        SeparationState State { get; set; }
        bool IsActive { get;set; }
        double Key { get; set; }
        double Value { get; set; }

        LvcSize UpdateLabel(string text);
        void UpdateLine(AxisTags source, ChartCore chart, int axisIndex, AxisCore axisCore);
    }

    public interface IChartView
    {
        ChartCore Model { get; }

        event Action<object, ChartPoint> DataClick;

        SeriesCollection Series { get; set; }
        TimeSpan TooltipTimeout { get; set; }
        ZoomingOptions Zoom { get; set; }
        LegendLocation LegendLocation { get; set; }
        bool DisableAnimations { get; set; }
        TimeSpan AnimationsSpeed { get; set; }

        bool HasTooltip { get; }
        bool HasDataClickEventAttached { get; }
        bool IsControlLoaded { get; }

        void InitializeSeries(ISeriesView series);
        void Update(bool restartAnimations = true);
        void SetDrawMarginTop(double value);
        void SetDrawMarginLeft(double value);
        void SetDrawMarginHeight(double value);
        void SetDrawMarginWidth(double value);
        void Erase();
        void AddToView(object element);
        void AddToDrawMargin(object element);
        void RemoveFromView(object element);
        void RemoveFromDrawMargin(object element);
        void ShowTooltip(ChartPoint sender, IEnumerable<ChartPoint> sibilings, LvcPoint at);
        void HideTooltop();
        void IntializeAxis();
        void ShowLegend(LvcPoint at);
        void HideLegend();

        LvcSize LoadLegend();
        TimeSpan? GetZoomingSpeed();

    }
}