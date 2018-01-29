using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using LiveCharts.Core;
using LiveCharts.Core.Coordinates;
using LiveCharts.Core.Data;
using LiveCharts.Core.DataSeries;

namespace Samples.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static MainWindow()
        {
            LiveChartsSettings.Define(
                settings =>
                {
                    settings
                        .Has2DPlotFor<City>(
                            (city, index) => new Point2D(index, city.Population)
                        )
                        .When(city => city.Population > 1000,
                            (city, args) =>
                            {
                                var shape = (Shape) args.Visual;
                                shape.Fill = Brushes.Red;
                            })
                        .LabeledAs(city => $"{city.Population:N2}");
                }
            );
        }

        public MainWindow()
        {
             InitializeComponent();

            var city = new City
            {
                Population = 4
            };

            var collection = new ObservableCollection<City>
            {
                city,
                new City
                {
                    Population = 5
                }
            };

            Series = new ObservableCollection<Series>
            {
                new ColumnSeries<City>
                {
                    Values = collection,
                    Title = "hola"
                }
            };

            var r = new Random();
            Task.Run(() =>
            {
                while (true)
                {
                    Thread.Sleep(2000);
                    city.Population = r.Next(0, 10);
                }
            });

            DataContext = Series;
        }

        public ObservableCollection<Series> Series { get; set; }
    }

    public class City : INotifyPropertyChanged
    {
        private double _population;

        public double Population

        {
            get => _population;
            set
            {
                _population = value; 
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
