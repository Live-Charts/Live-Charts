using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LiveCharts;

namespace Wpf.CartesianChart.SharedTooltip
{
    /// <summary>
    /// Interaction logic for SharedTooltip.xaml
    /// </summary>
    public partial class SharedTooltipExample : UserControl
    {
        public SharedTooltipExample()
        {
            InitializeComponent();

            LabelPoint = x => "A long label so it overflows " + x;

            DataContext = this;
        }

        public Func<ChartPoint, string> LabelPoint { get; set; }
    }
}
