using System.Windows;
using Assets.Models;
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

        private void MainButtonOnClick(object sender, RoutedEventArgs e)
        {
            var fe = (FrameworkElement) sender;
            var c = (Sample) fe.DataContext;
        }
    }
}
