using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
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
            LiveChartsSettings.Define(
                settings =>
                    settings.UseWpf()
                        .UseMaterialDesignLightTheme());
        }
    }
}
