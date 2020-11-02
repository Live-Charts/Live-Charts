using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using LiveCharts.Definitions.Points;
using LiveCharts.Definitions.Series;
using LiveCharts.SeriesAlgorithms;
using LiveCharts.Wpf.Charts.Base;
using LiveCharts.Wpf.Components;
using LiveCharts.Wpf.Points;
using LiveCharts.Dtos;
using LiveCharts.Charts;
using System.Linq;

namespace LiveCharts.Wpf
{
    /// <summary>
    /// ChartPointView for Bulk rendering
    /// this dosen' have UIElement
    /// </summary>
    internal class AccelHorizontalBezierPointView : AccelPointView, IBezierPointView
    {
        public BezierData Data { get; set; }

        public string Label { get; set; }



        #region shrink drawing

        /// <summary>
        /// if this view is shrinking or not
        /// </summary>
        public ViewShrinkState ShrinkState { get; private set; }

        /// <summary>
        /// 縮約表示の初期化（縮約表示しない状態にしておく）
        /// </summary>
        public void InitShrinkState()
        {
            this.ShrinkState = ViewShrinkState.Individual;
            this.ShrinkView = null;
        }

        /// <summary>
        /// 別のPointViewを自PointViewの縮約に取り込む
        /// </summary>
        public void AddToShrinkView(ChartPoint shrinkedPoint, AccelHorizontalBezierPointView shrinkedPointView, SeriesOrientation orientation)
        {
            shrinkedPointView.ShrinkState = ViewShrinkState.Shrinked;

            if (this.ShrinkView == null)
            {
                this.ShrinkState = ViewShrinkState.Shrinker;

                this.ShrinkView = new _ShrinkView();

                //縮約表示用の端点を初期化
                this.ShrinkView.PointH = this.Data.Point1;
                this.ShrinkView.PointL = this.Data.Point1;
                this.ShrinkView.PointC = this.Data.Point3;
            }

            //TODO:縦軸表示の場合への対応は後日
            //if(orientation== SeriesOrientation.Vertical)
            //{
            //}

            //より高い値をPointHに保持
            if (this.ShrinkView.PointH.Y > shrinkedPointView.Data.Point1.Y)
            {
                this.ShrinkView.PointH = shrinkedPointView.Data.Point1;
                this.ShrinkView.IsHighFirst = true;
            }

            //より低い値をPointLに保持
            if (this.ShrinkView.PointL.Y < shrinkedPointView.Data.Point1.Y)
            {
                this.ShrinkView.PointL = shrinkedPointView.Data.Point1;
                this.ShrinkView.IsHighFirst = false;
            }

            this.ShrinkView.PointC = shrinkedPointView.Data.Point3;
        }

        private _ShrinkView ShrinkView { set; get; }

        private class _ShrinkView
        {
            public CorePoint PointH { get; set; }
            public CorePoint PointL { get; set; }
            public CorePoint PointC { get; set; }

            public bool IsHighFirst { get; set; }
        }

        #endregion

        public void DrawBezierTo(
            ChartPoint current
            , StreamGeometryContext ctx
            )
        {
            if (this.ShrinkState == ViewShrinkState.Individual)
            {
                ctx.BezierTo(
                    this.Data.Point1.AsPoint(),
                    this.Data.Point2.AsPoint(),
                    this.Data.Point3.AsPoint(),
                    true, false);
            }
            else if (this.ShrinkState == ViewShrinkState.Shrinker)
            {
                //縮約での表示
                if (this.ShrinkView.IsHighFirst)
                {
                    ctx.LineTo(this.ShrinkView.PointH.AsPoint(), true, false);
                    ctx.LineTo(this.ShrinkView.PointL.AsPoint(), true, false);
                }
                else
                {
                    ctx.LineTo(this.ShrinkView.PointL.AsPoint(), true, false);
                    ctx.LineTo(this.ShrinkView.PointH.AsPoint(), true, false);
                }
                ctx.LineTo(this.ShrinkView.PointC.AsPoint(), true, false);
            }

        }

    }


    /// <summary>
    /// The line series that suppots Bulk rendering.
    /// 
    /// 
    /// </summary>
    public class AccelLineSeries : LineSeries
    {
        #region Overridden Methods

        /// <summary>
        /// This method runs when the update starts
        /// 
        /// ベースクラスの処理からEnsureElementBelongsToCurrentDrawMarginを除いただけ
        /// ベースクラスに変更がある場合は、要対応
        /// </summary>
        public override void OnSeriesUpdateStart()
        {
            //nothing to do
        }


        /// <summary>
        /// Gets the view of a given point
        /// </summary>
        /// <param name="point"></param>
        /// <param name="label"></param>
        /// <returns></returns>
        public override IChartPointView GetPointView(ChartPoint point, string label)
        {
            var pbv = (AccelHorizontalBezierPointView)point.View;

            if (pbv == null)
            {
                pbv = new AccelHorizontalBezierPointView
                {
                    IsNew = true,
                };
            }
            else
            {
                pbv.IsNew = false;
            }

            pbv.Label = label;

            return pbv;
        }


        /// <summary>
        /// This method runs when the update finishes
        /// </summary>
        public override void OnSeriesUpdatedFinish()
        {
            //本当はコールしたくないが、ベースクラスのベースクラス、の処理が欲しい故
            base.OnSeriesUpdatedFinish();

            if (m_SeriesAccelView == null)
            {
                m_SeriesAccelView = new _AccelViewElement(this);

                Model.Chart.View.AddToDrawMargin(m_SeriesAccelView);

                var wpfChart = Model.Chart.View as Chart;
                wpfChart.AttachHoverableEventTo(m_SeriesAccelView);

                Panel.SetZIndex(m_SeriesAccelView, Panel.GetZIndex(this));
            }
            m_SeriesAccelView.InvalidateVisual();
        }
        private _AccelViewElement m_SeriesAccelView;

        /// <summary>
        /// Erases series
        /// </summary>
        /// <param name="removeFromView"></param>
        public override void Erase(bool removeFromView = true)
        {
            if (m_SeriesAccelView != null)
            {
                Model?.Chart?.View?.RemoveFromDrawMargin(m_SeriesAccelView);
                m_SeriesAccelView = null;
            }

            base.Erase(removeFromView);
        }

        /// <summary> segments would be considerd when rendering </summary>
        public override void StartSegment(int atIndex, CorePoint location)
        {
            //nothing to do
        }

        /// <summary> segments would be considerd when rendering </summary>
        public override void EndSegment(int atIndex, CorePoint location)
        {
            //nothing to do
        }



        #endregion


        #region Bulk rendering element and method

        /// <summary>
        /// chart point that mouse is over on
        /// </summary>
        private ChartPoint HoverringChartPoint
        {
            get { return m_HoverringChartPoint; }
            set
            {
                if ( m_HoverringChartPoint != value )
                {
                    m_HoverringChartPoint = value;
                    m_SeriesAccelView?.InvalidateVisual();
                }
            }
        }
        private ChartPoint m_HoverringChartPoint;


        /// <summary>
        /// prepate rendered chart points for quick access
        /// </summary>
        private IList<ChartPoint> RenderdChartPointList { get; set; }


        private void _Render(DrawingContext drawingContext)
        {
            if( Visibility == Visibility.Visible)
            {
                var seriesOrientation = Model?.SeriesOrientation ?? SeriesOrientation.Horizontal;


                Brush brushStroke = Stroke.Clone();
                brushStroke.Freeze();

                Brush brushFill = Fill.Clone();
                brushFill.Freeze();

                Brush brushPointForeground = PointForeground.Clone();
                brushPointForeground.Freeze();

                Pen penStroke = new Pen(Stroke, StrokeThickness + (HoverringChartPoint!=null? 1d : 0));
                penStroke.DashStyle = new DashStyle(StrokeDashArray, 0);
                penStroke.Freeze();


                //prepat chart point list 
                this.RenderdChartPointList = this.ActualValues.GetPoints(this,
                    new CoreRectangle(0, 0, Model.Chart.DrawMargin.Width, Model.Chart.DrawMargin.Height)).ToList();


                // shrink draw items
                AccelHorizontalBezierPointView shrinkerView = null;
                foreach (var current in this.RenderdChartPointList)
                {
                    var currentView = current.View as AccelHorizontalBezierPointView;
                    if (currentView != null)
                    {
                        currentView.InitShrinkState();

                        if (shrinkerView == null)
                        {
                            shrinkerView = currentView;
                        }
                        else
                        {
                            //compare with shrinkerView
                            if (Math.Abs(shrinkerView.Data.Point1.X - currentView.Data.Point3.X) < 1.5d)
                            //if (false)
                            {
                                shrinkerView.AddToShrinkView(current, currentView, seriesOrientation);
                            }
                            else
                            {
                                shrinkerView = null;
                            }
                        }
                    }

                }
                


                // Draw path line

                foreach (var segmentedList in SegmentedRenderdChartPointList(this.RenderdChartPointList))
                {

                    StreamGeometry geometry = new StreamGeometry();
                    using (StreamGeometryContext ctx = geometry.Open())
                    {
                        AccelHorizontalBezierPointView lastPointView = null;
                        CorePoint firstPoint = new CorePoint();
                        bool isFirst = true;
                        foreach (var current in segmentedList)
                        {
                            var currentView = current.View as AccelHorizontalBezierPointView;
                            if (currentView != null)
                            {
                                if (isFirst)
                                {
                                    isFirst = false;

                                    firstPoint = currentView.Data.Point1;
                                    if(seriesOrientation== SeriesOrientation.Horizontal)
                                    {
                                        firstPoint.Y = Model.Chart.DrawMargin.Top + Model.Chart.DrawMargin.Height;
                                    }
                                    else
                                    {
                                        firstPoint.X = Model.Chart.DrawMargin.Left;
                                    }

                                    //start point of segment
                                    ctx.BeginFigure(firstPoint.AsPoint(), true, true);
                                    ctx.LineTo(currentView.Data.Point1.AsPoint(), false, true);
                                }

                                currentView.DrawBezierTo(current, ctx);

                                lastPointView = currentView;
                            }
                        }

                        //close segment
                        if (lastPointView != null)
                        {
                            var lastPoint = lastPointView.Data.Point3;
                            if (seriesOrientation == SeriesOrientation.Horizontal)
                            {
                                lastPoint.Y = Model.Chart.DrawMargin.Top + Model.Chart.DrawMargin.Height;
                            }
                            else
                            {
                                lastPoint.X = Model.Chart.DrawMargin.Left;
                            }

                            ctx.LineTo(lastPoint.AsPoint(), false, true);
                            ctx.LineTo(firstPoint.AsPoint(), false, true);
                        }
                    }

                    // draw one segment
                    geometry.Freeze();
                    drawingContext.DrawGeometry(brushFill, penStroke, geometry);

                }


                // Draw point geometry

                if (PointGeometry != null && Math.Abs(PointGeometrySize) > 0.1)
                {
                    var rect = PointGeometry.Bounds;
                    var offsetX = rect.X + rect.Width / 2d;
                    var offsetY = rect.Y + rect.Height / 2d;

                    var pgeoRate = Math.Max(0.1d, Math.Abs(PointGeometrySize) - StrokeThickness) / Math.Max(1d,  Math.Max( rect.Width, rect.Height));

                    //prepate pen for scaled
                    Pen pgeoPenStroke = penStroke.Clone();
                    pgeoPenStroke.Thickness /= pgeoRate;
                    pgeoPenStroke.Freeze();

                    //prepare transforms
                    Transform transformOffset = new TranslateTransform(-offsetX, -offsetY);
                    transformOffset.Freeze();
                    Transform transformScale = new ScaleTransform(-pgeoRate, pgeoRate, offsetX, offsetY);
                    transformScale.Freeze();

                    drawingContext.PushTransform(transformOffset);
                    foreach (var current in this.RenderdChartPointList)
                    {
                        var currentView = current.View as AccelHorizontalBezierPointView;
                        if (currentView != null)
                        {
                            if (currentView.ShrinkState == ViewShrinkState.Individual)
                            {
                                drawingContext.PushTransform(new TranslateTransform(current.ChartLocation.X, current.ChartLocation.Y));
                                drawingContext.PushTransform(transformScale);
                                drawingContext.DrawGeometry(
                                    Object.ReferenceEquals(current, m_HoverringChartPoint) ? brushStroke : brushPointForeground
                                    , pgeoPenStroke, PointGeometry);
                                drawingContext.Pop();
                                drawingContext.Pop();
                            }
                        }
                    }
                    drawingContext.Pop();
                }


                //Draw label

                if (DataLabels)
                {
                    Typeface typeFace = new Typeface(FontFamily, FontStyle, FontWeight, FontStretch);

                    Brush brushFg = Foreground.Clone();
                    brushFg.Freeze();

                    CoreRectangle preRect = new CoreRectangle(0, 0, 0, 0);
                    foreach (var current in this.RenderdChartPointList)
                    {
                        var currentView = current.View as AccelHorizontalBezierPointView;
                        if (currentView != null)
                        {
                            //if text position is in the previous text rectange, skip draw it.
                            if (preRect.HitTest(current.ChartLocation, 0))
                            {
                                continue;
                            }
                            if (currentView.ShrinkState == ViewShrinkState.Shrinked)
                            {
                                continue;
                            }


                            FormattedText formattedText = new FormattedText(
                                    currentView.Label,
                                    System.Globalization.CultureInfo.CurrentCulture,
                                    FlowDirection.LeftToRight,
                                    typeFace,
                                    FontSize,
                                    brushFg);

                            var xl = CorrectXLabel(current.ChartLocation.X - formattedText.Width * .5, Model.Chart, formattedText.Width);
                            var yl = CorrectYLabel(current.ChartLocation.Y - formattedText.Height * .5, Model.Chart, formattedText.Height);

                            preRect = new CoreRectangle(xl, yl, formattedText.Width, formattedText.Height);

                            drawingContext.DrawText(formattedText,
                                new Point(xl, yl));
                        }
                    }

                }

            }

        }

        /// <summary>
        /// Splits a collection of points every double.Nan
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private IEnumerable<IEnumerable<ChartPoint>> SegmentedRenderdChartPointList(IEnumerable<ChartPoint> list)
        {
            var segList = new List<ChartPoint>();

            foreach(var p in list)
            {
                if (double.IsNaN(p.X) || double.IsNaN(p.Y))
                {
                    if (segList.Count > 0)
                    {
                        yield return segList;
                    }
                    segList.Clear();
                }
                else
                {
                    segList.Add(p);
                }
            }

            if (segList.Count > 0)
            {
                yield return segList;
            }
        }



        private double CorrectXLabel(double desiredPosition, ChartCore chart, Double textWidth)
        {
            if (desiredPosition + textWidth * .5 < -0.1) return -textWidth;

            if (desiredPosition + textWidth > chart.DrawMargin.Width)
                desiredPosition -= desiredPosition + textWidth - chart.DrawMargin.Width + 2;

            if (desiredPosition < 0) desiredPosition = 0;

            return desiredPosition;
        }

        private double CorrectYLabel(double desiredPosition, ChartCore chart, Double textHeight)
        {
            desiredPosition -= (PointGeometry == null ? 0 : GetPointDiameter()) + textHeight * .5 + 2;

            if (desiredPosition + textHeight > chart.DrawMargin.Height)
                desiredPosition -= desiredPosition + textHeight - chart.DrawMargin.Height + 2;

            if (desiredPosition < 0) desiredPosition = 0;

            return desiredPosition;
        }


        private ChartPoint _HitTest(CorePoint pt)
        {
            ChartPoint hitChartPoint = null;

            double hittestMargin = GetPointDiameter() + StrokeThickness + 2d;

            double currentDistance = Double.MaxValue;
            foreach (var current in this.RenderdChartPointList)
            {
                if (current.ChartLocation.HitTest(pt, hittestMargin))
                {
                    var d = (current.ChartLocation.X - pt.X) * (current.ChartLocation.X - pt.X)
                            + (current.ChartLocation.Y - pt.Y) * (current.ChartLocation.Y - pt.Y);
                    if (d < currentDistance)
                    {
                        currentDistance = d;
                        hitChartPoint = current;
                    }
                }
            }
            return hitChartPoint;
        }

        private class _AccelViewElement : FrameworkElement, ISeriesAccelView
        {
            public _AccelViewElement(AccelLineSeries owner)
            {
                _owner = owner;
            }
            private AccelLineSeries _owner;

            protected override void OnRender(DrawingContext drawingContext)
            {
                base.OnRender(drawingContext);
                _owner._Render(drawingContext);
            }
            
            protected override HitTestResult HitTestCore(PointHitTestParameters hitTestParameters)
            {
                Point pt = hitTestParameters.HitPoint;

                var cp = _owner._HitTest(new CorePoint(pt.X, pt.Y));
                if (cp != null)
                {
                    return new PointHitTestResult(this, pt);
                }
                return null;
            }

            public ChartPoint HitTestChartPoint(CorePoint pt)
            {
                return _owner._HitTest(pt);
            }

            public void OnHover(ChartPoint point)
            {
                _owner.HoverringChartPoint = point;
            }

            public void OnHoverLeave()
            {
                _owner.HoverringChartPoint = null;
            }
        }

        #endregion

    }
}
