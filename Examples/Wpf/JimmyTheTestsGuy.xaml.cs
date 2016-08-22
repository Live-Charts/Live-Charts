using System.ComponentModel;
using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Wpf;
using Wpf.Annotations;

namespace Wpf
{
   
    //This is Jimmy, be rude with him.

    public partial class JimmyTheTestsGuy 
    {
        public JimmyTheTestsGuy()
        {
            InitializeComponent();

            Charting.For<TestVm>(Mappers.Xy<TestVm>().X(m => m.X).Y(m => m.Y));

            SeriesCollection = new SeriesCollection
            {
                new StackedColumnSeries
                {
                    Values = new ChartValues<TestVm>
                    {
                        new TestVm { X = 0, Y = 1},
                    }
                },
                new StackedColumnSeries
                {
                    Values = new ChartValues<TestVm>
                    {
                        new TestVm { X = 1, Y = 2},
                    }
                }
            };

            DataContext = this;
        }

        public SeriesCollection SeriesCollection { get; set; }
        

    }

    public class TestVm
    {
        public double X { get; set; }
        public double Y { get; set; }
    }

}


