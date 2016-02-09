using System;
using System.Windows;

namespace ChartsTest.Pie_Examples
{
    /// <summary>
    /// Interaction logic for BasicBar.xaml
    /// </summary>
    public partial class BasicPie
    {
        public BasicPie()
        {
            InitializeComponent();

            Formatter = val => val.ToString("N1") + " (" + (val/Chart.PieTotalSum).ToString("P1") + ")";

            DataContext = this;
        }

        public Func<double, string> Formatter { get; set; }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            //this is just to see animation everytime you click next
            Chart.Update();
        }
    }
}
