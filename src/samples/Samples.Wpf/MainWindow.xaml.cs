using System.Collections.ObjectModel;
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
                    settings.Has2DPlotFor<City>(
                            (city, index) => new Point2D(index, city.Population))
                        .When(city => city.Population > 1000,
                            (city, args) =>
                            {
                                var shape = (Shape) args.VisualElement;
                                shape.Fill = Brushes.Red;
                            })
                        .When(UiActions.PointerEnter,
                            (city, args) =>
                            {

                            });
                }
            );
        }

        public MainWindow()
        {
            // InitializeComponent();

            DataContext = this;

            Series = new ObservableCollection<ISeries>
            {
                new ColumnSeries<double>
                {
                    Values = new[] {3d, 4d}
                }
            };
        }

        public ObservableCollection<ISeries> Series { get; set; }
    }

    public class City
    {
        public double Population { get; set; }
    }
}
