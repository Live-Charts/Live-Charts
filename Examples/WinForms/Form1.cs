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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cartesianChart1.Series.Add(new LineSeries
            {
                Values = new ChartValues<ObservableValue>
                {
                    new ObservableValue(3),
                    new ObservableValue(5),
                    new ObservableValue(7),
                    new ObservableValue(3),
                    new ObservableValue(1)
                }
            });
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var r = new Random();
            foreach (var observable in cartesianChart1.Series[0].Values.Cast<ObservableValue>())
            {
                observable.Value = r.Next(0, 10);
            }
        }
    }
}
