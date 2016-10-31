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

namespace UWP.CartesianChart.Inverted_Series
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class InvertedExample : Page
    {
        public InvertedExample()
        {
            InitializeComponent();

            Values1 = new ChartValues<double> { 3, 5, 2, 6, 2, 7, 1 };

            Values2 = new ChartValues<double> { 6, 2, 6, 3, 2, 7, 2 };

            DataContext = this;
        }

        public ChartValues<double> Values1 { get; set; }
        public ChartValues<double> Values2 { get; set; }
    }
}
