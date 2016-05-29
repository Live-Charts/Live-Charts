using LiveCharts;
using LiveCharts.Wpf;

namespace Wpf.CartesianChart.Customized_Line_Series
{
    /// <summary>
    /// Interaction logic for CustomizedExample.xaml
    /// </summary>
    public partial class CustomizedLineSeries 
    {
        public CustomizedLineSeries()
        {
            InitializeComponent();

            Values1 = new ChartValues<double>
            {
                3,
                4,
                6,
                3,
                2,
                6
            };
            Values2 = new ChartValues<double>
            {
                5,
                3,
                5,
                7,
                3,
                9
            };

            DataContext = this;
        }

        public ChartValues<double> Values1 { get; set; }
        public ChartValues<double> Values2 { get; set; }
    }
}
