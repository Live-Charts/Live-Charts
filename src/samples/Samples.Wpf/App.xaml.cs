using System.Windows;
using LiveCharts.Core;
using LiveCharts.Core.Defaults;
using LiveCharts.Core.Themes;
using LiveCharts.Wpf;

namespace Samples.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Charting.Settings(charting =>
            {
                charting
                    .ForPrimitiveAndDefaultTypes()
                    .UsingWpf()
                    .UsingMaterialDesignLightTheme();
            });
        }
    }
}
