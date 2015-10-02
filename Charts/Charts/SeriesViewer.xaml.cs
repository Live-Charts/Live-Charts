using System.Collections.ObjectModel;
using System.Windows;
using LiveCharts.Series;

namespace LiveCharts.Charts
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
    }
}
