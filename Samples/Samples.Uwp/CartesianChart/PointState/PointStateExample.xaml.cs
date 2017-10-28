using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Defaults;
using System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace UWP.CartesianChart.PointState
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PointStateExample : Page
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
            OkBrush = new SolidColorBrush(Color.FromArgb(255, 254, 192, 7));
            DangerBrush = new SolidColorBrush(Color.FromArgb(255, 238, 83, 80));

            DataContext = this;
        }

        public Func<double, string> Formatter { get; set; }
        public ChartValues<ObservableValue> Values { get; set; }
        public Brush OkBrush { get; set; }
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
