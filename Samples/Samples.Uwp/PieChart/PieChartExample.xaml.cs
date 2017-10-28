using LiveCharts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace UWP.PieChart
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PieChartExample : Page
    {
        public PieChartExample()
        {
            InitializeComponent();

            PointLabel = chartPoint =>
                string.Format("{0} ({1:P})", chartPoint.Y, chartPoint.Participation);

            DataContext = this;
        }

        public Func<ChartPoint, string> PointLabel { get; set; }

        public IChartValues Values1 { get; set; } = new ChartValues<int>(new int[] { 3 });
        public IChartValues Values2 { get; set; } = new ChartValues<int>(new int[] { 3 });
        public IChartValues Values3 { get; set; } = new ChartValues<int>(new int[] { 3 });
        public IChartValues Values4 { get; set; } = new ChartValues<int>(new int[] { 3 });
    }
}
