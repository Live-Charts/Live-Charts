using System.ComponentModel;
using System.Linq;
using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Defaults;
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
            
            var i = new []{1, 2, 3};
            var j = new[] { 1, 2, 3,4,5 };
            var k = new[] { 1, 2, 3,4,5,6,7 };

            var source = new[] {i, j, k};

            var g = source.Select(n =>
            {
                var pieSeries = new PieSeries
                {
                    DataLabels = true,
                    Values = new ChartValues<ObservableValue> {new ObservableValue(n.Length)}
                };
                return pieSeries;
            }).ToList();

            SeriesCollection = new SeriesCollection();

            SeriesCollection.Clear();
            SeriesCollection.AddRange(g);

            DataContext = this;
        }

        public SeriesCollection SeriesCollection { get; set; }
        
    }
}


