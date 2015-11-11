
<p align="center">
  <img src="https://dl.dropboxusercontent.com/u/40165535/live.png" />
</p>
<p align="center">
  <img src="https://dl.dropboxusercontent.com/u/40165535/live1.gif" />
  <img src="https://dl.dropboxusercontent.com/u/40165535/live2.gif" />
  <img src="https://dl.dropboxusercontent.com/u/40165535/live3.gif" />
  <img src="https://dl.dropboxusercontent.com/u/40165535/live4.gif" />
  <img src="https://dl.dropboxusercontent.com/u/40165535/live5.gif" />
  <img src="https://dl.dropboxusercontent.com/u/40165535/live6.gif" />
</p>

**What this library is**
 - Good looking, animated and easy to customize charts, you can practically change all properties.
 - Easy to maintain and create new charts, as you can see in the source code, some charts have almost no code.
 - Ready for WPF Bindings.
 - Supports zooming and panning.
 - MIT License, permissive licensing.

**What this is not**
 - A high performance library (by now), the first target of this library is to make good looking charts. That does not means performance will not be improved.

I decided to start this because current open source alternatives were not exactly what I needed. The best options I could find were:

   - **[Oxy plot](http://oxyplot.org/)**: It's really good, open source, has a nice documentation but its hard to customize, I downloaded the library and the first thing I wanted to change was Axis color and I after some hours of trying I couldn't, another good point is that is support Silverlight, WP8, Andorid, IOS, Mac, but at the same time that makes Oxyplot really hard to mantain. We deserve a specialized library for .net
   - **[Modern UI Charts](https://modernuicharts.codeplex.com/)** It was almost what I was looking for except for some details, it does not have simple line charts and licensing is not so permissive,  I’m not a lawyer but it does not seems to friendly for commercial use.
   - Now if you Google it you will surely arrive to [this Stackoverflow question](http://stackoverflow.com/questions/577278/wpf-chart-controls), but that question is closed and the newest answer was at May 2013. So most of answers are out of date.
   - The last option pay for a charting library. Really? Is this that an option? How can someone charge you any amount of money for some thousands of lines, we should build our own.

#Installation

 1. Install package from [**Nuget**](https://www.nuget.org/packages/LiveCharts) `Install-Package LiveCharts`
 2. Add name space to your `Window` `XAML` `xmlns:charts="clr-namespace:LiveCharts.Charts;assembly=LiveCharts"`
 3. Thats it. You are ready.
 
**NOTE:** Since this library is in a beta version, everything could not be documented. So we highly recommend to clone this repo and see included examples, there are a lot trying to cover all the cases. Images included in this documentation might not be updated to the last version.

#Examples
It is recommended to clone this repo on your desktop, you only have to scroll up and click `Open in Visual Studio` button to the right, then see examples included, but here is a resume.

**XAML**
```xml
<charts:LineChart Height="300" x:Name="LineChart" 
                                      Background="#FBFBFB" BorderBrush="LightGray" BorderThickness="1"
                                      Zooming="True"></charts:LineChart>
<charts:BarChart Height="300" x:Name="BarChart" Margin="0 10"
                                 Background="#FBFBFB" BorderBrush="LightGray" 
                                 BorderThickness="1"></charts:BarChart>
<charts:PieChart Height="300" Name="PieChart" Margin="10"
                                 Background="#FBFBFB" BorderBrush="LightGray" 
                                 BorderThickness="1" InnerRadius="50"></charts:PieChart>
<charts:ScatterChart Height="300" Name="ScatterChart" Margin="10"
                                 Background="#FBFBFB" BorderBrush="LightGray" 
                                 BorderThickness="1" LineType="Bezier"></charts:ScatterChart>
```
**C#**
```c#
            LineChart.PrimaryAxis.LabelFormatter = x => x.ToString("C");
            LineChart.SecondaryAxis.Labels = standardLabels;
            LineChart.Series = new ObservableCollection<Serie>
            {
                new LineSerie
                {
                    PrimaryValues = new ObservableCollection<double>
                    {
                        -10, 5, 9, 28, -3, 2, 0, 5, 10, 1, 7, 2
                    }
                }
            };

            BarChart.SecondaryAxis.Labels = standardLabels;
            BarChart.Series = new ObservableCollection<Serie>
            {
                new BarSerie
                {
                    PrimaryValues = new ObservableCollection<double> { 1,2,3,4 }
                },
                new BarSerie
                {
                    PrimaryValues = new ObservableCollection<double> { 4,3,2,1 }
                },
                new BarSerie
                {
                    PrimaryValues = new ObservableCollection<double> { 3,1,4,2 }
                }
            };

            PieChart.Series = new ObservableCollection<Serie>
            {
                //if you add more than one serie to pie chart, they will be overridden
                new PieSerie
                {
                    PrimaryValues = new ObservableCollection<double> { 8,2,5 },
                    Labels = standardLabels
                }
            };

            //func for series, just for an example
            Func<double, double> fx1 = x => Math.Pow(x, 2) + 10*x;
            Func<double, double> fx2 = x => Math.Pow(x, 2);
            ScatterChart.Series = new ObservableCollection<Serie>
            {
                new ScatterSerie
                {
                    PrimaryValues = new ObservableCollection<double> {fx1(-10), fx1(-3), fx1(5), fx1(7)},
                    SecondaryValues =  new double[] {-10, -3, 5, 7},
                    PointRadius = 7
                },
                new ScatterSerie
                {
                    PrimaryValues = new ObservableCollection<double> {fx2(-5), fx2(-2), fx2(3), fx2(10)},
                    SecondaryValues =  new double[]{-5, -2, 3, 10},
                    PointRadius = 7
                },
            };
```

#Included Charts
Chart | Example
------------- | -------------
|**Lines and Area:** single axis scale chart, useful to show trend or compare data, joins points with a bezier or polyline.  | <img align="right" src="https://dl.dropboxusercontent.com/u/40165535/live1.gif" /> |
|**Bar:** single axis scale chart, useful to show trend or compare data, draws bars according to primary axis value.|<img align="right" src="https://dl.dropboxusercontent.com/u/40165535/live3.gif" />|
|**StackedBar** single axis scale chart, useful to show trend or compare data and relational proportions between data.|<img align="right" src="https://dl.dropboxusercontent.com/u/40165535/live6.gif" />|
|**Pie and doughnut:** useful to display relational proportion between data.|<img align="right" src="https://dl.dropboxusercontent.com/u/40165535/live2.gif" />|
|**Scatter:** a chart with scale on both axis, useful when you need to display math functions or simply need scale on both axis.|<img align="right" src="https://dl.dropboxusercontent.com/u/40165535/live4.gif" />|
|**Radar** usefull to show multiple data points and the variation between them.|<img align="right" src="https://dl.dropboxusercontent.com/u/40165535/live5.gif" />|

*Single axis scale chart:* it means that this chart is designed to compare values just in one axis, for example if John sales 3 items, Mark 8 and Susan 3, in you chart, sales should be displayed on primary axis while secondary axis labels should contain names (John, Mark, Susan). Records on Secondary axis will have the same distance between them. 

#Questions 
Try [Stackoverflow](http://stackoverflow.com/). if you are not getting an aswer then try reporting a [new issue](https://github.com/beto-rodriguez/Live-Charts/issues)

#Formatters

Formatters are your functions that takes a double value as parameter and returns a string. for example if you need to display your values as currency you would need to:
  
```
private string MyFormat(double value)
{
  return value.ToString("C");
}

public SomeWhereInYourCode()
{
  LineChart.PrimaryAxis.LabelFormatter = MyFormat;
  //or use a lambda expression
  LineChart.PrimaryAxis.LabelFormatter = value => value.ToString("C");
}
```

This library also includes some common label formatters, you don't normally need to display big labels because they will take a lot of space in the chart, so if you set your chart with the preloaded number label formatter, like this:
```
MyChart.PrimaryAxis.LabelFormatter = LabelFormatters.Number;
MyChart.PrimaryAxis.LabelFormatter = LabelFormatters.Currency; //or this to get same behavior but as currency.
```
will make `10` look like `10.0`, or `1000` like `1.00K`, or `1000000` like `1.00M`

<hr/>
#[Chart](https://github.com/beto-rodriguez/Live-Charts/blob/master/Charts/Charts/Chart.cs)
All charts inherits from `Chart`, this is the core of this library, change properties to customize your charts.
<h3>Properties</h3>
| Name  | Description | Type |
| ------------- | ------------- | ------------- |
| Zooming  | Gets or sets whether graph can zoom  | `bool` |
| Hoverable  | Gets or sets whether graph will display tooltip on hover  | `bool` | 
| PointHoverColor | Gets or sets color when a point is hovered | [`Color`](https://msdn.microsoft.com/en-us/library/system.windows.media.colors(v=vs.110).aspx) |
| PrimaryAxis | Gets or sets primary axis | `Axis` |
| SecondaryAxis | Gets or sets secondary axis | `Axis` |
| DisableAnimation | Gets or sets whether animation is disabled or not | `bool` |
| Series | Gets or sets series to draw | `IEnumerable<Serie>`  |
| TooltipBackground | Gets or sets tooltip backgorund | [`Brush`](https://msdn.microsoft.com/en-us/library/system.windows.media.brush(v=vs.110).aspx)  |
| TooltipForeground | Gets or sets tooltip foreground | [`Brush`](https://msdn.microsoft.com/en-us/library/system.windows.media.brush(v=vs.110).aspx)  |
| TooltipBorderBrush | Gets or sets tooltip borderbrush | [`Brush`](https://msdn.microsoft.com/en-us/library/system.windows.media.brush(v=vs.110).aspx)  |
| TooltipCornerRadius | Gets or sets tooltip corner radius | [`CornerRadius`](https://msdn.microsoft.com/en-us/library/system.windows.cornerradius(v=vs.110).aspx)  |
| TooltipBorderThickness | Gets or sets tooltip border thickness | [`Thickness`](https://msdn.microsoft.com/en-us/library/system.windows.thickness(v=vs.110).aspx)  |

<h3>Methods</h3>
| Name  | Description | Returned Type |
| ------------- | ------------- | ------------- |
| ClearAndPlot  | Redraw chart  | `void` |
| ZoomIn  | Zooms a unit in  | `void` |
| ZoomOut  | Zooms a unit out  | `void` |

<hr/>
#[BarChart](https://github.com/beto-rodriguez/Live-Charts/blob/master/Charts/Charts/BarChart.cs)
Inherits from [`Chart`](https://github.com/beto-rodriguez/Live-Charts#chart)
<h3>Properties</h3>
| Name  | Description | Type |
| ------------- | ------------- | ------------- |
| MaxColumnWidth | Gets or sets max columns width, default is 60 | `double` |
<h3>Methods</h3>
None extra methods
<hr/>
#[StackedBarChart](https://github.com/beto-rodriguez/Live-Charts/blob/master/Charts/Charts/StackedBarChart.cs)
Inherits from [`Chart`](https://github.com/beto-rodriguez/Live-Charts#chart)
<h3>Properties</h3>
| Name  | Description | Type |
| ------------- | ------------- | ------------- |
| MaxColumnWidth | Gets or sets max columns width, default is 60 | `double` |
<h3>Methods</h3>
None extra methods
<hr/>
#[LineChart](https://github.com/beto-rodriguez/Live-Charts/blob/master/Charts/Charts/LineChart.cs)
Inherits from [`Chart`](https://github.com/beto-rodriguez/Live-Charts#chart)
<h3>Properties</h3>
| Name  | Description | Type |
| ------------- | ------------- | ------------- |
| IncludeArea | Gets or sets whether series should draw its area | `bool` |
| LineType | Gets or sets line type to draw | [`LineChartLineType`](https://github.com/beto-rodriguez/Live-Charts/blob/master/Charts/LineChartLineType.cs) |
<h3>Methods</h3>
None extra methods
<hr/>
#[PieChart](https://github.com/beto-rodriguez/Live-Charts/blob/master/Charts/Charts/PieChart.cs)
Inherits from [`Chart`](https://github.com/beto-rodriguez/Live-Charts#chart)
<h3>Properties</h3>
| Name  | Description | Type |
| ------------- | ------------- | ------------- |
| InnerRadius | Gets or sets chart inner radius | `double` |
| SlicePadding | Gets or sets slice padding | `double` |
<h3>Methods</h3>
None extra methods
<hr/>
#[ScatterChart](https://github.com/beto-rodriguez/Live-Charts/blob/master/Charts/Charts/ScatterChart.cs)
Inherits from [`Chart`](https://github.com/beto-rodriguez/Live-Charts#chart)
<h3>Properties</h3>
| Name  | Description | Type |
| ------------- | ------------- | ------------- |
| LineType | Gets or sets line type to draw | [`LineChartLineType`](https://github.com/beto-rodriguez/Live-Charts/blob/master/Charts/LineChartLineType.cs) |
<hr/>
#[RadarChart](https://github.com/beto-rodriguez/Live-Charts/blob/master/Charts/Charts/RadarChart.cs)
Inherits from [`Chart`](https://github.com/beto-rodriguez/Live-Charts#chart)
<h3>Properties</h3>
| Name  | Description | Type |
| ------------- | ------------- | ------------- |
| InnerRadius | Gets or sets inner radius, this is the distance between center and min value, default is 10 | `double` |
<h3>Methods</h3>
None extra methods
<hr/>
#[Serie](https://github.com/beto-rodriguez/Live-Charts/blob/master/Charts/Series/Serie.cs)
A serie is an abstract class to define chart series.
<h3>Properties</h3>
| Name  | Description | Type |
| ------------- | ------------- | ------------- |
| PrimaryValues | Gets or sets primary values collection, chart observe for changes at primary values, they will be auto updated when they change | `ObservableCollection<double>` |
| ColorId | Gets or sets color id, it is not necesary to set this property | `int` |
| StrokeThickness | Gets or sets stroke thickness to use  | `dobule` |
| PointRadius | Gets or sets point radius | `dobule` | 
<hr/>
#[BarSerie](https://github.com/beto-rodriguez/Live-Charts/blob/master/Charts/Series/BarSerie.cs)
Inherits from Serie, defines how a bar serie should be drawn
<hr/>
#[StackedBarSerie](https://github.com/beto-rodriguez/Live-Charts/blob/master/Charts/Series/StackedBarSerie.cs)
Inherits from Serie, defines how a bar serie should be drawn
<hr/>
#[LineSerie](https://github.com/beto-rodriguez/Live-Charts/blob/master/Charts/Series/LineSerie.cs)
Inherits from Serie, defines how a line serie should be drawn
<hr/>
#[PieSerie](https://github.com/beto-rodriguez/Live-Charts/blob/master/Charts/Series/PieSerie.cs)
Inherits from Serie, defines how a pie serie should be drawn
<h3>Properties</h3>
| Name  | Description | Type |
| ------------- | ------------- | ------------- |
| Labels | Gets or sets corresponding labels for serie | `string[]` |
<hr/>
#[RadarSerie](https://github.com/beto-rodriguez/Live-Charts/blob/master/Charts/Series/RadarSerie.cs)
Inherits from Serie, defines how a radar serie should be drawn
<hr/>
#[ScatterSerie](https://github.com/beto-rodriguez/Live-Charts/blob/master/Charts/Series/ScatterSerie.cs)
Inherits from Serie, defines how a scatter serie should be drawn
<h3>Properties</h3>
| Name  | Description | Type |
| ------------- | ------------- | ------------- |
| SecondaryValues | Gets or sets values for secondary axis | `double[]` |
<hr/>
#[Axis](https://github.com/beto-rodriguez/Live-Charts/blob/master/Charts/Axis.cs)
helper class to define axis behavior and style
<h3>Properties</h3>
| Name  | Description | Type |
| ------------- | ------------- | ------------- |
| Separator | Gets or sets separator | `Separator` |
| LabelFormatter | Gets or sets a label formatter, a formatter is useful when you need charts to display a custom format, for example diplay labels as currency, see for example below | `Func<double, string>` |
| Labels | Gets or sets labels to use for the axis, labels is what is shown over the axis, if this property is set `LabelFormatter` will be ignored | `IEnumerable<string>` |
| MaxValue | Indicates max value to plot, if null this will be calculated automatically based on values of series, default is null | `double?` |
| MinValue | Indicates min value to plot, if null this will be calculated automatically based on values of series, default is null| `double?` |
| Enabled | Indicates whether to draw axis or not. | `bool` |
| Color | Gets or sets axis color |  [`Color`](https://msdn.microsoft.com/en-us/library/system.windows.media.colors(v=vs.110).aspx) |
| Thickness | Gets or sets axis thickness | `int` |
| FontFamily | Gets or sets axis FontFamily | [`FontFamily`](https://msdn.microsoft.com/es-es/library/system.windows.media.fontfamily(v=vs.110).aspx) |
| FontSize | Gets or sets axis font size | `int` |
| FontWeight | Gets or sets axis font weight | [`FontWeight`](https://msdn.microsoft.com/en-us/library/system.windows.controls.textblock.fontweight(v=vs.110).aspx ) |
| FontStretch | Gets or sets axis font stretch  | [`FontStretch`](https://msdn.microsoft.com/en-us/library/system.windows.fontstretch(v=vs.110).aspx) |
| PrintLabels | Indicates whether or not print axis labels | `bool` | 
| TextColor | Gets or sets axis text color | [`Color`](https://msdn.microsoft.com/en-us/library/system.windows.media.colors(v=vs.110).aspx) |
| CleanFactor | Gets or sets axis clean factor, clean factor is used to determinate separations, default is 3. increase it to make it 'cleaner', separations have not easy to explain without seeing the source code.  | `int`  |
<hr/>
#[Separator](https://github.com/beto-rodriguez/Live-Charts/blob/master/Charts/Separator.cs)
helper class to define axis separators
<h3>Properties</h3>
| Name  | Description | Type |
| ------------- | ------------- | ------------- |
| Enabled | Indicates whether to draw separators or not. | `bool` |
| Color | Gets or sets separators color |  [`Color`](https://msdn.microsoft.com/en-us/library/system.windows.media.colors(v=vs.110).aspx) |
| Thickness | Gets or sets separators thickness | `int` |
| Step | Gets or sets separator step, default value changes according to chart, if null then it will be calculated based on series values | `double?` |
