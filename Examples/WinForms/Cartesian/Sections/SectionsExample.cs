using System.Collections.Generic;
using System.Windows.Forms;
using System.Windows.Media;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;

namespace Winforms.Cartesian.Sections
{
    public partial class SectionsExample : Form
    {
        public SectionsExample()
        {
            InitializeComponent();

            cartesianChart1.Series = new SeriesCollection
            {
                new LineSeries
                {
                    Values = new ChartValues<ObservableValue>
                    {
                        new ObservableValue(3),
                        new ObservableValue(5),
                        new ObservableValue(2),
                        new ObservableValue(7),
                        new ObservableValue(7),
                        new ObservableValue(4)
                    },
                    PointGeometry = DefaultGeometries.None,
                    StrokeThickness = 4,
                    Fill = Brushes.Transparent
                },
                new LineSeries
                {
                    Values = new ChartValues<ObservableValue>
                    {
                        new ObservableValue(3),
                        new ObservableValue(4),
                        new ObservableValue(6),
                        new ObservableValue(8),
                        new ObservableValue(7),
                        new ObservableValue(5)
                    },
                    PointGeometry = DefaultGeometries.None,
                    StrokeThickness = 4,
                    Fill = Brushes.Transparent
                }
            };

            cartesianChart1.AxisY.Add(new Axis
            {
                Sections = new SectionsCollection
                {
                    new AxisSection
                    {
                        Value = 8.5,
                        Stroke = new SolidColorBrush(System.Windows.Media.Color.FromRgb(248, 213, 72))
                    }, 
                    new AxisSection
                    {
                        Label = "Good",
                        Value = 4,
                        SectionWidth = 4,
                        Fill = new SolidColorBrush
                        {
                            Color = System.Windows.Media.Color.FromRgb(204,204,204),
                            Opacity = .4
                        }
                    },
                    new AxisSection
                    {
                        Label = "Bad",
                        Value = 0,
                        SectionWidth = 4,
                        Fill = new SolidColorBrush
                        {
                            Color = System.Windows.Media.Color.FromRgb(254,132,132),
                            Opacity = .4
                        }
                    }
                }
            });

        }
    }
}
