using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LiveCharts;

namespace ChartsTest.z.DebugCases
{
    /// <summary>
    /// Interaction logic for Issue102.xaml
    /// </summary>
    public partial class Issue102 : UserControl
    {
        public Issue102()
        {
            InitializeComponent();
        }

        LineChart m_chart;
        SeriesCollection m_series;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var rand = new Random();
            m_series.Clear();
            for (int i = 0; i < 10; i++)
            {
                var series = new LineSeries();
                series.Values = new ChartValues<double>();
                for (int j = 0; j < 10; j++)
                {
                    series.Values.Add(rand.NextDouble());
                }
                m_series.Add(series);
            }
        }

        private void Issue102_OnLoaded(object sender, RoutedEventArgs e)
        {
            m_chart = new LineChart();
            m_series = new SeriesCollection();
            m_chart.Series = m_series;
            mainGrid.Children.Add(m_chart);
        }
    }
}
