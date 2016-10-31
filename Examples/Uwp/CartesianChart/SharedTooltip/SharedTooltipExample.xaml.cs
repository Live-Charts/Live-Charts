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

namespace UWP.CartesianChart.SharedTooltip
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SharedTooltipExample : Page
    {
        public SharedTooltipExample()
        {
            InitializeComponent();

            LabelPoint = x => "A long label so it overflows " + x;

            DataContext = this;
        }

        public Func<ChartPoint, string> LabelPoint { get; set; }

        public IChartValues ChartValues { get; set; } = new ChartValues<int>(new int[] { 1, 2, 3, 4, 5 });
    }
}
