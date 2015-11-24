using System;
using System.Collections.ObjectModel;
using System.Windows.Forms;
using LiveCharts;

namespace WinForms
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var lineChart = Host.Child as LineChart;
            if (lineChart == null) return;
            lineChart.Zooming = true;
            lineChart.PrimaryAxis.LabelFormatter = d => d.ToString("C");
            lineChart.PrimaryAxis.Labels = new []
            {
                "label1","label2","label3","label4","label5","label6","label7","label8",
                "label9","label10","label11","label12","label13","label14","label15","label16",
                "label17","label18","label19","label20","label21","label22","label23","label24",
            };
            lineChart.Series.Add(new LineSerie
                {
                    PrimaryValues = new ObservableCollection<double>{1,-1, 1,-1, 1,-1, 1,-1, 1,-1, 1,-1, 1,-1, 1,-1}
                });
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var lineChart = Host.Child as LineChart;
            lineChart?.ClearAndPlot();
        }
    }
}
