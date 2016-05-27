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
    #region Enumerators

    public enum TooltipSelectionMode
    {
        OnlySender, SharedXValues, SharedYValues
    }

    public enum SeriesOrientation
    {
        All, Horizontal, Vertical
    }

    public enum AxisPosition
    {
        LeftBottom, RightTop
    }

    public enum SeparationState
    {
        Remove,
        Keep,
        InitialAdd
    }

    public enum ZoomingOptions
    {
        None,
        X,
        Y,
        Xy
    }

    public enum LegendLocation
    {
        None,
        Top,
        Bottom,
        Left,
        Right
    }

    public enum StackMode
    {
        Values, Percentage
    }

    #endregion

    #region Structs And Classes

    public struct CorePoint
    {
        public CorePoint(double x, double y) : this()
        {
            X = x;
            Y = y;
        }

        public CorePoint(CorePoint point): this()
        {
            X = point.X;
            Y = point.Y;
        }

        public double X { get; set; }
        public double Y { get; set; }

        public static CorePoint operator +(CorePoint p1, CorePoint p2)
        {
            return new CorePoint(p1.X + p2.X, p1.Y + p2.Y);
        }

        public static CorePoint operator -(CorePoint p1, CorePoint p2)
        {
            return new CorePoint(p1.X - p2.X, p1.Y - p2.Y);
        }
    }

    public struct CoreSize
    {
        public CoreSize(double width, double heigth) : this()
        {
            Width = width;
            Height = heigth;
        }

        public double Width { get; set; }
        public double Height { get; set; }
    }

    public struct CoreLimit
    {
        public CoreLimit(double min, double max) : this()
        {
            Max = max;
            Min = min;
        }

        public double Max { get; set; }
        public double Min { get; set; }
    }

    public struct Xyw
    {
        public Xyw(double x, double y, double w) : this()
        {
            X = x;
            Y = y;
            W = w;
        }

        public double X { get; set; }
        public double Y { get; set; }
        public double W { get; set; }
    }

    internal struct StackedSum
    {
        public StackedSum(double value): this()
        {
            if (value < 0)
            {
                Left = value;
            }
            else
            {
                Right = value;
            }
        }

        public double Left { get; set; }
        public double Right { get; set; }
    }

    public class CoreRectangle
    {
        private double _left;
        private double _top;
        private double _width;
        private double _height;

        public CoreRectangle()
        {
            
        }

        public CoreRectangle(double left, double top, double width, double height) : this()
        {
            Left = left;
            Top = top;
            Width = width;
            Height = height;
        }

        public event Action<double> SetTop;
        public event Action<double> SetLeft;
        public event Action<double> SetWidth;
        public event Action<double> SetHeight;

        public double Left
        {
            get { return _left; }
            set
            {
                _left = value;
                if (SetLeft != null) SetLeft.Invoke(value);
            }
        }

        public double Top
        {
            get { return _top; }
            set
            {
                _top = value;
                if (SetTop != null) SetTop.Invoke(value);
            }
        }

        public double Width
        {
            get
            {
                return _width;
            }
            set
            {
                _width = value < 0 ? 0 : value;
                if (SetWidth != null) SetWidth.Invoke(value);
            }
        }

        public double Height
        {
            get
            {
                return _height;
            }
            set
            {
                _height = value < 0 ? 0 : value;
                if (SetHeight != null) SetHeight.Invoke(value);
            }
        }
    }

    public class BezierData
    {
        public BezierData()
        {
        }

        public BezierData(CorePoint point)
        {
            Point1 = point;
            Point2 = point;
            Point3 = point;
        }

        public CorePoint Point1 { get; set; }

        public CorePoint Point2 { get; set; }
        public CorePoint Point3 { get; set; }
        public CorePoint StartPoint { get; set; }
    }
    #endregion

    #region Interfaces

    public interface IChartUpdater
    {
        ChartCore Chart { get; set; }
        void Run(bool restartView = false, bool updateNow = false);
        void UpdateFrequency(TimeSpan freq);
    }

    public interface IObservableChartPoint
    {
        event Action PointChanged;
    }

    public interface IPieSeries
    {
        ISeriesView View { get; }
    }

    public interface ICartesianSeries
    {
        ISeriesView View { get; }

        double GetMinX(AxisCore axis);
        double GetMaxX(AxisCore axis);
        double GetMinY(AxisCore axis);
        double GetMaxY(AxisCore axis);
    }

    public interface IStackModelableSeriesView
    {
        StackMode StackMode { get; set; }
    }

    #region ChartViews

    public interface IPieChart
    {
        double InnerRadius { get; set; }
        double StartingRotationAngle { get; set; }
    }

    #endregion

    #region Series Views

    public interface IBubbleSeriesView : ISeriesView
    {
        double MaxBubbleDiameter { get; set; }
        double MinBubbleDiameter { get; set; }
    }

    public interface IColumnSerieView : ISeriesView
    {
        double MaxColumnWidth { get; set; }
    }

    public interface IPieSeriesView : ISeriesView
    {
        double PushOut { get; set; }
    }

    public interface IOhlcSeriesView : ISeriesView
    {
        double MaxColumnWidth { get; set; }
    }

    public interface IStackedColumnSeriesViewView : ISeriesView, IStackModelableSeriesView
    {
        double MaxColumnWidth { get; set; }
    }

    public interface IStackedRowSeriesViewView : ISeriesView, IStackModelableSeriesView
    {
        double MaxRowHeigth { get; set; }
    }

    public interface IRowSeriesView : ISeriesView
    {
        double MaxRowHeigth { get; set; }
    }

    public interface ILineSeriesView : ISeriesView
    {
        double LineSmoothness { get; set; }
        void StartSegment(int atIndex, CorePoint location);
        void EndSegment(int atIndex, CorePoint location);
    }

    public interface IStackedAreaSeriesViewView : ILineSeriesView, IStackModelableSeriesView
    {

    }

    public interface IVerticalStackedAreaSeriesViewView : ILineSeriesView, IStackModelableSeriesView
    {

    }

    #endregion

    #region Point Views
    public interface IBezierPointView : IChartPointView
    {
        BezierData Data { get; set; }
    }

    public interface IRectanglePointView : IChartPointView
    {
        CoreRectangle Data { get; set; }
        double ZeroReference { get; set; }
    }

    public interface IBubblePointView : IChartPointView
    {
        double Diameter { get; set; }
    }

    public interface IOhlcPointView : IChartPointView
    {
        double Open { get; set; }
        double High { get; set; }
        double Close { get; set; }
        double Low { get; set; }
        double Width { get; set; }
        double Left { get; set; }
        double StartReference { get; set; }
    }

    public interface IPieSlicePointView : IChartPointView
    {
        double Rotation { get; set; }
        double Wedge { get; set; }
        double InnerRadius { get; set; }
        double Radius { get; set; }
    }
    #endregion


    #endregion

    #region Views
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

        void SetDrawMarginTop(double value);
        void SetDrawMarginLeft(double value);
        void SetDrawMarginHeight(double value);
        void SetDrawMarginWidth(double value);

        void AddToView(object element);
        void AddToDrawMargin(object element);
        void RemoveFromView(object element);
        void RemoveFromDrawMargin(object element);

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
        bool IsInVisualTree { get; }
        Func<ChartPoint, string> LabelPoint { get; set; }

        IChartPointView GetPointView(IChartPointView view, ChartPoint point ,string label);
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
        List<IAxisSectionView> Sections { get; set; }
        AxisTags Source { get; set; }
        double? MaxValue { get; set; }
        double? MinValue { get; set; }
        double LabelsRotation { get; }


        CoreSize UpdateTitle(ChartCore chart, double rotationAngle = 0);
        void SetTitleTop(double value);
        void SetTitleLeft(double value);
        double GetTitleLeft();
        double GetTileTop();
        CoreSize GetLabelSize();
        AxisCore AsCoreElement(ChartCore chart, AxisTags source);
        void RenderSeparator(SeparatorElementCore model, ChartCore chart);
    }

    public interface IAxisSectionView
    {
        AxisSectionCore Model { get; set; }
        double FromValue { get; set; }
        double ToValue { get; set; }
        void DrawOrMove(AxisTags source, int axis);

        AxisSectionCore AsCoreElement(AxisCore axis, AxisTags source);
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
        CoreSize UpdateLabel(string text, AxisCore axis);
        void UpdateLine(AxisTags source, ChartCore chart, int axisIndex, AxisCore axisCore);
    }

    public interface IChartPointView
    {
        bool IsNew { get; set; }
        void DrawOrMove(ChartPoint previousDrawn, ChartPoint current, int index, ChartCore chart);
        void RemoveFromView(ChartCore chart);
        void OnHover(ChartPoint point);
        void OnHoverLeave(ChartPoint point);
    }
    #endregion
}