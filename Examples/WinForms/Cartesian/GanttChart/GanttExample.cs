using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;

namespace Winforms.Cartesian.GanttChart
{
    public partial class GanttExample : Form
    {
        private ChartValues<GanttPoint> _values;

        public GanttExample()
        {
            InitializeComponent();

            var now = System.DateTime.Now;

            _values = new ChartValues<GanttPoint>
            {
                new GanttPoint(now.Ticks, now.AddDays(2).Ticks),
                new GanttPoint(now.AddDays(1).Ticks, now.AddDays(3).Ticks),
                new GanttPoint(now.AddDays(3).Ticks, now.AddDays(5).Ticks),
                new GanttPoint(now.AddDays(5).Ticks, now.AddDays(8).Ticks),
                new GanttPoint(now.AddDays(6).Ticks, now.AddDays(10).Ticks),
                new GanttPoint(now.AddDays(7).Ticks, now.AddDays(14).Ticks),
                new GanttPoint(now.AddDays(9).Ticks, now.AddDays(12).Ticks),
                new GanttPoint(now.AddDays(9).Ticks, now.AddDays(14).Ticks),
                new GanttPoint(now.AddDays(10).Ticks, now.AddDays(11).Ticks),
                new GanttPoint(now.AddDays(12).Ticks, now.AddDays(16).Ticks),
                new GanttPoint(now.AddDays(15).Ticks, now.AddDays(17).Ticks),
                new GanttPoint(now.AddDays(18).Ticks, now.AddDays(19).Ticks)
            };

            cartesianChart1.Zoom = ZoomingOptions.X;

            cartesianChart1.Series = new SeriesCollection
            {
                new RowSeries
                {
                    Values = _values,
                    DataLabels = true
                }
            };

            cartesianChart1.AxisX.Add(new Axis
            {
                LabelFormatter = value => new System.DateTime((long)value).ToString("dd MMM")
            });

            var labels = new List<string>();
            for (var i = 0; i < 12; i++)
                labels.Add("Task " + i);
            cartesianChart1.AxisY.Add(new Axis
            {
                Labels = labels.ToArray()
            });

            button1_Click(null, null);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            cartesianChart1.AxisX[0].MinValue = _values.First().StartPoint;
            cartesianChart1.AxisX[0].MaxValue = _values.Last().EndPoint;
        }
    }
}
