using System.Collections.ObjectModel;
using LiveCharts.Core.DataSeries;
using LiveCharts.Core.Defaults;

namespace Assets.ViewModels
{
    public class UserInteractiveData
    {
        public UserInteractiveData()
        {
            SeriesCollection = new ObservableCollection<ISeries>
            {
                new BarSeries<double>
                {
                    Values = new ObservableCollection<double> {3, 8, 2, 6, 8}
                },
                new ScatterSeries<PointModel>
                {
                    Values = new ObservableCollection<PointModel>
                    {
                        new PointModel(0, 4),
                        new PointModel(1, 8),
                        new PointModel(2, 6),
                        new PointModel(3, 2),
                        new PointModel(4, 9)
                    }
                }
            };
        }

        public ObservableCollection<ISeries> SeriesCollection { get; set; }
    }
}
