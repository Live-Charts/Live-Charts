using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;
using LiveCharts.Core.Data;
using LiveCharts.Wpf.Interaction;

namespace Samples.Wpf.Views
{
    /// <summary>
    /// Interaction logic for PropertyAndInstanceChanged.xaml
    /// </summary>
    public partial class PropertyAndInstanceChanged : UserControl
    {
        private static int i;

        public PropertyAndInstanceChanged()
        {
            InitializeComponent();
        }

        private void GridOnMouseUp(object sender, MouseButtonEventArgs e)
        {
            Console.WriteLine("Grid Mouse Up");
        }

        private void Chart_OnDataClick(object sender, DataInteractionEventArgs args)
        {
            args.Handled = true;
            foreach (var p in args.Points)
            {
                Console.WriteLine(p.Coordinate);
            }
        }
    }
}
