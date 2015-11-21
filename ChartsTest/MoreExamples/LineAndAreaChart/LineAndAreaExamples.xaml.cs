using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace ChartsTest.MoreExamples.LineAndAreaChart
{
    /// <summary>
    /// Interaction logic for LineAndAreaExamples.xaml
    /// </summary>
    public partial class LineAndAreaExamples
    {
        private List<UserControl> _examples = new List<UserControl>
        {
            new Example1(), new Example2()
        };

        private int _current = 0;

        public LineAndAreaExamples()
        {
            InitializeComponent();
        }

        private void Next(object sender, RoutedEventArgs e)
        {
            _current++;
            if (_current > _examples.Count - 1) _current = 0;
            ContentControl.Content = _examples[_current];
        }

        private void Previous(object sender, RoutedEventArgs e)
        {
            _current--;
            if (_current < 0) _current = _examples.Count - 1;
            ContentControl.Content = _examples[_current];
        }

        private void LineAndAreaExamples_OnLoaded(object sender, RoutedEventArgs e)
        {
            _current = 0;
            ContentControl.Content = _examples[0];
        }
    }
}
