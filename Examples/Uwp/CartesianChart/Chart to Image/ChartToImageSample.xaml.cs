using System.IO;
using Windows.Foundation;
using Windows.Graphics.Imaging;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using LiveCharts;
using LiveCharts.Uwp;

namespace Wpf.CartesianChart.Chart_to_Image
{
    /// <summary>
    /// Interaction logic for ChartToImageSample.xaml
    /// </summary>
    public partial class ChartToImageSample : UserControl
    {
        public ChartToImageSample()
        {
            InitializeComponent();
        }

        private void BuildPngOnClick(object sender, RoutedEventArgs e)
        {
            var myChart = new LiveCharts.Uwp.CartesianChart
            {
                DisableAnimations = true,
                Width = 600,
                Height = 200,
                Series = new SeriesCollection
                {
                    new LineSeries
                    {
                        Values = new ChartValues<double> {1, 6, 7, 2, 9, 3, 6, 5}
                    }
                }
            };

            var viewbox = new Viewbox();
            viewbox.Child = myChart;
            viewbox.Measure(myChart.RenderSize);
            viewbox.Arrange(new Rect(new Point(0, 0), myChart.RenderSize));
            myChart.Update(true, true); //force chart redraw
            viewbox.UpdateLayout();

            //SaveToPng(myChart, "chart.png");
            //png file was created at the root directory.
        }

        //private void SaveToPng(FrameworkElement visual, string fileName)
        //{
        //    var encoder = new PngBitmapEncoder();
        //    EncodeVisual(visual, fileName, encoder);
        //}

        //private static void EncodeVisual(FrameworkElement visual, string fileName, BitmapEncoder encoder)
        //{
        //    var bitmap = new RenderTargetBitmap((int)visual.ActualWidth, (int)visual.ActualHeight, 96, 96, PixelFormats.Pbgra32);
        //    bitmap.Render(visual);
        //    var frame = BitmapFrame.Create(bitmap);
        //    encoder.Frames.Add(frame);
        //    using (var stream = File.Create(fileName)) encoder.Save(stream);
        //}
    }
}
