using LiveCharts;
using LiveCharts.Uwp.Charts.Base;
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
using LiveCharts.Uwp;
using Windows.UI.Popups;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace UWP.CartesianChart.UIElements
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class UIElementsExample : Page
    {
        public UIElementsExample()
        {
            InitializeComponent();
            DataContext = this;
        }

        private async void ChartOnDataClick(object sender, ChartPoint chartPoint)
        {
            var asPixels = this.Chart.ConvertToPixels(chartPoint.AsPoint());
            var dialog = new MessageDialog("You clicked (" + chartPoint.X + ", " + chartPoint.Y + ") in pixels (" +
                            asPixels.X + ", " + asPixels.Y + ")");
            await dialog.ShowAsync();
        }

        //TODO
        //private void ChartMouseMove(object sender, MouseEventArgs e)
        //{
        //    var point = Chart.ConvertToChartValues(e.GetPosition(Chart));

        //    X.Text = point.X.ToString("N");
        //    Y.Text = point.Y.ToString("N");
        //}

        public IChartValues Values1 { get; set; } = new ChartValues<int>(new int[] { 4, 6, 6, 3, 5 });

        public IChartValues Values2 { get; set; } = new ChartValues<int>(new int[] { 2, 6, 8, 9, 4 });

        public string[] Labels { get; set; } = new string[] { "Jan", "Feb", "Mar", "Abr", "May" };
    }
}
