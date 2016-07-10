using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Wpf.Gauges
{
    public partial class Gauge360 : UserControl, INotifyPropertyChanged
    {
        private double _value;

        public Gauge360()
        {
            InitializeComponent();

            Value = .85;
            Formatter = x => x.ToString("P");

            DataContext = this;
        }

        public double Value
        {
            get { return _value; }
            set
            {
                _value = value;
                OnPropertyChanged("Value");
            }
        }

        public Func<double, string> Formatter { get; set; } 

        private void MoveOnClick(object sender, RoutedEventArgs e)
        {
            Value = new Random().NextDouble();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
