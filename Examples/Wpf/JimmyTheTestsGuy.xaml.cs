using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;

namespace Wpf
{
   
    //This is Jimmy, be rude with him.

    public partial class JimmyTheTestsGuy
    {
        public JimmyTheTestsGuy()
        {
            InitializeComponent();
        }
    }

    public class TestVm
    {
        public TestVm()
        {
            var r = new Random();

            var values = new ChartValues<double>
            {
                12,6,8,2,8,2,11
            };

            SeriesCollection = new SeriesCollection
            {
                new ColumnSeries
                {
                    Values = values
                }
            };
        }

        public SeriesCollection SeriesCollection { get; set; }
    }
}


