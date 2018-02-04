using System.Windows;
using LiveCharts.Core;
using LiveCharts.Core.DefaultSettings.Themes;
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
            LiveChartsSettings.Set(
                settings =>
                    settings.UseWpf()
                        .UseMaterialDesignLightTheme());
        }
    }
}
