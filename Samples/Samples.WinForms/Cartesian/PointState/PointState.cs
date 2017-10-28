using System;
using System.Windows.Forms;
using System.Windows.Media;
using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Defaults;
using LiveCharts.Wpf;

namespace Winforms.Cartesian.PointState
{
    public partial class PointState : Form
    {
        private ChartValues<ObservableValue> _values;

        public PointState()
        {
            InitializeComponent();

            var r = new Random();
            _values = new ChartValues<ObservableValue>
            {
                new ObservableValue(r.Next(10, 400)),
                new ObservableValue(r.Next(10, 400)),
                new ObservableValue(r.Next(10, 400)),
                new ObservableValue(r.Next(10, 400)),
                new ObservableValue(r.Next(10, 400)),
                new ObservableValue(r.Next(10, 400))
            };

            var dangerBrush = new SolidColorBrush(Color.FromRgb(238, 83, 80));

            //Lets define a custom mapper, to set fill and stroke
            //according to chart values...
            var mapper = Mappers.Xy<ObservableValue>()
                .X((item, index) => index)
                .Y(item => item.Value)
                .Fill(item => item.Value > 200 ? dangerBrush : null)
                .Stroke(item => item.Value > 200 ? dangerBrush : null);

            cartesianChart1.Series.Add(new LineSeries
            {
                Configuration = mapper,
                Values = _values,
                PointGeometrySize = 20,
                PointForeground = Brushes.White
            });

            cartesianChart1.AxisY.Add(new Axis
            {
                LabelFormatter = x => x + " ms"
            });
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var r = new Random();
            foreach (var observable in _values)
            {
                observable.Value = r.Next(10, 400);
            }
        }
    }
}
