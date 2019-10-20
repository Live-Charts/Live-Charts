using System;
using System.Windows.Controls;
using System.Windows.Media;
using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Wpf;

namespace Wpf.CartesianChart.Using_DateTime
{
    public partial class DateTime : UserControl
    {
        public DateTime()
        {
            InitializeComponent();

            var dayConfig = Mappers.Xy<DateModel>()
                .X(dayModel => (double) dayModel.DateTime.Ticks/TimeSpan.FromHours(1).Ticks)
                .Y(dayModel => dayModel.Value);

            //Notice you can also configure this type globally, so you don't need to configure every
            //SeriesCollection instance using the type.
            //more info at http://lvcharts.net/App/Index#/examples/v1/wpf/Types%20and%20Configuration

            Series = new SeriesCollection(dayConfig)
            {
                new LineSeries
                {
                    Values = new ChartValues<DateModel>
                    {
                        new DateModel
                        {
                            DateTime = System.DateTime.Now,
                            Value = 5
                        },
                        new DateModel
                        {
                            DateTime = System.DateTime.Now.AddHours(2),
                            Value = 9
                        }
                    },
                    Fill = Brushes.Transparent
                },
                new ColumnSeries
                {
                    Values = new ChartValues<DateModel>
                    {
                        new DateModel
                        {
                            DateTime = System.DateTime.Now,
                            Value = 4
                        },
                        new DateModel
                        {
                            DateTime = System.DateTime.Now.AddHours(1),
                            Value = 6
                        },
                        new DateModel
                        {
                            DateTime = System.DateTime.Now.AddHours(2),
                            Value = 8
                        }
                    }
                }
            };

            Formatter = value => new System.DateTime((long) (value*TimeSpan.FromHours(1).Ticks)).ToString("t");
            
            DataContext = this;
        }

        public Func<double, string> Formatter { get; set; }
        public SeriesCollection Series { get; set; }
    }
}
