using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;

namespace Winforms.Gauge._360
{
    public partial class Gauge360Example : Form
    {
        public Gauge360Example()
        {
            InitializeComponent();

            //360 mode enabled
            solidGauge1.Uses360Mode = true;
            solidGauge1.From = 0;
            solidGauge1.To = 100;
            solidGauge1.Value = 50;

            //rotated 90° and has an inverted clockwise fill
            solidGauge2.Uses360Mode = true;
            solidGauge2.From = 0;
            solidGauge2.To = 100;
            solidGauge2.Value = 50;
            solidGauge2.Base.GaugeRenderTransform = new TransformGroup
            {
                Children = new TransformCollection
                {
                    new RotateTransform(90),
                    new ScaleTransform {ScaleX = -1}
                }
            };

            solidGauge3.Uses360Mode = true;
            solidGauge3.From = 0;
            solidGauge3.To = 100;
            solidGauge3.Value = 20;
            solidGauge3.HighFontSize = 60;
            solidGauge3.Base.Foreground = Brushes.White;
            solidGauge3.InnerRadius = 0;
            solidGauge3.GaugeBackground = new SolidColorBrush(Color.FromRgb(71,128,181));

            //the next gauge interpolates from color white, to color black according
            //to the current value in the gauge
            solidGauge4.Uses360Mode = true;
            solidGauge4.From = 0;
            solidGauge4.To = 100;
            solidGauge4.Value = 50;
            solidGauge4.HighFontSize = 60;
            solidGauge4.Base.Foreground = new SolidColorBrush(Color.FromRgb(66,66,66));
            solidGauge4.FromColor = Colors.White;
            solidGauge4.ToColor = Colors.Black;
            solidGauge4.InnerRadius = 0;
            solidGauge4.Base.Background = Brushes.Transparent;

            //standard gauge
            solidGauge5.From = 0;
            solidGauge5.To = 100;
            solidGauge5.Value = 50;

            //custom fill
            solidGauge6.From = 0;
            solidGauge6.To = 100;
            solidGauge6.Value = 50;
            solidGauge6.Base.LabelsVisibility = Visibility.Hidden;
            solidGauge6.Base.GaugeActiveFill = new LinearGradientBrush
            {
                GradientStops = new GradientStopCollection
                {
                    new GradientStop(Colors.Yellow, 0),
                    new GradientStop(Colors.Orange, .5),
                    new GradientStop(Colors.Red, 1)
                }
            };
        }
    }
}
