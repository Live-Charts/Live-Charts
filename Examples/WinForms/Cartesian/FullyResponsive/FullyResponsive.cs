using System;
using System.Windows.Forms;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;

namespace Winforms.Cartesian.FullyResponsive
{
    public partial class FullyResponsive : Form
    {
        public FullyResponsive()
        {
            InitializeComponent();

            Values = new ChartValues<ObservableValue>
            {
                new ObservableValue(3),
                new ObservableValue(6),
                new ObservableValue(7),
                new ObservableValue(4),
                new ObservableValue(2)
            };

            cartesianChart1.LegendLocation = LegendLocation.Right;
        }

        public ChartValues<ObservableValue> Values { get; set; }

        private void FullyResponsive_Load(object sender, EventArgs e)
        {
            cartesianChart1.Series.Add(new LineSeries
            {
                Values = Values,
                StrokeThickness = 4,
                PointGeometrySize = 0,
                DataLabels = true
            });
        }

        private void AddButtonOnClick(object sender, EventArgs e)
        {
            var r = new Random();
            Values.Add(new ObservableValue(r.Next(-20, 20)));
        }

        private void InsertButtonOnClick(object sender, EventArgs e)
        {
            var r = new Random();
            if (Values.Count > 3)
                Values.Insert(2, new ObservableValue(r.Next(-20, 20)));
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Values.RemoveAt(0);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var r = new Random();
            foreach (var observable in Values)
            {
                observable.Value = r.Next(-20, 20);
            }
        }
    }
}
