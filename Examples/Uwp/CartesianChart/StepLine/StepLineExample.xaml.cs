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

namespace UWP.CartesianChart.StepLine
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class StepLineExample : Page
    {
        public StepLineExample()
        {
            this.InitializeComponent();
        }

        public IChartValues ChartValues1 { get; set; } = new ChartValues<int>(new int[] { 9, 6, 5, 7, 8, 9, 7, 6, 7, 5 });
        public IChartValues ChartValues2 { get; set; } = new ChartValues<int>(new int[] { 1, 4, 3, 1, 4, 2, 1, 2, 3, 5 });
    }
}
