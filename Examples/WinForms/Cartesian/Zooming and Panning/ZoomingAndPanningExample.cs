using System;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;
using LiveCharts;
using LiveCharts.Wpf;
using LiveCharts.Defaults;

namespace Winforms.Cartesian.Zooming_and_Panning
{
    public partial class ZoomingAndPanningExample : Form
    {
        public ZoomingAndPanningExample()
        {
            InitializeComponent();


            var gradientBrush = new LinearGradientBrush
            {
                StartPoint = new Point(0, 0),
                EndPoint = new Point(0, 1)
            };
            gradientBrush.GradientStops.Add(new GradientStop(Color.FromRgb(33, 148, 241), 0));
            gradientBrush.GradientStops.Add(new GradientStop(Colors.Transparent, 1));


            cartesianChart1.Series.Add(new LineSeries
            {
                Values = GetData(),
                Fill = gradientBrush,
                StrokeThickness = 1,
                PointDiameter = 0
            });

            cartesianChart1.Zoom  = ZoomingOptions.X;

            cartesianChart1.AxisX.Add(new Axis
            {
                LabelFormatter = val => new System.DateTime((long) val).ToString("dd MMM")
            });

            cartesianChart1.AxisY.Add(new Axis
            {
                LabelFormatter = val => val.ToString("C")
            });

        }

        private ChartValues<DateTimePoint> GetData()
        {
            var r = new Random();
            var trend = 100;
            var values = new ChartValues<DateTimePoint>();

            for (var i = 0; i < 100; i++)
            {
                var seed = r.NextDouble();
                if (seed > .8) trend += seed > .9 ? 50 : -50;
                values.Add(new DateTimePoint(System.DateTime.Now.AddDays(i), trend + r.Next(0, 10)));
            }

            return values;
        }
    }
}
