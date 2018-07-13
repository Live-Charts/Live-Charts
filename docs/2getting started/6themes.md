# Themes

If you want to override the default value of a property globally this is the way to achieve it, this is how the default themes are built, lets take a look at some relevant parts in the Material Design theme, the source code can be found at `src/LiveCharts.Core/Themes/MaterialDesign.cs`, you can use the next block as a skeleton to create your own themes.

```c#
charting
    // overrides the default colors array
    .HasColors(new[]
    {
        System.Drawing.Color.FromArgb(255, 8, 98, 185),
        System.Drawing.Color.FromArgb(255, 219, 55, 52),
    })

    // every time we initialize a Series, we will set the
    // stroke thickness property to 3.
    .SetDefault<ISeries>(series =>
    {
        series.StrokeThickness = 3f;
    })

    // every time we initialize a line series we will use
    // the previous defined settings for ISeries and
    // we will also set the geometry property to
    // a circle geometry by default, this geometry is
    // used to represent a point in the series.
    .SetDefault<ILineSeries>(lineSeries =>
    {
        lineSeries.Geometry = Geometry.Circle;
    })

    .SetDefault<IBarSeries>(barSeries =>
    {
        // override bar series defaults here.
    })

    SetDefault<IScatterSeries>(scatterSeries =>
    {
        // override scatter series defaults here.
    })

    // finally for the Axis class we will set the separators styles.
    .SetDefault<Axis>(axis =>
    {
        axis.XSeparatorStyle = SeparatorStyle.Empty;
        axis.YSeparatorStyle = new SeparatorStyle(
                                    Color.FromArgb(0, 0, 0, 0),
                                    Color.FromArgb(255, 245, 245, 245),
                                    0);
        axis.XAlternativeSeparatorStyle = SeparatorStyle.Empty;
        axis.YAlternativeSeparatorStyle = SeparatorStyle.Empty;
    });
```

Notice we used an interface to override the series properties, you normally pass the class type you want to override, Series are the only exception were you need to use the interface they implement, they are named the same as the Series class, with the `I` prefix.