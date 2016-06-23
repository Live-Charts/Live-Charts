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
using System.Collections;
using System.Collections.Generic;
using LiveCharts.Charts;

namespace LiveCharts
{
    #region Enumerators

    public enum TooltipSelectionMode
    {
        /// <summary>
        /// Gets only the hovered point 
        /// </summary>
        OnlySender,
        /// <summary>
        /// Gets all the points that shares the value X in the chart
        /// </summary>
        SharedXValues,
        /// <summary>
        /// Gets all the points that shares the value Y in the chart
        /// </summary>
        SharedYValues,
        /// <summary>
        /// Gets all the points that shares the value X in the hovered series
        /// </summary>
        SharedXInSeries,
        /// <summary>
        /// Gets all the points that shares the value Y in the hovered series
        /// </summary>
        SharedYInSeries
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

    #region Structures And Classes

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

    public struct CoreColor
    {
        public CoreColor(byte a, byte r, byte g, byte b) : this()
        {
            A = a;
            R = r;
            G = g;
            B = b;
        }

        public byte A { get; set; }
        public byte R { get; set; }
        public byte G { get; set; }
        public byte B { get; set; }
    }

    public struct CoreGradientStop
    {
        public double Offset { get; set; }
        public CoreColor Color { get; set; }
    }

    public class LabelEvaluation
    {
        public LabelEvaluation(double angle, double w, double h, AxisCore axis, AxisTags source)
        {
            const double padding = 4;

            ActualWidth = w;
            ActualHeight = h;

            // for now there is no support for rotated and merged labels.
            // the labels will be rotated but there is no warranty that they are displayed correctly
            if (axis.View.IsMerged)
            {
                Top = 0;
                Bottom = 0;
                Left = 0;
                Right = 0;

                if (source == AxisTags.Y)
                {
                    XOffset = padding;
                    YOffset = padding;
                }
                else
                {
                    if (axis.Position == AxisPosition.LeftBottom)
                    {
                        //Bot
                        XOffset = padding;
                        YOffset = -h*2 - padding;
                    }
                    else
                    {
                        //Top
                        XOffset = padding;
                        YOffset = padding + h*2;
                    }
                }

                return;
            }

            //OK now lets evaluate the rotation angle...

            // the rotation angle starts from an horizontal line, yes like this text
            // - 0°, | 90°, - 180°, | 270°
            // notice normally rotating a label from 90 to 270° will show the label
            // in a wrong orientation
            // we need to fix that angle

            const double toRadians = Math.PI/180;

            // 1. width components
            // 2. height components

            WFromW = Math.Abs(Math.Cos(angle*toRadians)*w); // W generated from the width of the label
            WFromH = Math.Abs(Math.Sin(angle*toRadians)*h); // W generated from the height of the label

            HFromW = Math.Abs(Math.Sin(angle*toRadians)*w); // H generated from the width of the label
            HFromH = Math.Abs(Math.Cos(angle*toRadians)*h); // H generated from the height of the label

            LabelAngle = angle%360;
            if (LabelAngle < 0) LabelAngle += 360;
            if (LabelAngle > 90 && LabelAngle < 270)
                LabelAngle = (LabelAngle + 180)%360;

            //at this points angles should only exist in 1st and 4th quadrant
            //those are the only quadrants that generates readable labels
            //the other 2 quadrants display inverted labels

            var quadrant = ((int) (LabelAngle/90))%4 + 1;

            if (source == AxisTags.Y)
            {
                // Y Axis
                if (quadrant == 1)
                {
                    if (axis.Position == AxisPosition.LeftBottom)
                    {
                        // 1, L
                        Top = HFromW + (HFromH/2);      //space taken from separator to top
                        Bottom = TakenHeight - Top;          //space taken from separator to bottom
                        XOffset = -WFromW - padding;    //distance from separator to label origin in X
                        YOffset = -Top;                 //distance from separator to label origin in Y
                    }
                    else
                    {
                        // 1, R
                        Bottom = HFromW + (HFromH/2);
                        Top = TakenHeight - Bottom;
                        XOffset = padding + WFromH;
                        YOffset = -Top;
                    }
                }
                else
                {
                    if (axis.Position == AxisPosition.LeftBottom)
                    {
                        // 4, L
                        Bottom = HFromW + (HFromH/2);
                        Top = TakenHeight - Bottom;
                        XOffset = -TakenWidth - padding;
                        YOffset = HFromW - (HFromH/2);
                    }
                    else
                    {
                        // 4, R
                        Top = HFromW + (HFromH/2);
                        Bottom = TakenHeight - Top;
                        XOffset = padding;
                        YOffset = -Bottom;
                    }
                }
            }
            else
            {
                // X Axis

                //axis x has one exception, if labels rotation equals 0° then the label is centered
                if (Math.Abs(axis.View.LabelsRotation) < .01)
                {
                    Left = TakenWidth / 2;
                    Right = Left;
                    XOffset = -Left;
                    YOffset = axis.Position == AxisPosition.LeftBottom
                        ? padding
                        : -padding - TakenHeight;
                }
                else
                {
                    if (quadrant == 1)
                    {
                        if (axis.Position == AxisPosition.LeftBottom)
                        {
                            //1, B
                            Right = WFromW + (WFromH / 2);  //space taken from separator to right
                            Left = TakenWidth - Right;           //space taken from separator to left
                            XOffset = Left;                 //distance from separator to label origin in X
                            YOffset = padding;              //distance from separator to label origin in Y
                        }
                        else
                        {
                            //1, T
                            Left = WFromW + (WFromH/2);
                            Right = TakenWidth - Left;
                            XOffset = -WFromW;
                            YOffset = -padding - TakenHeight;
                        }
                    }
                    else
                    {
                        if (axis.Position == AxisPosition.LeftBottom)
                        {
                            //4, B
                            Left = WFromW + (WFromH/2);
                            Right = TakenWidth - Left;
                            XOffset = -Left;
                            YOffset = padding + HFromW;
                        }
                        else
                        {
                            //4, T
                            Right = WFromW + (WFromH/2);
                            Left = TakenWidth - Right;
                            XOffset = -Left;
                            YOffset = -HFromH;
                        }
                    }
                }
            }

        }

        public double LabelAngle { get; set; }
        public double WFromW { get; set; }
        public double WFromH { get; set; }
        public double HFromW { get; set; }
        public double HFromH { get; set; }
        public double Top { get; set; }
        public double Bottom { get; set; }
        public double Left { get; set; }
        public double Right { get; set; }
        public double XOffset { get; set; }
        public double YOffset { get; set; }

        public double TakenWidth { get { return WFromW + WFromH; } }
        public double TakenHeight { get { return HFromW + HFromH; } }

        public double ActualWidth { get; private set; }
        public double ActualHeight { get; private set; }

        public double GetOffsetBySource(AxisTags source)
        {
            return source == AxisTags.X
                ? XOffset
                : YOffset;
        }
    }

    public class CoreMargin
    {
        public double Top { get; set; }
        public double Bottom { get; set; }
        public double Left { get; set; }
        public double Right { get; set; }
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
        public double Range { get { return Max - Min; } }
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
            get { return _width; }
            set
            {
                _width = value < 0 ? 0 : value;
                if (SetWidth != null) SetWidth.Invoke(value);
            }
        }

        public double Height
        {
            get { return _height; }
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

    public interface ICartesianVisualElement
    {
        IChartView Chart { get; set; }
        bool RequiresAdd { get; set; }
        double X { get; set; }
        double Y { get; set; }
        int AxisX { get; set; }
        int AxisY { get; set; }
        void AddOrMove();
        void Remove();
    }

    #region ChartViews

    public interface IPieChart : IChartView
    {
        double InnerRadius { get; set; }
        double StartingRotationAngle { get; set; }
        double HoverPushOut { get; set; }
    }

    public interface ICartesianChart : IChartView
    {
        VisualElementsCollection VisualElements { get; set; }
    }

    #endregion

    #region Series Views

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

    public interface IBubbleSeriesView : ISeriesView
    {
        double MaxBubbleDiameter { get; set; }
        double MinBubbleDiameter { get; set; }
    }

    public interface IHeatSeries : ISeriesView
    {
        IList<CoreGradientStop> Stops { get; }
        bool DrawsHeatRange { get; }
    }

    public interface IColumnSeriesView : ISeriesView
    {
        double MaxColumnWidth { get; set; }
        double ColumnPadding { get; set; }
    }

    public interface IPieSeriesView : ISeriesView
    {
        double PushOut { get; set; }
    }

    public interface IOhlcSeriesView : ISeriesView
    {
        double MaxColumnWidth { get; set; }
    }

    public interface IStackedColumnSeriesView : ISeriesView, IStackModelableSeriesView
    {
        double MaxColumnWidth { get; set; }
        double ColumnPadding { get; set; }
    }

    public interface IStackedRowSeriesView : ISeriesView, IStackModelableSeriesView
    {
        double MaxRowHeight { get; set; }
        double RowPadding { get; set; }
    }

    public interface IRowSeriesView : ISeriesView
    {
        double MaxRowHeigth { get; set; }
        double RowPadding { get; set; }
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

    public interface IHeatPointView : IChartPointView
    {
        CoreColor ColorComponents { get; set; }
        double Width { get; set; }
        double  Height { get; set; }
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

        bool IsMocked { get; set; }
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
        void EnsureElementBelongsToCurrentView(object element);
        void EnsureElementBelongsToCurrentDrawMargin(object element);

        void HideTooltop();
        void ShowLegend(CorePoint at);
        void HideLegend();

        CoreSize LoadLegend();
        List<AxisCore> MapXAxes(ChartCore chart);
        List<AxisCore> MapYAxes(ChartCore chart);

#if DEBUG
        int GetCanvasElements();
        int GetDrawMarginElements();
        object GetCanvas();
        void MockIt(CoreSize size);
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
        Func<ChartPoint, string> LabelPoint { get; set; }
        IChartValues ActualValues { get; }

        IChartPointView GetPointView(IChartPointView view, ChartPoint point ,string label);
        void OnSeriesUpdateStart();
        void Erase();
        void OnSeriesUpdatedFinish();
        void InitializeColors();
        void DrawSpecializedElements();
        void PlaceSpecializedElements();
    }

    public interface IAxisView
    {
        AxisCore Model { get; set; }
        bool DisableAnimations { get; set; }
        double UnitWidth { get; set; }
        bool ShowLabels { get; set; }
        AxisTags Source { get; set; }
        double? MaxValue { get; set; }
        double? MinValue { get; set; }
        double LabelsRotation { get; set; }
        bool IsMerged { get; set; }

        CoreSize UpdateTitle(ChartCore chart, double rotationAngle = 0);
        void SetTitleTop(double value);
        void SetTitleLeft(double value);
        double GetTitleLeft();
        double GetTileTop();
        CoreSize GetLabelSize();
        AxisCore AsCoreElement(ChartCore chart, AxisTags source);
        void RenderSeparator(SeparatorElementCore model, ChartCore chart);
        void Clean();
    }

    public interface IAxisSectionView
    {
        AxisSectionCore Model { get; set; }
        double FromValue { get; set; }
        double ToValue { get; set; }

        void DrawOrMove(AxisTags source, int axis);
        void Remove();
        AxisSectionCore AsCoreElement(AxisCore axis, AxisTags source);
    }

    public interface ISeparatorView
    {
        bool IsEnabled { get; set; }
        /// <summary>
        /// Gets or sets separator step, this means the value between each line, use null for auto.
        /// </summary>
        double? Step { get; set; }

        SeparatorConfigurationCore AsCoreElement(AxisCore axis);
    }

    public interface ISeparatorElementView
    {
        SeparatorElementCore Model { get; }
        LabelEvaluation LabelModel { get; }

        LabelEvaluation UpdateLabel(string text, AxisCore axis, AxisTags source);

        void Clear(IChartView chart);

        //No animated methods
        void Place(ChartCore chart, AxisCore axis, AxisTags direction, int axisIndex, double toLabel, double toLine, double tab);
        void Remove(ChartCore chart);

        //Animated methods
        void Move(ChartCore chart, AxisCore axis, AxisTags direction, int axisIndex, double toLabel, double toLine, double tab);
        void FadeIn(AxisCore axis, ChartCore chart);
        void FadeOutAndRemove(ChartCore chart);
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