using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;

namespace Winforms.Cartesian.BasicBubbles
{
    public partial class BasicBubblesExample : Form
    {
        public BasicBubblesExample()
        {
            InitializeComponent();

            cartesianChart1.Series = new SeriesCollection
            {
                new BubbleSeries
                {
                    Values = new ChartValues<BubblePoint>
                    {
                        //X  Y   W
                        new BubblePoint(5, 5, 20),
                        new BubblePoint(3, 4, 80),
                        new BubblePoint(7, 2, 20),
                        new BubblePoint(2, 6, 60),
                        new BubblePoint(8, 2, 70)
                    }
                },
                new BubbleSeries
                {
                    Values = new ChartValues<BubblePoint>
                    {
                        new BubblePoint(7, 5, 1),
                        new BubblePoint(2, 2, 1),
                        new BubblePoint(1, 1, 1),
                        new BubblePoint(6, 3, 1),
                        new BubblePoint(8, 8, 1)
                    }
                }
            };

        }
    }
}
