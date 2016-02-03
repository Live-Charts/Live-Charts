using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using LiveCharts.CoreComponents;

namespace ChartsTest.Line_Examples.UiElements
{
    /// <summary>
    /// Interaction logic for UiElementsLine.xaml
    /// </summary>
    public partial class UiElementsLine
    {
        private readonly Image _sun = new Image
        {
            Source = new BitmapImage(new Uri("pack://application:,,,/ChartsTest;component/sun.png")),
            Height = 40,
            Width = 40
        };
        private readonly Image _snow = new Image
        {
            Source = new BitmapImage(new Uri("pack://application:,,,/ChartsTest;component/snow.png")),
            Height = 40,
            Width = 40
        };
        private readonly TextBlock _note = new TextBlock
        {
            Text = "This is a test note, you can place any UIElement in a chart, " +
                   "and use the Plot event to move them to a certain location",
            MaxWidth = 120,
            TextWrapping = TextWrapping.Wrap
        };

        public UiElementsLine()
        {
            InitializeComponent();
            Chart.Canvas.Children.Add(_sun);
            Chart.Canvas.Children.Add(_snow);
            Chart.Canvas.Children.Add(_note);
            TemperatureAxis.LabelFormatter = x => x + "°";
            Panel.SetZIndex(_note, int.MaxValue);
            Panel.SetZIndex(_sun, int.MaxValue);
            Panel.SetZIndex(_snow, int.MaxValue);
        }

        private void Chart_OnPlot(Chart chart)
        {
            var sunPoint = chart.ToPlotArea(new Point(4, 35));
            var snowPoint = chart.ToPlotArea(new Point(8, -3));
            var notePoint = chart.ToPlotArea(new Point(1, 35));

            Canvas.SetLeft(_sun, sunPoint.X - 20);
            Canvas.SetLeft(_snow, snowPoint.X - 20);
            Canvas.SetLeft(_note, notePoint.X);

            Canvas.SetTop(_sun, sunPoint.Y - 20);
            Canvas.SetTop(_snow, snowPoint.Y - 20);
            Canvas.SetTop(_note, notePoint.Y);
        }

        private void UiElementsLine_OnLoaded(object sender, RoutedEventArgs e)
        {
            //this is only to force animation everytime you change view.
            Chart.ClearAndPlot();
        }
    }
}
