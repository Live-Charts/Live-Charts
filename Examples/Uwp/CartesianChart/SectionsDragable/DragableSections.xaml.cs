using LiveCharts;
using System.ComponentModel;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace UWP.CartesianChart.SectionsDragable
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DragableSections : Page, INotifyPropertyChanged
    {
        private double _xSection;
        private double _ySection;

        public DragableSections()
        {
            InitializeComponent();

            XSection = 5;
            YSection = 5;

            DataContext = this;
        }

        public double XSection
        {
            get { return _xSection; }
            set
            {
                _xSection = value;
                OnPropertyChanged("XSection");
            }
        }

        public double YSection
        {
            get { return _ySection; }
            set
            {
                _ySection = value;
                OnPropertyChanged("YSection");
            }
        }

        public IChartValues ChartValues { get; set; } = new ChartValues<int>(new int[] { 7, 2, 8, 2, 7, 4, 9, 4, 2, 8 });

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
