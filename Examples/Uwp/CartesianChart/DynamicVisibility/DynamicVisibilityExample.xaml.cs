using LiveCharts;
using System.ComponentModel;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace UWP.CartesianChart.DynamicVisibility
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DynamicVisibilityExample : Page, INotifyPropertyChanged
    {
        private bool _mariaSeriesVisibility;
        private bool _charlesSeriesVisibility;
        private bool _johnSeriesVisibility;

        public DynamicVisibilityExample()
        {
            InitializeComponent();

            MariaSeriesVisibility = true;
            CharlesSeriesVisibility = true;
            JohnSeriesVisibility = false;

            DataContext = this;
        }

        public bool MariaSeriesVisibility
        {
            get { return _mariaSeriesVisibility; }
            set
            {
                _mariaSeriesVisibility = value;
                OnPropertyChanged("MariaSeriesVisibility");
            }
        }

        public bool CharlesSeriesVisibility
        {
            get { return _charlesSeriesVisibility; }
            set
            {
                _charlesSeriesVisibility = value;
                OnPropertyChanged("CharlesSeriesVisibility");
            }
        }

        public bool JohnSeriesVisibility
        {
            get { return _johnSeriesVisibility; }
            set
            {
                _johnSeriesVisibility = value;
                OnPropertyChanged("JohnSeriesVisibility");
            }
        }

        public IChartValues MariaValues { get; set; } = new ChartValues<int>(new int[] { 4, 7, 2, 9, 3 });
        public IChartValues CharlesValues { get; set; } = new ChartValues<int>(new int[] { 6, 2, 6, 3, 8 });
        public IChartValues JohnValues { get; set; } = new ChartValues<int>(new int[] { 7, 2, 8, 3, 9 });
        public string[] Labels { get; set; } = new string[] { "January", "February", "March", "April", "May" };

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
