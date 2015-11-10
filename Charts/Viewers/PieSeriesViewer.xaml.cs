using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using LiveCharts.Series;

namespace LiveCharts.Viewers
{
    /// <summary>
    /// Interaction logic for PieSeriesViewer.xaml
    /// </summary>
    public partial class PieSeriesViewer
    {
        public PieSeriesViewer()
        {
            InitializeComponent();
            DataContext = this;
        }

        public static readonly DependencyProperty SeriesProperty = DependencyProperty.Register(
            "Series", typeof(ObservableCollection<Serie>), typeof(PieSeriesViewer), new PropertyMetadata(null));

        public ObservableCollection<Serie> Series
        {
            get { return (ObservableCollection<Serie>)GetValue(SeriesProperty); }
            set { SetValue(SeriesProperty, value); }
        }

        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(
            "Orientation", typeof(Orientation), typeof(PieSeriesViewer), new PropertyMetadata(Orientation.Vertical));

        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }
    }
}
