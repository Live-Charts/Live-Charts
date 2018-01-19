using System.Windows.Media;
using System.Windows.Shapes;
using LiveCharts.Core.Abstractions;
using Point = LiveCharts.Core.Drawing.Point;

namespace LiveCharts.Wpf.Separators
{
    /// <summary>
    /// The separator class.
    /// </summary>
    /// <seealso cref="LiveCharts.Core.Abstractions.ISeparator" />
    public class SeparatorView : ISeparator
    {
        /// <summary>
        /// Gets or sets the line.
        /// </summary>
        /// <value>
        /// The line.
        /// </value>
        public Line Line { get; protected set; }

        /// <inheritdoc />
        public void Move(Point point1, Point point2, bool dispose, IChartView chart)
        {
            var isNew = Line == null;

            var speed = chart.AnimationsSpeed;

            if (isNew)
            {
                var wpfChart = (CartesianChart) chart;
                Line = new Line();
                wpfChart.DrawArea.Children.Add(Line);
                Line.Animate()
                    .WithSpeed(speed)
                    .Property(line => line.Opacity, 0, 1);
            }

            Line.Stroke = Brushes.Black;

            var animation = Line.Animate()
                .WithSpeed(speed)
                .Property(line => line.X1, point1.X)
                .Property(line => line.X2, point2.X)
                .Property(line => line.Y1, point1.Y)
                .Property(line => line.Y2, point2.Y);

            if (dispose)
            {
                animation.Property(p => p.Opacity, 0)
                    .Then((sender, args) =>
                    {
                        Dispose(chart);
                        animation = null;
                    });
            }
        }

        public object UpdateId { get; set; }

        public void Dispose(IChartView view)
        {
            var wpfChart = (CartesianChart) view;
            wpfChart.DrawArea.Children.Remove(Line);
            Line = null;
        }
    }
}
