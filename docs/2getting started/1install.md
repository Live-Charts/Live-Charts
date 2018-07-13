# Install

The easier way is to use Nuget, if you have any question about how to use Nuget, we recommend you [this tutorial](https://www.youtube.com/watch?v=UCEUOvwxKtg).

${installFromNuget}

The package will install in a few seconds.

${if, {platform} == 'wpf'}

Finally configure LiveCharts according to your needs:

${if, {platform} == 'wpf'}

```c#
Charting.Settings(charting =>
{
    charting
        .ForPrimitiveAndDefaultTypes()
        .UsingWpf()
        .UsingMaterialDesignLightTheme();
});
```

If you need more information about the previous block, please see ${link, The settings section, docs/getting started/settings}.

## Working with the source code

If you want to run the source code for contributing or your own debugging session purposes, follow the next steps:

1. Clone Live-Charts repository. `git clone <https://github.com/Live-Charts/Live-Charts.git>}`.
1. Go to the `src` folder, there you will find LiveChart.Core, LiveCharts.WPF, LiveCharts.UWP and LiveCharts.Xamarin at least, in the project you want to use the source code of LiveCharts, add the reference to LiveCharts.Core and the version of LiveCharts you need according to your target framework (WPF, UWP or Xamarin).