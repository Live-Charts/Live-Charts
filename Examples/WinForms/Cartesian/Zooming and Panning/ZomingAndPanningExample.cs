using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using Color = System.Drawing.Color;
using Point = System.Windows.Point;

namespace Winforms.Cartesian.Zooming_and_Panning
{
    public partial class ZomingAndPanningExample : Form
    {
        public ZomingAndPanningExample()
        {
            InitializeComponent();

            var gradientBrush = new LinearGradientBrush
            {
                StartPoint = new System.Windows.Point(0, 0),
                EndPoint = new Point(0, 1)
            };
            gradientBrush.GradientStops.Add(new GradientStop(System.Windows.Media.Color.FromRgb(33, 148, 241), 0));
            gradientBrush.GradientStops.Add(new GradientStop(Colors.Transparent, 1));


            cartesianChart1.Series.Add(new LineSeries
            {
                Values = GetData(),
                Fill = gradientBrush,
                StrokeThickness = 1,
                PointGeometry = null
            });

            cartesianChart1.Zoom = ZoomingOptions.X;

            cartesianChart1.AxisX.Add(new Axis
            {
                LabelFormatter = val => new System.DateTime((long)val).ToString("dd MMM")
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

        private void ClearZoom()
        {
            //to clear the current zoom/pan just set the axis limits to double.NaN

            cartesianChart1.AxisX[0].MinValue = double.NaN;
            cartesianChart1.AxisX[0].MaxValue = double.NaN;
            cartesianChart1.AxisY[0].MinValue = double.NaN;
            cartesianChart1.AxisY[0].MaxValue = double.NaN;
        }

        private void ZomingAndPanningExample_Load(object sender, EventArgs e)
        {

        }
    }
}
