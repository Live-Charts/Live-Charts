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

            DataContext = this;
        }

        public SeriesCollection SeriesCollection { get; set; }
    }
}


