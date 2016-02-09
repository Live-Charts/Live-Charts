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

            //create a config for StoreViewModel
            var config = new SeriesConfiguration<StoreViewModel>()
                .Y(y => y.Income); //use Income property as Y
                                   //do not configure X
                                   //this will pull a zero based index as X

            //create a SeriesCollection with this config
            StoresCollection = new SeriesCollection(config);

            //add some Series with ChartValues<StoreViewModel>
            StoresCollection.Add(new BarSeries
            {
                Title = "Apple Store",
                Values = new ChartValues<StoreViewModel>
                {
                    new StoreViewModel {Income = 15},
                    new StoreViewModel {Income = 18}
                },
                DataLabels = true
            });
            StoresCollection.Add(new BarSeries
            {
                Title = "Google Play",
                Values = new ChartValues<StoreViewModel>
                {
                    new StoreViewModel {Income = 5},
                    new StoreViewModel {Income = 7}
                },
                DataLabels = true
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

        private void PointPropertyChangedBar_OnLoaded(object sender, RoutedEventArgs e)
        {
            //this line is only to display animation every time you change view in this examples.
            Chart.Update();
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
               if (PointChanged != null) PointChanged.Invoke(this);
            }
        }

        public event Action<object> PointChanged;
    }
}
