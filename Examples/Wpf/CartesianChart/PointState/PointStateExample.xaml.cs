using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Defaults;

namespace Wpf.CartesianChart.PointState
{
    /// <summary>
    /// Interaction logic for PointStateExample.xaml
    /// </summary>
    public partial class PointStateExample : UserControl
    {
        public PointStateExample()
        {
            InitializeComponent();
            
            var r = new Random();
            Values = new ChartValues<ObservableValue>
            {
                new ObservableValue(r.Next(10, 400)),
                new ObservableValue(r.Next(10, 400)),
                new ObservableValue(r.Next(10, 400)),
                new ObservableValue(r.Next(10, 400)),
                new ObservableValue(r.Next(10, 400)),
                new ObservableValue(r.Next(10, 400))
            };

            //Lets define a custom mapper, to set fill and stroke
            //according to chart values...
            Mapper = Mappers.Xy<ObservableValue>()
                .X((item, index) => index)
                .Y(item => item.Value)
                .Fill(item => item.Value > 200 ? DangerBrush : null)
                .Stroke(item => item.Value > 200 ? DangerBrush : null);

            Formatter = x => x + " ms";

            DangerBrush = new SolidColorBrush(Color.FromRgb(238,83,80));

            DataContext = this;
        }

        public Func<double, string> Formatter { get; set; }
        public ChartValues<ObservableValue> Values { get; set; }
        public Brush DangerBrush { get; set; }
        public CartesianMapper<ObservableValue> Mapper { get; set; } 

        private void UpdateDataOnClick(object sender, RoutedEventArgs e)
        {
            var r = new Random();
            foreach (var observable in Values)
            {
                observable.Value = r.Next(10, 400);
            }
        }
    }
}
