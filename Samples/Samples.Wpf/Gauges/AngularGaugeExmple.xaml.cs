using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace Wpf.Gauges
{

    public partial class AngularGaugeExmple : UserControl, INotifyPropertyChanged
    {
        private double _value;

        public AngularGaugeExmple()
        {
            InitializeComponent();

            Value = 160;

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

        private void ChangeValueOnClick(object sender, RoutedEventArgs e)
        {
            Value = new Random().Next(50, 250);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
