using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Helpers;
using LiveCharts.Wpf;
using Separator = LiveCharts.Wpf.Separator;

namespace Winforms.Cartesian.Linq
{
    public partial class LinqExample : Form
    {
        public LinqExample()
        {
            InitializeComponent();
            
            //lets configure the chart to plot cities
            var mapper = Mappers.Xy<City>()
                .X((city, index) => index)
                .Y(city => city.Population);

            //lets take the first 15 records by default;
            var records = DataBase.Cities.OrderByDescending(x => x.Population).Take(15).ToArray();

            Results = records.AsChartValues();
            Labels = records.Select(x => x.Name).ToList();

            cartesianChart1.Series = new SeriesCollection
            {
                new ColumnSeries
                {
                    Configuration = mapper,
                    Values = Results
                }
            };

            cartesianChart1.AxisY.Add(new Axis
            {
                LabelFormatter = value => (value/1000000).ToString("N") + "M"
            });
            cartesianChart1.AxisX.Add(new Axis
            {
                Labels = Labels,
                DisableAnimations = true,
                LabelsRotation = 20,
                Separator = new Separator
                {
                    Step = 1
                }
            });
        }

        public ChartValues<City> Results { get; set; }
        public List<string> Labels { get; set; }

        private void textBox1_TextChanged(object sender, System.EventArgs e)
        {
            var q = ((TextBox) sender).Text ?? string.Empty;
            q = q.ToUpper();

            var records = DataBase.Cities
                .Where(x => x.Name.ToUpper().Contains(q) || x.Country.ToUpper().Contains(q))
                .OrderByDescending(x => x.Population)
                .Take(15)
                .ToArray();

            Results.Clear();
            Results.AddRange(records);

            Labels.Clear();
            foreach (var record in records) Labels.Add(record.Name);

        }
    }
}
