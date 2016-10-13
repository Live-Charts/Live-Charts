using LiveCharts;
using LiveCharts.Uwp;
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

namespace UWP.CartesianChart.BasicLine
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BasicLineExample : Page
    {
        public BasicLineExample()
        {
            this.InitializeComponent();

            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Series 1",
                    Values = new ChartValues<double> { 4, 6, 5, 2 ,7 }
                },
                //new LineSeries
                //{
                //    Title = "Series 2",
                //    Values = new ChartValues<double> { 6, 7, 3, 4 ,6 }
                //}
            };

            //Labels = new[] { "Jan", "Feb", "Mar", "Apr", "May" };
            //YFormatter = value => value.ToString("C");

            ////modifing the series collection will animate and update the chart
            //SeriesCollection.Add(new LineSeries
            //{
            //    Values = new ChartValues<double> { 5, 3, 2, 4 },
            //    LineSmoothness = 0 //rect lines, 1 really smooth lines
            //});

            ////modifing any series values will also animate and update the chart
            //SeriesCollection[2].Values.Add(5d);

            DataContext = this;
        }

        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> YFormatter { get; set; }
    }
}
