using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Wpf.CartesianChart.SectionsDragable
{
    /// <summary>
    /// Interaction logic for DragableSections.xaml
    /// </summary>
    public partial class DragableSections : UserControl, INotifyPropertyChanged
    {
        private double _xSection;
        private double _ySection;

        public DragableSections()
        {
            InitializeComponent();

            XSection = 5;
            YSection = 5;

            DataContext = this;
        }

        public double XSection
        {
            get { return _xSection; }
            set
            {
                _xSection = value;
                OnPropertyChanged("XSection");
            }
        }

        public double YSection
        {
            get { return _ySection; }
            set
            {
                _ySection = value;
                OnPropertyChanged("YSection");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
