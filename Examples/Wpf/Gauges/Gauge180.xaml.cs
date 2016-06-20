using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using Wpf.Annotations;

namespace Wpf.Gauges
{
    public partial class Gauge180 : INotifyPropertyChanged
    {
        private double _value;

        public Gauge180()
        {
            InitializeComponent();

            Value = 10;
            Formatter = x => x + " Km/Hr";

            DataContext = this;
        }

        public double Value
        {
            get { return _value; }
            set
            {
                _value = value;
                OnPropertyChanged();
            }
        }

        public Func<double, string> Formatter { get; set; } 

        private void MoveOnClick(object sender, RoutedEventArgs e)
        {
            Value = new Random().Next(0, 100);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
