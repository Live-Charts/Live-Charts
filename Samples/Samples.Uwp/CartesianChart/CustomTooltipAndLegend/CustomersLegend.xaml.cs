using LiveCharts.Uwp;
using System.Collections.Generic;
using System.ComponentModel;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace UWP.CartesianChart.CustomTooltipAndLegend
{
    public sealed partial class CustomersLegend : UserControl, IChartLegend
    {
        private List<SeriesViewModel> _series;

        public CustomersLegend()
        {
            InitializeComponent();

            DataContext = this;
        }

        public List<SeriesViewModel> Series
        {
            get { return _series; }
            set
            {
                _series = value;
                OnPropertyChanged("Series");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
