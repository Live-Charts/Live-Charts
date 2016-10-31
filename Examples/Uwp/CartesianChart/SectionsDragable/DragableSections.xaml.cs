using LiveCharts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace UWP.CartesianChart.SectionsDragable
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DragableSections : Page, INotifyPropertyChanged
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

        public IChartValues ChartValues { get; set; } = new ChartValues<int>(new int[] { 7, 2, 8, 2, 7, 4, 9, 4, 2, 8 });

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
