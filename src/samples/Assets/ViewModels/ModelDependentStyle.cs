using System.Collections.Generic;
using System.Collections.ObjectModel;
using Assets.Models;
using LiveCharts.Core.DataSeries;

namespace Assets.ViewModels
{
    public class ModelDependentStyle
    {
        public ModelDependentStyle()
        {
            SeriesCollection = new ObservableCollection<ISeries>
            {
                new LineSeries<Player>
                {
                    Values = new ObservableCollection<Player>
                    {
                        new Player
                        {
                            Score = -1
                        },
                        new Player
                        {
                            Score = 1
                        }
                    }
                }
            };
        }

        public IEnumerable<ISeries> SeriesCollection { get; set; }

    }
}