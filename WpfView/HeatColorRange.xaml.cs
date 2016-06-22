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

namespace LiveCharts.Wpf
{
    /// <summary>
    /// Interaction logic for HeatColorRange.xaml
    /// </summary>
    public partial class HeatColorRange : UserControl
    {
        public HeatColorRange()
        {
            InitializeComponent();
        }

        public void UpdateFill(CoreColor min, CoreColor max)
        {
            Background = new LinearGradientBrush
            {
                StartPoint = new Point(0, 0),
                EndPoint = new Point(0, 1),
                GradientStops = new GradientStopCollection
                {
                    new GradientStop
                    {
                        Color = new Color
                        {
                            A = min.A,
                            R = min.R,
                            G = min.G,
                            B = min.B
                        },
                        Offset = 0
                    },
                    new GradientStop
                    {
                        Color = new Color
                        {
                            A = max.A,
                            R = max.R,
                            G = max.G,
                            B = max.B
                        },
                        Offset = 1
                    }
                }
            };
        }

        public double SetMax(string value)
        {
            MaxVal.Text = value;
            MaxVal.UpdateLayout();
            return MaxVal.ActualWidth;
        }

        public double SetMin(string value)
        {
            MinVal.Text = value;
            MinVal.UpdateLayout();
            return MinVal.ActualWidth;
        }
    }
}
