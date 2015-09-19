
<p align="center">
  <img src="https://dl.dropboxusercontent.com/u/40165535/live.png" />
</p>
<p align="center">
  <img src="https://dl.dropboxusercontent.com/u/40165535/live1.gif" />
  <img src="https://dl.dropboxusercontent.com/u/40165535/live2.gif" />
  <img src="https://dl.dropboxusercontent.com/u/40165535/live3.gif" />
</p>

**What this library is**
 - Good looking, animated and easy to customize charts, you can practically change all properties.
 - Easy to maintain and create new charts, as you can see in the source code, some charts have almost no code.
 - Ready for WPF Bindings.
 - Supports zooming and panning.
 - MIT License, permissive licesing.

**What this is not**
 - A high performance library (by now), the first target of this library is to make good looking charts. That does not means performance will not be improved.

I decided to start this because current open source alternatives were not exactly what I needed. The best options I could find were:

   - **[Oxy plot](http://oxyplot.org/)**: It's really good, open source, has a nice documentation but its hard to customize, I downloaded the library and the first thing I wanted to change was Axis color and I after some hours of trying I couldn't, another good point is that is support Silverlight, WP8, Andorid, IOS, Mac, but at the same time that makes Oxyplot really hard to mantain. We deserve a specialized library for .net
   - **[Modern UI Charts](https://modernuicharts.codeplex.com/)** It was almost what I was looking for except for some details, it does not have simple line charts and licensing is not so permissive,  I’m not a lawyer but it does not seems to friendly for commercial use.
   - Now if you Google it you will surely arrive to [this Stackoverflow question](http://stackoverflow.com/questions/577278/wpf-chart-controls), but that question is closed and the newest answer was at May 2013. So most of answers are out of date.
   - The last option pay for a charting library. Really? Is this that an option? How can someone charge you any amount of money for some thousands of lines, we should build our own.

#Instalation

 1. Install package from [**Nuget**](https://www.nuget.org/packages/LiveCharts/0.0.1) `Install-Package LiveCharts`
 2. Add name space to your `Window` XAML `xmlns:charts="clr-namespace:Charts.Charts;assembly=Charts"`
 3. Thats it. You are ready.
 
#Examples
It is recommended to clone this repo on your desktop, you only have to click the `Open In Visual Studio` button to the right, and see examples included but here is a resume.

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

#Included Graphs

  - Lines and Areas
  - Bars
  - Pie and doughnut
  - Scatter

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
<h3>Methods</h3>
| Name  | Description | Returned Type |
| ------------- | ------------- | ------------- |
| Zooming  | Gets or sets whether graph can zoom  | `bool` |
<hr/>
#[BarChart](https://github.com/beto-rodriguez/Live-Charts/blob/master/Charts/Charts/BarChart.cs)
Inherits from `Chart`
<h3>Properties</h3>
No extra properties
<h3>Methods</h3>
None extra methods
<hr/>
#[LineChart](https://github.com/beto-rodriguez/Live-Charts/blob/master/Charts/Charts/LineChart.cs)
Inherits from `Chart`
<h3>Properties</h3>
| Name  | Description | Type |
| ------------- | ------------- | ------------- |
| IncludeArea | Gets or sets whether series should draw its area | `bool` |
| LineType | Gets or sets line type to draw | [`LineChartLineType`](https://github.com/beto-rodriguez/Live-Charts/blob/master/Charts/LineChartLineType.cs) |
<h3>Methods</h3>
None extra methods
<hr/>
#[PieChart](https://github.com/beto-rodriguez/Live-Charts/blob/master/Charts/Charts/PieChart.cs)
Inherits from `Chart`
<h3>Properties</h3>
| Name  | Description | Type |
| ------------- | ------------- | ------------- |
| InnerRadius | Gets or sets chart inner radius | `double` |
| SlicePadding | Gets or sets slice padding | `double` |
<h3>Methods</h3>
None extra methods
<hr/>
#[ScatterChart](https://github.com/beto-rodriguez/Live-Charts/blob/master/Charts/Charts/ScatterChart.cs)
Inherits from `Chart`
<h3>Properties</h3>
| Name  | Description | Type |
| ------------- | ------------- | ------------- |
| LineType | Gets or sets line type to draw | [`LineChartLineType`](https://github.com/beto-rodriguez/Live-Charts/blob/master/Charts/LineChartLineType.cs) |
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
