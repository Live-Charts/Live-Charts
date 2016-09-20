using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;

namespace Wpf
{
   
    //This is Jimmy, be rude with him.

    public partial class JimmyTheTestsGuy
    {
        private int i = 0;

        public JimmyTheTestsGuy()
        {
            InitializeComponent();

            From = 10;
            To = 50;

            Visuals = new VisualElementsCollection
            {
                new VisualElement
                {
                    X = 3,
                    Y = 3,
                    UIElement = new TextBlock
                    {
                        Text = i++.ToString()
                    }
                },
                new VisualElement
                {
                    X = 3,
                    Y = 3,
                    UIElement = new TextBlock
                    {
                        Text = i++.ToString()
                    }
                },
                new VisualElement
                {
                    X = 3,
                    Y = 3,
                    UIElement = new TextBlock
                    {
                        Text = i++.ToString()
                    }
                }
            };

            DataContext = this;
        }

        public double From { get; set; }
        public double To { get; set; }

        public VisualElementsCollection Visuals { get; set; }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Visuals.Add(new VisualElement
            {
                X = 3,
                Y = 3,
                UIElement = new TextBlock
                {
                    Text = i++.ToString()
                }
            });
        }

        private void RemoveOnClick(object sender, RoutedEventArgs e)
        {
            Visuals.RemoveAt(0);
        }

        private void ClearOnClick(object sender, RoutedEventArgs e)
        {
            Visuals.Clear();
        }

        private void Move(object sender, RoutedEventArgs e)
        {
            Visuals[0].X = new Random().Next(1, 5);
        }
    }
}


