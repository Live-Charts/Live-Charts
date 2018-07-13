# Settings

LiveCharts provides the `Charting.Settings()` method, it is inspired in the way you configure a dotnet core web application, the idea behind this method is that you can share configure LiveCharts for all the platforms you are using, WPF, UWP and Xamarin using the same code, with this approach for example you can write a theme, or you can teach LiveChars how to plot a custom class and the interesting part is that for both cases the same code can be reused within WPF, UWP or Xamarin.

The settings method overrides the default settings of the library, you normally need to configure the library only when your application starts,  to achieve this, there are good practices according to each platform, in the `docs/getting started/install` article we describe where we recommend you to configure LiveCharts for each platform.

The library already provides many useful settings, like themes, UserInterface providers, or teaching the library how to plot a custom type.

Take a look at the following block, this is really all you need to plot in WPF using the already defined material design theme.

``` c#
Charting.Settings(charting =>
{
    charting
        .LearnPrimitiveAndDefaultTypes()
        .SetTheme(Themes.MaterialDesign)
        .TargetsWpf();
});
```

When we call `Charting.Settings()` is requires a parameter of type `Action<Charting>` where the Charting class is our current settings, here we can override anything we need, lets take a deeper look.

When we call `.LearnPrimitiveAndDefaultTypes()` we are teaching the library how to plot all primitive types (explained at `docs/getting started/custom types`).

Then `.SetTheme(Themes.MaterialDesign)` is overriding the defaults of some properties for our series, axis, separator and label classes this way every time we initialize any of this instances in our application they will already have the properties we require by default, in this case we are giving them a Material Design look, the `SetTheme()` method requires the enum `Themes`, intellisense will make its work and will let you know all the themes we have already built for you.

Finally `TargetsWpf()` lets the core know how to draw all the shapes, in this case we will use the WPF framework to render our charts.

After you call these 3 lines, you can follow any sample in this tutorial, more is explained about settings in the articles `docs/getting started/themes` and `docs/getting started/custom types`.