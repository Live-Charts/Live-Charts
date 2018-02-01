using System;
using System.Threading;
using System.Threading.Tasks;
using Assets.Models;
using LiveCharts.Core;
using LiveCharts.Core.Coordinates;
using LiveCharts.Core.DataSeries;

namespace Assets.ViewModels
{
    public class PropertyAndInstanceChanged
    {
        static PropertyAndInstanceChanged()
        {
            LiveChartsSettings.Set(settings =>
                {
                    settings.Has2DPlotFor<City>((city, index) => new Point2D(index, city.Population));
                });
        }

        public PropertyAndInstanceChanged()
        {
            var columnSeries = new ColumnSeries<City>
            {
                new City
                {
                    Population = 6d
                },
                new City
                {
                    Population = 10d
                }
            };

            var r = new Random();

            Task.Run(() =>
            {
                Thread.Sleep(1000);
                foreach (var city in columnSeries)
                {
                    city.Population = r.Next(0, 10);
                }

                if (r.Next() < .1)
                {
                    columnSeries.Add(new City
                    {
                        Population = r.Next(0, 10)
                    });
                }
            });

            SeriesCollection.Add(columnSeries);
        }

        public SeriesCollection SeriesCollection { get; set; }
    }
}
