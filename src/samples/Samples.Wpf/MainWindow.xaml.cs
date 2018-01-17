using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using LiveCharts.Core;
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Coordinates;
using LiveCharts.Core.Data;
using LiveCharts.Core.Series;

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
                        .When(UiActions.PointerEnter,
                            (city, args) =>
                            {

                            })
                        .LabeledAs(city => $"{city.Population:N2}");
                }
            );
        }

        public MainWindow()
        {
            // InitializeComponent();

            Series = new ObservableCollection<Series>
            {
                new ColumnSeries<double>
                {
                    Values = new[] {3d, 4d}, Title = "hola"
                }
            };

            DataContext = Series;
        }

        public ObservableCollection<Series> Series { get; set; }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            var isInUiTread = Dispatcher.CheckAccess();
        }
    }

    public class City : INotifyPropertyChanged
    {
        private double _population;

        public double Population

        {
            get { return _population; }
            set { _population = value; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
