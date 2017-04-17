using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using LiveCharts;
using Wpf.Annotations;

namespace Wpf.CartesianChart.ManualZAndP
{
    public partial class ManualZAndPExample : UserControl, INotifyPropertyChanged
    {
        private double _to;
        private double _from;

        public ManualZAndPExample()
        {
            InitializeComponent();

            Values = new ChartValues<double>();

            var r = new Random();
            for (var i = 0; i < 100; i++)
            {
                Values.Add(r.Next(0, 10));
            }

            //In this case we are paginating the data only showing the first 25 records
            //clicking the buttons previous and next changes the page
            From = 0;
            To = 25;

            DataContext = this;
        }

        public ChartValues<double> Values { get; set; }

        public double From
        {
            get { return _from; }
            set
            {
                _from = value;
                OnPropertyChanged("From");
            }
        }

        public double To
        {
            get { return _to; }
            set
            {
                _to = value;
                OnPropertyChanged("To");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        
        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void NextOnClick(object sender, RoutedEventArgs e)
        {
            From += 25;
            To += 25;
        }

        private void PrevOnClick(object sender, RoutedEventArgs e)
        {
            From -= 25;
            To -= 25;
        }

        private void ManualZoom(object sender, RoutedEventArgs e)
        {
            //you only need to change the axis limits to zoom in/out any axis.
            From = 5;
            To = 10;
        }
    }
}
