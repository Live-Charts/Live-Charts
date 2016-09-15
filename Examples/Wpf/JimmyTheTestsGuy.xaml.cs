using System.Linq;
using System.Windows;
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

            From = 10;
            To = 50;

            DataContext = this;
        }

        public double From { get; set; }
        public double To { get; set; }

        public SeriesCollection SeriesCollection { get; set; }
    }
}


