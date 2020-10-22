using System;
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

namespace LiveCharts.Wpf
{
    /// <summary>
    /// ChartPointView for Bulk rendering
    /// this dosen' have UIElement
    /// </summary>
    internal class AccelHorizontalBezierPointView : HorizontalBezierPointView
    {
        public string Label { get; set; }


        public override void DrawOrMove(ChartPoint previousDrawn, ChartPoint current, int index, ChartCore chart)
        {
            base.DrawOrMove(previousDrawn, current, index, chart);
        }

        public override void RemoveFromView(ChartCore chart)
        {
            base.RemoveFromView(chart);
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
            ActiveSplitters = 0;

            if (SplittersCollector == int.MaxValue - 1)
            {
                //just in case!
                Splitters.ForEach(s => s.SplitterCollectorIndex = 0);
                SplittersCollector = 0;
            }

            SplittersCollector++;

            if (IsPathInitialized)
            {
                //Model.Chart.View.EnsureElementBelongsToCurrentDrawMargin(Path);
                //Path.Stroke = Stroke;
                //Path.StrokeThickness = StrokeThickness;
                //Path.Fill = Fill;
                //Path.Visibility = Visibility;
                //Path.StrokeDashArray = StrokeDashArray;
                //Panel.SetZIndex(Path, Panel.GetZIndex(this));
                return;
            }

            IsPathInitialized = true;

            Path = new Path
            {
                //Stroke = Stroke,
                //StrokeThickness = StrokeThickness,
                //Fill = Fill,
                //Visibility = Visibility,
                //StrokeDashArray = StrokeDashArray
            };

            Panel.SetZIndex(Path, Panel.GetZIndex(this));

            var geometry = new PathGeometry();
            Figure = new PathFigure();
            geometry.Figures.Add(Figure);
            Path.Data = geometry;

            //Model.Chart.View.EnsureElementBelongsToCurrentDrawMargin(Path);
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
                    Segment = new BezierSegment(),
                    Container = Figure,
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


        #endregion


        #region Bulk rendering element and method

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


        private ChartPoint _HitTest(CorePoint pt)
        {
            ChartPoint hitChartPoint = null;

            var chartPointList = this.ActualValues.GetPoints(this,
                new CoreRectangle(0, 0, Model.Chart.DrawMargin.Width, Model.Chart.DrawMargin.Height));

            double hittestMargin = GetPointDiameter() + StrokeThickness;

            foreach (var current in chartPointList)
            {
                if( current.ChartLocation.HitTest(pt, hittestMargin) )
                {
                    hitChartPoint = current;
                    break;
                }
            }
            return hitChartPoint;
        }


        private void _OnHover(ChartPoint point)
        {
            HoverringChartPoint = point;
        }


        private void _OnHoverLeave()
        {
            HoverringChartPoint = null;
        }


        private void _Render(DrawingContext drawingContext)
        {
            if( Visibility == Visibility.Visible)
            {
                Brush brushStroke = Stroke.Clone();
                brushStroke.Freeze();

                Brush brushFill = Fill.Clone();
                brushFill.Freeze();

                Brush brushPointForeground = PointForeground.Clone();
                brushPointForeground.Freeze();

                Pen penStroke = new Pen(Stroke, StrokeThickness + (HoverringChartPoint!=null? 1d : 0));
                penStroke.DashStyle = new DashStyle(StrokeDashArray, 0);
                penStroke.Freeze();

                var chartPointList = this.ActualValues.GetPoints(this,
                    new CoreRectangle(0, 0, Model.Chart.DrawMargin.Width, Model.Chart.DrawMargin.Height));


                // Draw path line

                drawingContext.DrawGeometry(brushFill, penStroke, Path.Data);



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
                    foreach (var current in chartPointList)
                    {
                        drawingContext.PushTransform(new TranslateTransform(current.ChartLocation.X , current.ChartLocation.Y));
                        drawingContext.PushTransform(transformScale);
                        drawingContext.DrawGeometry(
                            Object.ReferenceEquals(current, m_HoverringChartPoint) ? brushStroke : brushPointForeground
                            , pgeoPenStroke, PointGeometry);
                        drawingContext.Pop();
                        drawingContext.Pop();
                    }
                    drawingContext.Pop();
                }


                //Draw label

                if (DataLabels)
                {
                    Typeface typeFace = new Typeface(FontFamily, FontStyle, FontWeight, FontStretch);

                    Brush brushFg = Foreground.Clone();
                    brushFg.Freeze();

                    foreach (var current in chartPointList)
                    {
                        var pointView = current.View as AccelHorizontalBezierPointView;

                        FormattedText formattedText = new FormattedText(
                                pointView.Label,
                                System.Globalization.CultureInfo.CurrentCulture,
                                FlowDirection.LeftToRight,
                                typeFace,
                                FontSize,
                                brushFg);

                        var xl = CorrectXLabel(current.ChartLocation.X - formattedText.Width * .5, Model.Chart, formattedText.Width);
                        var yl = CorrectYLabel(current.ChartLocation.Y - formattedText.Height * .5, Model.Chart, formattedText.Height);

                        drawingContext.DrawText(formattedText,
                            new Point(xl, yl));
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
            desiredPosition -= (PointGeometry == null ? 0 : GetPointDiameter()) + textHeight * .5 + 2;

            if (desiredPosition + textHeight > chart.DrawMargin.Height)
                desiredPosition -= desiredPosition + textHeight - chart.DrawMargin.Height + 2;

            if (desiredPosition < 0) desiredPosition = 0;

            return desiredPosition;
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

            public ChartPoint HitTest(CorePoint pt)
            {
                return _owner._HitTest(pt);
            }

            public void OnHover(ChartPoint point)
            {
                _owner._OnHover(point);
            }

            public void OnHoverLeave()
            {
                _owner._OnHoverLeave();
            }
        }

        #endregion

    }
}
