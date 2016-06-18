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

            SeriesCollection = GetSeries();

            Task.Run(() =>
            {
                while (true)
                {
                    Thread.Sleep(3000);
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        SeriesCollection = GetSeries();
                    });
                }
            });

            DataContext = this;
        }

        public SeriesCollection SeriesCollection
        {
            get { return _seriesCollection; }
            set
            {
                _seriesCollection = value;
                OnPropertyChanged("SeriesCollection");
            }
        }


        private SeriesCollection GetSeries()
        {
            return new SeriesCollection
            {
                new LineSeries
                {
                    Values = new ChartValues<double> {1, 4, 7, 2, 6}
                },
                new LineSeries
                {
                    Values = new ChartValues<double> {6, 2, 8, 3, 6}
                },
                new LineSeries
                {
                    Values = new ChartValues<double> {9, 3, 6, 3, 6}
                }
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
