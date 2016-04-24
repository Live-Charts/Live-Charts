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

    #region Series
    public interface ISeriesModel
    {
        ISeriesView View { get; set; }
        IChartModel Chart { get; set; }
        IChartValues Values { get; set; }
        int ScalesXAt { get; set; }
        int ScalesYAt { get; set; }
        SeriesConfiguration Configuration { get; set; }
        SeriesCollection SeriesCollection { get; set; }
        string Title { get; set; }
        void Update();
    }

    public interface ISeriesView
    {
        ISeriesModel Model { get; set; }
        IChartPointView InitializePointView();
        void InitializeView();
        void Erase();
    }
    #endregion

    #region Axis

    public interface IAxisModel
    {
        IChartModel Chart { get; set; }
        IAxisView View { get; }
        IList<string> Labels { get; set; }
        Func<double, string> LabelFormatter { get; set; }
        double StrokeThickness { get; set; }
        bool ShowLabels { get; set; }
        double? MaxValue { get; set; }
        double? MinValue { get; set; }
        string Title { get; set; }
        AxisPosition Position { get; set; }
        bool IsMerged { get; set; }
        double MaxLimit { get; set; }
        double MinLimit { get; set; }
        double S { get; set; }
        int CleanFactor { get; set; }
        Dictionary<double, ISeparatorCacheView> Cache { get; set; }
        void CalculateSeparator(IChartModel chart, AxisTags source);
        double FromPreviousAxisState(double value, AxisTags source, IChartModel chart);
        LvcSize PrepareChart(AxisTags source, IChartModel chart);
        void UpdateSeparators(AxisTags source, IChartModel chart, int axisPosition);
    }

    public interface IAxisView
    {
        IAxisModel Model { get; }
        ISeparatorCacheView NewSeparator();

        ISeparatorView Separator { get; set; }
        double LabelsReference { get; set; }
        double UnitWidth { get; set; }
        LvcSize UpdateTitle(double rotationAngle = 0);
        void SetTitleTop(double value);
        void SetTitleLeft(double value);
        double GetTitleLeft();
        double GetTileTop();
        LvcSize GetLabelSize();
    }

    public interface ISeparatorView
    {
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
        void UpdateLine(AxisTags source, IChartModel chart, int axisIndex, IAxisModel axis);
    }
    #endregion

    #region Chart

    public interface IChartModel
    {
        IChartView View { get; set; }
        IChartUpdater Updater { get; set; }
        LvcSize ChartControlSize { get; set; }
        LvcRectangle DrawMargin { get; set; }
        SeriesCollection Series { get; set; }
        bool HasUnitaryPoints { get; set; }
        bool Invert { get; set; }
        object AxisX { get; set; }
        object AxisY { get; set; }
        TimeSpan TooltipTimeout { get; set; }
        ZoomingOptions Zoom { get; set; }
        LegendLocation LegendLocation { get; set; }
        AxisTags PivotZoomingAxis { get; set; }
        bool DisableAnimatons { get; set; }
        TimeSpan AnimationsSpeed { get; set; }

        void PrepareAxes();
        void CalculateComponentsAndMargin();
        LvcRectangle PlaceLegend(LvcRectangle drawMargin);

        void Update(bool restartAnimations = true);
        void ZoomIn(LvcPoint pivot);
        void ZoomOut(LvcPoint pivot);
        void ClearZoom();
        void TooltipHideStartCount();
    }

    public interface IChartView
    {
        IChartModel Model { get; }
        bool IsHoverable { get; }
        bool IsControlLoaded { get; }

        void InitializeSeries(ISeriesView series);
        void Update(bool restartAnimations = true);
        void SetDrawMarginTop(double value);
        void SetDrawMarginLeft(double value);
        void SetDrawMarginHeight(double value);
        void SetDrawMarginWidth(double value);
        void Erase();
        void AddToView(object element);
        void RemoveFromView(object element);
        void ShowTooltip(ChartPoint sender, IEnumerable<ChartPoint> sibilings, LvcPoint at);
        void HideTooltop();
        void IntializeAxis();
        void ShowLegend(LvcPoint at);
        void HideLegend();


        LvcSize LoadLegend();
        TimeSpan? GetZoomingSpeed();

    }
    #endregion


}