using System.Collections.Generic;
using LiveCharts.Core.DataSeries;
using LiveCharts.Core.Drawing.Svg;

namespace Assets.ViewModels
{
    public class DataLabels
    {
        public DataLabels()
        {
            SeriesCollection = new List<ISeries>
            {
                new LineSeries<double>
                {
                    Values = new List<double>
                    {
                        3,
                        8,
                        2,
                        9,
                        5
                    },
                    DataLabels = true
                },
                new BarSeries<double>
                {
                    Values = new List<double>
                    {
                        8,
                        2,
                        5,
                        1,
                        7
                    },
                    DataLabels = true,
                    DefaultFillOpacity = .2
                },
                new ScatterSeries<double>
                {
                    Values = new List<double>
                    {
                        3,
                        6,
                        9,
                        2,
                        1
                    },
                    Geometry = Geometry.Diamond,
                    DataLabels = true
                }
            };
        }

        public IEnumerable<ISeries> SeriesCollection { get; set; }
    }
}
