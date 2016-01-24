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

            StoresCollection = new SeriesCollection(new SeriesConfiguration<StoreViewModel>().Y(y => y.Income));
            StoresCollection.Add(new BarSeries
            {
                Title = "Apple Store",
                Values = new ChartValues<StoreViewModel>
                {
                    new StoreViewModel {Income = 15, Collection = StoresCollection}
                }
            });
            StoresCollection.Add(new BarSeries
            {
                Title = "Google Play",
                Values = new ChartValues<StoreViewModel>
                {
                    new StoreViewModel {Income = 5, Collection = StoresCollection}
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
                UpdateChart();
            }
        }

        public SeriesCollection Collection { get; set; }
        public void UpdateChart()
        {
            //when you create the object there is no Collection or chart properties assigned
            //live charts will assign them when ploting.
            if (Collection == null) return;
            if (Collection.Chart == null) return;
            
            //In this case we force all values to evaluate,
            //you can implement a smarter logic
            //according to your needs
            foreach (var values in Collection.Select(x => x.Values))
                values.RequiresEvaluation = true;

            //by now disable animations with the false parameter,
            //series are not ready to plot partial changes
            //and you could get undesired animation
            Collection.Chart.ClearAndPlot(false);
        }
    }
}
