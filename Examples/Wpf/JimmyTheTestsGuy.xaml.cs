using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
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
using LiveCharts.Wpf;
using Wpf.Annotations;

namespace Wpf
{
   
    //This is Jimmy, be rude with him.

    public partial class JimmyTheTestsGuy : INotifyPropertyChanged
    {
        private SeriesCollection _seriesCollection;

        public JimmyTheTestsGuy()
        {
            InitializeComponent();

            var r = new Random();

            Source = new List<SeriesCollection>
            {
                new SeriesCollection
                {
                    new LineSeries
                    {
                        Values = new ChartValues<double> { r.Next(0,10), r.Next(0, 10), r.Next(0, 10) }
                    },
                    new LineSeries
                    {
                        Values = new ChartValues<double> { r.Next(0,10), r.Next(0, 10), r.Next(0, 10) }
                    },
                    new LineSeries
                    {
                        Values = new ChartValues<double> { r.Next(0,10), r.Next(0, 10), r.Next(0, 10) }
                    }
                },
                new SeriesCollection
                {
                    new LineSeries
                    {
                        Values = new ChartValues<double> { r.Next(0,10), r.Next(0, 10), r.Next(0, 10) }
                    },
                    new LineSeries
                    {
                        Values = new ChartValues<double> { r.Next(0,10), r.Next(0, 10), r.Next(0, 10) }
                    },
                    new LineSeries
                    {
                        Values = new ChartValues<double> { r.Next(0,10), r.Next(0, 10), r.Next(0, 10) }
                    }
                },
                new SeriesCollection
                {
                    new LineSeries
                    {
                        Values = new ChartValues<double> { r.Next(0,10), r.Next(0, 10), r.Next(0, 10) }
                    },
                    new LineSeries
                    {
                        Values = new ChartValues<double> { r.Next(0,10), r.Next(0, 10), r.Next(0, 10) }
                    },
                    new LineSeries
                    {
                        Values = new ChartValues<double> { r.Next(0,10), r.Next(0, 10), r.Next(0, 10) }
                    }
                }
            };

            DataContext = this;
        }

        public List<SeriesCollection> Source { get; set; }


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            foreach (var dahCurrentItem in Control.Items)
            {
                var container = Control.ItemContainerGenerator.ContainerFromItem(dahCurrentItem) as FrameworkElement;
                var chart = Control.ItemTemplate.FindName("Chart", container) as LiveCharts.Wpf.CartesianChart;
                chart.Series = (SeriesCollection) dahCurrentItem;

            }
        }
    }
}
