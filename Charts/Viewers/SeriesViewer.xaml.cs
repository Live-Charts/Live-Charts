using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace LiveCharts
{
    /// <summary>
    /// Interaction logic for SeriesViewer.xaml
    /// </summary>
    public partial class SeriesViewer
    {
        public SeriesViewer()
        {
            InitializeComponent();
            DataContext = this;
        }

        public static readonly DependencyProperty SeriesProperty = DependencyProperty.Register(
            "Series", typeof (ObservableCollection<Serie>), typeof (SeriesViewer), new PropertyMetadata(null));

        public ObservableCollection<Serie> Series
        {
            get { return (ObservableCollection<Serie>) GetValue(SeriesProperty); }
            set { SetValue(SeriesProperty, value); }
        }

        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register(
            "Orientation", typeof (Orientation), typeof (SeriesViewer), new PropertyMetadata(Orientation.Vertical));

        public Orientation Orientation
        {
            get { return (Orientation) GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }
    }
}
