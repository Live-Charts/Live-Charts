using System;
using System.Linq;
using LiveCharts;
using LiveCharts.CoreComponents;

namespace WinForms.LineExamples.Mvvm
{
    public class SeriesModel : SeriesCollection
    {
        private Random _r = new Random();

        public SeriesModel()
        {
            Configuration = new SeriesConfiguration<SalesInfo>().Y(val => (double) val.Income);

            //series using SeriesCollection configuration
            Add(new LineSeries
            {
                Title = "Series 1",
                Values = new ChartValues<SalesInfo>
                {
                    new SalesInfo{Income = 10000, Rentability = .10},
                    new SalesInfo{Income = 8000, Rentability = .12},
                    new SalesInfo{Income = 6000, Rentability = .13},
                    new SalesInfo{Income = 13000, Rentability = .12},
                    new SalesInfo{Income = 11000, Rentability = .11}
                }
            });
            //this one too, using SeriesCollection configuration 
            Add(new LineSeries
            {
                Title = "Series 2",
                Values = new ChartValues<SalesInfo>
                {
                    new  SalesInfo{Income = 10000, Rentability = .10},
                    new  SalesInfo{Income = 8000, Rentability = .12},
                    new  SalesInfo{Income = 6000, Rentability = .13},
                    new  SalesInfo{Income = 13000, Rentability = .12},
                    new  SalesInfo{Income = 11000, Rentability = .11}
                }
            });

            //now lets map to another type, we need to configure this type too.
            var averageConfig = new SeriesConfiguration<AverageSalesDto>()
                .Y(val => val.AverageIncome);

            Add(new LineSeries
            {
                Title = "Average Series",
                Values = new ChartValues<AverageSalesDto>
                {
                    new AverageSalesDto {AverageIncome = 9000},
                    new AverageSalesDto {AverageIncome = 7000},
                    new AverageSalesDto {AverageIncome = 8000},
                    new AverageSalesDto {AverageIncome = 16000},
                    new AverageSalesDto {AverageIncome = 12000}
                }
            }.Setup(averageConfig));
        }

        public void AddRandomSeries()
        {
            var c = this.FirstOrDefault() == null ? 5 : this.First().Values.Count;

            var values = new ChartValues<SalesInfo>();
            for (int i = 0; i < c; i++)
            {
                values.Add(new SalesInfo
                {
                    Id = 1,
                    Income = _r.Next(5000, 15000),
                    Rentability = _r.NextDouble()*.15
                });
            }
            Add(new LineSeries { Title = "Random Series", Values = values });
        }

        public void RemoveSeries()
        {
            if (Count > 0) RemoveAt(0);
        }

        public void AddPoint()
        {
            foreach (var series in this.Where(x => x.Title != "Average Series"))
            {
                series.Values.Add(new SalesInfo
                {
                    Id = 1,
                    Income = _r.Next(5000, 15000),
                    Rentability = _r.NextDouble()*.15
                });
            }
            var av = this.FirstOrDefault(x => x.Title == "Average Series");
            if (av != null)
                av.Values.Add(new AverageSalesDto
                {
                    AverageIncome = _r.Next(5000, 15000)
                });
        }

        public void RemovePoint()
        {
            foreach (var series in this)
            {
                if (series.Values.Count > 1) series.Values.RemoveAt(0);
            }
        }
    }
}
