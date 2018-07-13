using System;
using System.Collections.ObjectModel;
using System.Linq;
using LiveCharts.Core.DataSeries;
using LiveCharts.Core.Defaults;

namespace Assets.ViewModels
{
    public class UserInteractiveData
    {
        public UserInteractiveData()
        {
            var r = new Random();
            int[] generator = new int[35];

            SeriesCollection = new ObservableCollection<ISeries>
            {
                new ScatterSeries<PointModel>
                {
                    Values = new ObservableCollection<PointModel> (generator.Select(x => new PointModel(r.Next(0,50), r.Next(0,50))))
                }
            };
        }

        public ObservableCollection<ISeries> SeriesCollection { get; set; }
    }
}
