using System;
using System.Linq;
using System.Windows;
using LiveCharts;
using LiveCharts.CoreComponents;

namespace ChartsTest.BarExamples.UsingObservableChartPoint
{
    /// <summary>
    /// Interaction logic for PropertyChangedBar.xaml
    /// </summary>
    public partial class PointPropertyChangedBar
    {
        public PointPropertyChangedBar()
        {
            InitializeComponent();

            var config = new SeriesConfiguration<StoreViewModel>().Y(y => y.Income);

            StoresCollection = new SeriesCollection(config);

            StoresCollection.Add(new BarSeries
            {
                Title = "Apple Store",
                Values = new ChartValues<StoreViewModel>
                {
                    new StoreViewModel {Income = 15}
                }
            });
            StoresCollection.Add(new BarSeries
            {
                Title = "Google Play",
                Values = new ChartValues<StoreViewModel>
                {
                    new StoreViewModel {Income = 5}
                }
            });

            DataContext = this;
        }

        public SeriesCollection StoresCollection { get; set; }

        private void UpdateValueOnClick(object sender, RoutedEventArgs e)
        {
            var r = new Random();
            foreach (var series in StoresCollection)
            {
                foreach (var value in series.Values)
                {
                    var storeVm = value as StoreViewModel;
                    if (storeVm == null) continue;
                    storeVm.Income = r.Next(3, 20);
                }
            }
        }
    }

    //Implement ObservableChartPoint and specify the logic to follow when data Changes
    public class StoreViewModel : IObservableChartPoint
    {
        private double _income;

        public double Income
        {
            get { return _income; }
            set
            {
                _income = value;
               if (ValueChanged != null) ValueChanged.Invoke();
            }
        }

        public event Action ValueChanged;
    }
}
