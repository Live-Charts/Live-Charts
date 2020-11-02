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
using LiveCharts.Wpf.Points;
using LiveCharts.Dtos;
using LiveCharts.Charts;
using System.Threading.Tasks;

namespace LiveCharts.Wpf
{
    /// <summary>
    /// ChartPointView for Bulk rendering
    /// this dosen' have UIElement
    /// </summary>
    internal class AccelCandlePointView : AccelPointView, IOhlcPointView
    {
        public double Open { get; set; }
        public double High { get; set; }
        public double Close { get; set; }
        public double Low { get; set; }
        public double Width { get; set; }
        public double Left { get; set; }
        public double StartReference { get; set; }


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
        public void AddToShrinkView(ChartPoint shrinkedPoint, AccelCandlePointView shrinkedPointView)
        {
            shrinkedPointView.ShrinkState = ViewShrinkState.Shrinked;

            if(this.ShrinkView == null)
            {
                this.ShrinkState = ViewShrinkState.Shrinker;

                this.ShrinkView = new _ShrinkView();

                //縮約表示用の矩形を初期化
                this.ShrinkView.ShrinkerRectangle = new CoreRectangle(
                        this.Left, this.High,
                        this.Width, this.Low - this.High);
            }

            //縮約表示用の矩形に、縮約されるViewのViewをマージする
            this.ShrinkView.ShrinkerRectangle.Merge(new CorePoint(shrinkedPointView.Left, shrinkedPointView.High));
            this.ShrinkView.ShrinkerRectangle.Merge(new CorePoint(shrinkedPointView.Left + shrinkedPointView.Width, shrinkedPointView.Low));

            this.ShrinkView.LastShrinked = shrinkedPoint;
        }

        private _ShrinkView ShrinkView{ set; get;}

        private class _ShrinkView
        {
            /// <summary>
            /// this rectangle is representative.
            /// </summary>
            public CoreRectangle ShrinkerRectangle { set; get; }

            /// <summary>
            /// end point
            /// </summary>
            public ChartPoint LastShrinked { get; set; }
        }

        #endregion


        public void DrawCandle(
            ChartPoint current
            , DrawingContext drawingContext
            , Pen penIncrease , Pen penDecrease
            , Brush brushIncrease , Brush brushDecrease
            , ChartPoint previous
            , IList<FinancialColoringRule> coloringRules
            )
        {
            if (this.ShrinkState == ViewShrinkState.Individual)
            {
                //draw as nomal candle

                var center = this.Left + this.Width / 2;

                var penLine = current.Open <= current.Close ? penIncrease : penDecrease;
                var brushRect = current.Open <= current.Close ? brushIncrease : brushDecrease;
                var penRect = current.Open <= current.Close ? penIncrease : penDecrease;

                if (coloringRules != null)
                {
                    foreach (var rule in coloringRules)
                    {
                        if (!rule.Condition(current, previous)) continue;

                        penLine = penLine.Clone();
                        penLine.Brush = rule.Stroke;
                        penLine.Freeze();

                        brushRect = rule.Fill.Clone();
                        brushRect.Freeze();

                        penRect = penRect.Clone();
                        penRect.Brush = rule.Stroke;
                        penRect.Freeze();

                        break;
                    }
                }

                drawingContext.DrawLine(
                    penLine
                    , new Point(center, this.High)
                    , new Point(center, this.Low));


                drawingContext.DrawRectangle(brushRect, penRect,
                    new Rect(
                        this.Left
                        , Math.Min(this.Open, this.Close)
                        , this.Width
                        , Math.Abs(this.Open - this.Close)
                    ));


            }
            else if (this.ShrinkState == ViewShrinkState.Shrinker)
            {
                //draw as shrinker
                if (this.ShrinkView != null)
                {
                    var brushRect = current.Open <= this.ShrinkView.LastShrinked.Close ? brushIncrease : brushDecrease;
                    drawingContext.DrawRectangle(brushRect, null, this.ShrinkView.ShrinkerRectangle.AsRect());
                }
            }
        }
    }


    /// <summary>
    /// The Candle series that suppots Bulk rendering.
    /// </summary>
    public class AccelCandleSeries : CandleSeries
    {
        #region Overridden Methods

        /// <summary>
        /// Gets the view of a given point
        /// </summary>
        /// <param name="point"></param>
        /// <param name="label"></param>
        /// <returns></returns>
        public override IChartPointView GetPointView(ChartPoint point, string label)
        {
            var pbv = (AccelCandlePointView)point.View;

            if (pbv == null)
            {
                pbv = new AccelCandlePointView
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

        #endregion


        #region Bulk rendering element and method

        private ChartPoint HoverringChartPoint
        {
            get { return m_HoverringChartPoint; }
            set
            {
                if (m_HoverringChartPoint != value)
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
        private IEnumerable<ChartPoint> RenderdChartPointList { get; set; }


        private void _Render(DrawingContext drawingContext)
        {
            if (Visibility == Visibility.Visible)
            {
                Brush brushStroke = Stroke.Clone();
                brushStroke.Freeze();

                Brush brushFill = Fill.Clone();
                brushFill.Freeze();

                

                Brush brushIncrease = IncreaseBrush.Clone();
                brushIncrease.Freeze();

                Brush brushDecrease = DecreaseBrush.Clone();
                brushDecrease.Freeze();


                Pen penIncrease = new Pen(IncreaseBrush, StrokeThickness + (HoverringChartPoint != null ? 1d : 0));
                penIncrease.DashStyle = new DashStyle(StrokeDashArray, 0);
                penIncrease.Freeze();

                Pen penDecrease = new Pen(DecreaseBrush, StrokeThickness + (HoverringChartPoint != null ? 1d : 0));
                penDecrease.DashStyle = new DashStyle(StrokeDashArray, 0);
                penDecrease.Freeze();


                //prepat chart point list 
                this.RenderdChartPointList = this.ActualValues.GetPoints(this,
                    new CoreRectangle(0, 0, Model.Chart.DrawMargin.Width, Model.Chart.DrawMargin.Height));


                // shrink draw items 

                AccelCandlePointView shrinkerView = null;
                foreach (var current in this.RenderdChartPointList)
                {
                    var currentView = current.View as AccelCandlePointView;
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
                            if (Math.Abs(shrinkerView.Left - currentView.Left) < 1.5d)
                            {
                                shrinkerView.AddToShrinkView(current, currentView);
                            }
                            else
                            {
                                shrinkerView = null;
                            }
                        }
                    }
                }



                // Draw candle

                ChartPoint previous = null;
                foreach (var current in this.RenderdChartPointList)
                {
                    var currentView = current.View as AccelCandlePointView;

                    if (currentView != null)
                    {
                        currentView.DrawCandle(
                            current,
                            drawingContext,
                            penIncrease, penDecrease, brushIncrease, brushDecrease,
                            previous, this.ColoringRules);

                        previous = current;
                    }
                }



                //Draw label

                if (DataLabels)
                {
                    Typeface typeFace = new Typeface(FontFamily, FontStyle, FontWeight, FontStretch);

                    Brush brushFg = Foreground.Clone();
                    brushFg.Freeze();

                    CoreRectangle preRect = new CoreRectangle(0,0,0,0);
                    foreach (var current in this.RenderdChartPointList)
                    {
                        var pointView = current.View as AccelCandlePointView;
                        if (pointView != null)
                        {
                            //if text position is in the previous text rectange, skip draw it.
                            if( preRect.HitTest(current.ChartLocation, 0) )
                            {
                                continue;
                            }
                            if (pointView.ShrinkState == ViewShrinkState.Shrinked)
                            {
                                continue;
                            }


                            FormattedText formattedText = new FormattedText(
                                    pointView.Label,
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
            if (desiredPosition + textHeight > chart.DrawMargin.Height)
                desiredPosition -= desiredPosition + textHeight - chart.DrawMargin.Height + 2;

            if (desiredPosition < 0) desiredPosition = 0;

            return desiredPosition;
        }


        private ChartPoint _HitTest(CorePoint pt)
        {
            ChartPoint hitChartPoint = null;

            double hittestMargin = StrokeThickness + 2d;

            double currentDistance = Double.MaxValue;
            foreach (var current in this.RenderdChartPointList)
            {
                var currentView = current.View as AccelCandlePointView;
                if (currentView != null)
                {
                    CoreRectangle rectBox = new CoreRectangle(
                                        currentView.Left, Math.Min(currentView.High, currentView.Low),
                                        currentView.Width, Math.Abs(currentView.Low - currentView.High));
                    if (rectBox.HitTest(pt, hittestMargin))
                    {
                        var center = currentView.Left + currentView.Width / 2;
                        var d = Math.Abs(center - pt.X);
                        if (d < currentDistance)
                        {
                            currentDistance = d;
                            hitChartPoint = current;
                        }
                    }
                }

            }
            return hitChartPoint;
        }


        private class _AccelViewElement : FrameworkElement, ISeriesAccelView
        {
            public _AccelViewElement(AccelCandleSeries owner)
            {
                _owner = owner;
            }
            private AccelCandleSeries _owner;

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
