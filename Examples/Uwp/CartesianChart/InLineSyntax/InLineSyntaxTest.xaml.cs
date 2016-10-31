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

namespace UWP.CartesianChart.InLineSyntax
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class InLineSyntaxTest : Page
    {
        public InLineSyntaxTest()
        {
            this.InitializeComponent();
        }

        public IChartValues LineSeries1 { get; set; } = new ChartValues<int>(new int[] { 1, 2, 3, 5 });

        public IChartValues LineSeries2 { get; set; } = new ChartValues<int>(new int[] { 4, 6, 2, 5 });

        public string[] Labels { get; set; } = new string[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Ago", "Sep", "Oct" };
    }
}
