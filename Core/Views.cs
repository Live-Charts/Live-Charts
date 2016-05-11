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

namespace LiveCharts
{
    public interface IChartView
    {
        ChartCore Model { get; }

        event Action<object, ChartPoint> DataClick;

        SeriesCollection Series { get; set; }
        TimeSpan TooltipTimeout { get; set; }
        TimeSpan? UpdaterFrequency { get; set; }
        ZoomingOptions Zoom { get; set; }
        LegendLocation LegendLocation { get; set; }
        bool DisableAnimations { get; set; }
        TimeSpan AnimationsSpeed { get; set; }

        bool HasTooltip { get; }
        bool HasDataClickEventAttached { get; }
        bool IsControlLoaded { get; }

        void SetDrawMarginTop(double value);
        void SetDrawMarginLeft(double value);
        void SetDrawMarginHeight(double value);
        void SetDrawMarginWidth(double value);
        void Erase();
        void AddToView(object element);
        void AddToDrawMargin(object element);
        void RemoveFromView(object element);
        void RemoveFromDrawMargin(object element);
        void ShowTooltip(ChartPoint sender, IEnumerable<ChartPoint> sibilings, CorePoint at);
        void HideTooltop();
        void ShowLegend(CorePoint at);
        void HideLegend();

        CoreSize LoadLegend();
        List<AxisCore> MapXAxes(ChartCore chart);
        List<AxisCore> MapYAxes(ChartCore chart);

#if DEBUG
        void CountElements();
#endif
    }

    public interface ISeriesView
    {
        SeriesAlgorithm Model { get; set; }
        IChartValues Values { get; set; }
        bool DataLabels { get; }
        int ScalesXAt { get; set; }
        int ScalesYAt { get; set; }
        object Configuration { get; set; }
        bool IsSeriesVisible { get; }

        IChartPointView GetPointView(IChartPointView view, string label);
        void OnSeriesUpdateStart();
        void Erase();
        void OnSeriesUpdatedFinish();
    }

    public interface IAxisView
    {
        AxisCore Model { get; set; }
        bool DisableAnimations { get; set; }
        double LabelsReference { get; set; }
        double UnitWidth { get; set; }
        bool ShowLabels { get; set; }
        CoreSize UpdateTitle(ChartCore chart, double rotationAngle = 0);
        void SetTitleTop(double value);
        void SetTitleLeft(double value);
        double GetTitleLeft();
        double GetTileTop();
        CoreSize GetLabelSize();
        AxisCore AsCoreElement(ChartCore chart);
        void RenderSeparator(SeparatorElementCore model, ChartCore chart);
    }

    public interface ISeparatorView
    {
        bool IsEnabled { get; set; }
        /// <summary>
        /// Gets or sets sepator step, this means the value between each line, use null for auto.
        /// </summary>
        double? Step { get; set; }

        SeparatorConfigurationCore AsCoreElement(AxisCore axis);
    }

    public interface ISeparatorElementView
    { 
        SeparatorElementCore Model { get; }
        CoreSize UpdateLabel(string text);
        void UpdateLine(AxisTags source, ChartCore chart, int axisIndex, AxisCore axisCore);
    }

    public interface IChartPointView
    {
        bool IsNew { get; set; }
        void DrawOrMove(ChartPoint previousDrawn, ChartPoint current, int index, ChartCore chart);
        void RemoveFromView(ChartCore chart);
    }
}