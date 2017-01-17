using System;
using System.Windows.Media;
using System.Windows.Forms;
using LiveCharts;
using LiveCharts.Events;
using LiveCharts.Wpf;

namespace Winforms.Cartesian.Events
{
    public partial class EventsExample : Form
    {
        public EventsExample()
        {
            InitializeComponent();

            cartesianChart1.Series.Add(new LineSeries
            {
                Values = new ChartValues<double> { 4, 6, 5, 3, 5 },
                Fill = Brushes.Transparent,
                StrokeThickness = 4,
                PointGeometrySize = 25
            });

            var ax = new Axis();
            ax.RangeChanged += AxOnRangeChanged; 

            cartesianChart1.AxisX.Add(ax);
        }

        private void ChartOnDataClick(object sender, ChartPoint p)
        {
            var asPixels = cartesianChart1.Base.ConvertToPixels(p.AsPoint());
            Console.WriteLine("[EVENT] You clicked (" + p.X + ", " + p.Y + ") in pixels (" +
                            asPixels.X + ", " + asPixels.Y + ")");
        }

        private void Chart_OnDataHover(object sender, ChartPoint p)
        {
            Console.WriteLine("[EVENT] you hovered over " + p.X + ", " + p.Y);
        }

        private void ChartOnUpdaterTick(object sender)
        {
            Console.WriteLine("[EVENT] chart was updated");
        }

        private void AxOnRangeChanged(RangeChangedEventArgs eventArgs)
        {
            Console.WriteLine("[EVENT] axis range changed");
        }
    }
}
