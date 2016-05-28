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

namespace Winforms
{
    public partial class Labels : Form
    {
        public Labels()
        {
            InitializeComponent();
        }

        private void Labels_Load(object sender, EventArgs e)
        {
            cartesianChart1.Series.Add(new ColumnSeries
            {
                Values = new ChartValues<ObservableValue>
                {
                    new ObservableValue(4),
                    new ObservableValue(2),
                    new ObservableValue(8),
                    new ObservableValue(2),
                    new ObservableValue(3),
                    new ObservableValue(0),
                    new ObservableValue(1),
                },
                DataLabels = true,
                LabelPoint = point => point.Y + "K"
            });

            cartesianChart1.AxisX.Add(new Axis
            {
                Labels = new[]
                {
                    "Shea Ferriera",
                    "Maurita Powel",
                    "Scottie Brogdon",
                    "Teresa Kerman",
                    "Nell Venuti",
                    "Anibal Brothers",
                    "Anderson Dillman"
                },
                Separator = DefaultAxes.CleanSeparator,
                //ToDo Add the rotate tranform...
            });

            cartesianChart1.AxisY.Add(new Axis
            {
                LabelFormatter = value => value + ".00K items"
            });

        }
    }
}
