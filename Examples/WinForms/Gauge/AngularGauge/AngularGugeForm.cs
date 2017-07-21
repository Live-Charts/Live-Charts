using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;
using LiveCharts.Wpf;
using Brushes = System.Windows.Media.Brushes;

namespace Winforms.Gauge.AngularGauge
{
    public partial class AngularGugeForm : Form
    {
        public AngularGugeForm()
        {
            InitializeComponent();

            angularGauge1.Value = 160;
            angularGauge1.FromValue = 50;
            angularGauge1.ToValue = 250;
            angularGauge1.TicksForeground = Brushes.White;
            angularGauge1.Base.Foreground = Brushes.White;
            angularGauge1.Base.FontWeight = FontWeights.Bold;
            angularGauge1.Base.FontSize = 16;
            angularGauge1.SectionsInnerRadius = 0.5;

            angularGauge1.Sections.Add(new AngularSection
            {
                FromValue = 50, 
                ToValue = 200,
                Fill = new SolidColorBrush(Color.FromRgb(247,166,37))
            });
            angularGauge1.Sections.Add(new AngularSection
            {
                FromValue = 200,
                ToValue = 250,
                Fill = new SolidColorBrush(Color.FromRgb(254, 57, 57))
            });
        }
    }
}
