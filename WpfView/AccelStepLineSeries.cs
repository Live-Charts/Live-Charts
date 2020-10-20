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


namespace LiveCharts.Wpf
{
    using LiveCharts.Charts;
    using System.Linq;
    using System.Runtime.CompilerServices;


    /// <summary>
    /// ChartPoint側にはLine, Shapeなどの描画オブジェクトを持たせない
    /// </summary>
    internal class AccelStepLinePointView : PointView, IStepPointView
    {
        public double DeltaX { get; set; }
        public double DeltaY { get; set; }

        public string Label { get; set; }



        public override void DrawOrMove(ChartPoint previousDrawn, ChartPoint current, int index, ChartCore chart)
        {
        }

        public override void RemoveFromView(ChartCore chart)
        {
        }
    }




    /// <summary>
    /// The Step line series that suppots accel view.
    /// 
    /// 
    /// </summary>
    public class AccelStepLineSeries : StepLineSeries
    {
        /// <summary>
        /// Get the view of a chart point
        /// </summary>
        /// <param name="point"></param>
        /// <param name="label"></param>
        /// <returns></returns>
        public override IChartPointView GetPointView(ChartPoint point, string label)
        {
            var pbv = (AccelStepLinePointView)point.View;

            if (pbv == null)
            {
                pbv = new AccelStepLinePointView
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
        /// Get the view of a series drawing
        /// </summary>
        /// <returns></returns>
        public override ISeriesAccelView GetSeriesAccelView()
        {
            if (m_SeriesAccelView == null)
            {
                m_SeriesAccelView = new _AccelViewElement(_Render, _HitTest, _OnHover, _OnHoverLeave);

                Model.Chart.View.AddToDrawMargin(m_SeriesAccelView);

                var wpfChart = Model.Chart.View as Chart;
                wpfChart.AttachHoverableEventTo(m_SeriesAccelView);
                //m_SeriesDrawView.MouseDown += _SeriesDrawView_MouseDown;
                //m_SeriesDrawView.MouseEnter += _SeriesDrawView_MouseEnter;
                //m_SeriesDrawView.MouseMove += _SeriesDrawView_MouseMove;
                //m_SeriesDrawView.MouseLeave += _SeriesDrawView_MouseLeave;
            }
            return m_SeriesAccelView;
        }
        private _AccelViewElement m_SeriesAccelView;



        private ChartPoint HoverringChartPoint
        {
            get { return m_HoverringChartPoint; }
            set
            {
                
                if(m_HoverringChartPoint == null && value != null)
                {
                    StrokeThickness = StrokeThickness + 1;
                }
                else if(m_HoverringChartPoint != null && value == null)
                {
                    StrokeThickness = StrokeThickness - 1;
                }
                

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
            foreach (var current in this.ActualValues.GetPoints(this))
            {
                if( current.HitTest(pt, GetPointDiameter() + StrokeThickness))
                {
                    hitChartPoint = current;
                    break;
                }
            }
            return hitChartPoint;
        }


        private void _OnHover(ChartPoint point)
        {
            System.Diagnostics.Debug.Print("_OnHover");
            HoverringChartPoint = point;
        }


        private void _OnHoverLeave()
        {
            System.Diagnostics.Debug.Print("_OnHoverLeave");
            HoverringChartPoint = null;
        }


        private void _Render(DrawingContext drawingContext)
        {
            if( Visibility == Visibility.Visible)
            {
                Brush brushStroke = Stroke.Clone();
                brushStroke.Freeze();

                Brush brushPointForeground = PointForeground.Clone();
                brushPointForeground.Freeze();

                var w = Model.Chart.DrawMargin.Width;
                var chartPointList = this.ActualValues.GetPoints(this)
                    .Where(o=> 
                    {
                        return (o.ChartLocation.X > 0) && (o.ChartLocation.X < w); 
                    }).ToArray();


                // Draw step line

                Pen penAlt = new Pen(AlternativeStroke, StrokeThickness);
                penAlt.Freeze();
                Pen penMain = new Pen(Stroke, StrokeThickness);
                penMain.Freeze();
                Pen pen3 = new Pen(PointForeground, StrokeThickness);

                ChartPoint previous = null;
                foreach (var current in chartPointList)
                {
                    if (previous != null)
                    {
                        var currentView = current.View as AccelStepLinePointView;

                        if (InvertedMode)
                        {
                            drawingContext.DrawLine(penAlt
                                , new Point(current.ChartLocation.X, current.ChartLocation.Y)
                                , new Point(current.ChartLocation.X - currentView.DeltaX, current.ChartLocation.Y));

                            drawingContext.DrawLine(penMain
                                , new Point(current.ChartLocation.X - currentView.DeltaX, current.ChartLocation.Y)
                                , new Point(current.ChartLocation.X - currentView.DeltaX, current.ChartLocation.Y - currentView.DeltaY));
                        }
                        else
                        {
                            drawingContext.DrawLine(penAlt
                                , new Point(current.ChartLocation.X, current.ChartLocation.Y)
                                , new Point(current.ChartLocation.X, current.ChartLocation.Y - currentView.DeltaY));

                            drawingContext.DrawLine(penMain
                                , new Point(current.ChartLocation.X - currentView.DeltaX, current.ChartLocation.Y - currentView.DeltaY)
                                , new Point(current.ChartLocation.X, current.ChartLocation.Y - currentView.DeltaY));

                        }
                    }

                    previous = current;
                }


                // Draw point geometry

                foreach (var current in chartPointList)
                {
                    if (PointGeometry != null && Math.Abs(PointGeometrySize) > 0.1)
                    {
                        //var pointView = current.View as AccelStepLinePointView;

                        drawingContext.DrawEllipse(
                            Object.ReferenceEquals(current, m_HoverringChartPoint) ? brushStroke : brushPointForeground
                            , penMain
                            , new Point(current.ChartLocation.X, current.ChartLocation.Y)
                            , GetPointDiameter(), GetPointDiameter());
                    }
                }


                //Draw label

                if (DataLabels)
                {
                    Typeface typeFace = new Typeface(FontFamily, FontStyle, FontWeight, FontStretch);

                    Brush brushFg = Foreground.Clone();
                    brushFg.Freeze();

                    foreach (var current in chartPointList)
                    {
                        var pointView = current.View as AccelStepLinePointView;

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
            public _AccelViewElement(
                Action<DrawingContext> _func_OnRender,
                Func<CorePoint, ChartPoint> _func_HitTest,
                Action<ChartPoint> _func_OnHover,
                Action _func_OnHoverLeave
                )
            {
                m_Func_OnRender = _func_OnRender;
                m_Func_HitTest = _func_HitTest;
                m_Func_OnHover = _func_OnHover;
                m_Func_OnHoverLeave = _func_OnHoverLeave;
            }
            private Action<DrawingContext> m_Func_OnRender;
            private Func<CorePoint, ChartPoint> m_Func_HitTest;
            private Action<ChartPoint> m_Func_OnHover;
            private Action m_Func_OnHoverLeave;


            public void DrawOrMove()
            {
                this.InvalidateVisual();
                /*
                Task.Run(async () =>
                {
                    //await Task.Delay(1);
                    await Dispatcher.BeginInvoke((Action)this.InvalidateVisual);
                });
                */
            }

            protected override void OnRender(DrawingContext drawingContext)
            {
                System.Diagnostics.Debug.Print($"Called OnRender {DateTime.Now}");

                base.OnRender(drawingContext);

                m_Func_OnRender( drawingContext );
            }

            public ChartPoint HitTest(CorePoint pt)
            {
                return m_Func_HitTest(pt);
            }

            public void OnHover(ChartPoint point)
            {
                m_Func_OnHover(point);
            }

            public void OnHoverLeave()
            {
                m_Func_OnHoverLeave();
            }
        }

    }
}
