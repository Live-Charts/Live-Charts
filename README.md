
<p align="center">
  <img src="https://dl.dropboxusercontent.com/u/40165535/LiveCharts/live.png" />
</p>

<p align="center">
  <img src="https://dl.dropboxusercontent.com/u/40165535/LiveCharts/LineChart.gif" />
  <img src="https://dl.dropboxusercontent.com/u/40165535/LiveCharts/BarChart.gif" />
  <img src="https://dl.dropboxusercontent.com/u/40165535/LiveCharts/StackedBarChart.gif" />
  <img src="https://dl.dropboxusercontent.com/u/40165535/LiveCharts/PieChart.gif" />
  <img src="https://dl.dropboxusercontent.com/u/40165535/LiveCharts/ScatterChart.gif" />
</p>

### 0.6.0 is here, If you come from an older verison concider reading [this](https://github.com/beto-rodriguez/Live-Charts/releases/tag/0.6.0)

[![Join the chat at https://gitter.im/beto-rodriguez/Live-Charts](https://badges.gitter.im/beto-rodriguez/Live-Charts.svg)](https://gitter.im/beto-rodriguez/Live-Charts?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)

Live charts is an easy way to build useful charts, all charts are animated, they update every time you change your data, or when you rezise the chart, it also has an awesome performance. 

 - **3,000,000 points in 1 second, (coming soon in next release, almost ready!)**
 - **MVVM Charting, Support for WPF Binding, All charts update when data changes.**
 - **Good looking, animated and easy to customize charts, you can practically change all properties.**
 - **Easy to maintain and create new charts, as you can see in the source code, some charts have almost no code.**
 - **Supports zooming and panning.**
 - **MIT License, permissive licensing.**
 
This is the logic you use in every chart, there are just some litle properties or rules that change from each type of chart. Use the syntax that better fits your needs, you can also see examples here: [Winforms](https://github.com/beto-rodriguez/Live-Charts/tree/master/WinForms), [WPF](https://github.com/beto-rodriguez/Live-Charts/tree/master/ChartsTest)

## a) In Line Charting 

Useful when you just a chart now! with static number of series and values.

**XAML** [see full file](https://github.com/beto-rodriguez/Live-Charts/blob/master/ChartsTest/Line%20Examples/Basic/BasicLine.xaml)
```xml
 <lvc:LineChart>
    <lvc:LineChart.Series>
      <lvc:LineSeries Title="Maria" Values="20, 40, 45, 60, 55, 60, 65, 70" />
      <lvc:LineSeries Title="John" Values="30, 35, 43, 68, 65 ,70, 55, 60" />
    </lvc:LineChart.Series>
</lvc:LineChart>
```

## b) Partial Binding

Useful to keep your view models simple and when you have a static number of series.

**XAML** [see full file](https://github.com/beto-rodriguez/Live-Charts/blob/master/ChartsTest/Line%20Examples/Binding/BindingLine.xaml)
```xml
<lvc:LineChart>
  <lvc:LineChart.Series>
    <lvc:LineSeries Title="Series 1" Values="{Binding ViewModel.Series1}" />
    <lvc:LineSeries Title="Series 2" Values="{Binding ViewModel.Series2}" />
  </lvc:LineChart.Series>
</lvc:LineChart>
```
**CodeBehind** [see full file](https://github.com/beto-rodriguez/Live-Charts/blob/master/ChartsTest/Line%20Examples/Binding/BindingLine.xaml.cs)

*view model*
```c#
public class BindingLineViewModel
{
  public ChartValues<double> Series1 { get; set; }
  public ChartValues<double> Series2 { get; set; }
}
```
*view constructor*
```c#
ViewModel = new BindedLinesViewModel
{
  Series1 = new ChartValues<double> {15, 25, 29, 32, 16, 10},
  Series2 = new ChartValues<double> {12, 10, 9, 8, 5, -10 }
};

DataContext = this;
```

## c) Full Binding (Recommended Method)

Useful when you need to change the number of series and the values of each serie, in this example also we are not ploting just a column of dobule, we are ploting `ChartValues<SalesData>` then we specify wich property to use for X and Y.

**XAML** [see full file](https://github.com/beto-rodriguez/Live-Charts/blob/master/ChartsTest/Line%20Examples/Mvvm/MvvmLine.xaml)
```xml
<lvc:LineChart Series="{Binding Sales.SalesmenSeries}" ></lvc:LineChart>
```
**Code Behind** [see full file](https://github.com/beto-rodriguez/Live-Charts/blob/master/ChartsTest/Line%20Examples/Mvvm/MvvmLine.xaml.cs)

*view model*
```c#
public class SalesViewModel
{
  public SalesViewModel()
  {
    //Specify a setup for SeriesCollection class, so LiveCharts know which property use as Y, 
    //you can also specify X, but in this case, It will use a zero based index (default config)
    SalesmenSeries = new SeriesCollection (new SeriesConfiguration<SalesData>().Y(data => data.ItemsSold))
    {
      new LineSeries
      {
        Title = "Charles",
        Values = new ChartValues<SalesData> { new SalesData {ItemsSold = 15, ... }, ... }
      },
      new LineSeries
      {
        Title = "Frida",
        Values = new ChartValues<SalesData> { new SalesData {ItemsSold = 25, ...  } ... }
      },
      
      // This series is ploting another type, you can also override configuration only for a Series
      // to map to another property or Type
      new LineSeries (new SeriesConfiguration<AverageSalesData>().Y(data => data.AverageItemsSold))
      {
        Title = "Average Series",
        Values = new ChartValues<AverageSalesData> { new AverageSalesData {AverageItemsSold = 22} ... }
      }
    }
  }

  public SeriesCollection SalesmenSeries { get; set; }

}
```
*view contructor*
```c#
Sales = new SalesViewModel();
DataContext = this;
```

## d) Full Code Behind or WinForms

```c#
Chart.Series.Add(new LineSeries
{
    Title = "Charles",
    Values = new ChartValues<double> { 5, 8, 1, 9}
});
Chart.Series.Add(new LineSeries
{
    Title = "Maria",
    Values = new ChartValues<double> { 4, 1, 2, 7}
});
```

# Installation

**1**. Install package from [**Nuget**](https://www.nuget.org/packages/LiveCharts) `Install-Package LiveCharts`


**2**. Add name space to your `XAML` 
```
clr-namespace:LiveCharts;assembly=LiveCharts
```
**3**. Thats it. You are ready.

**Note:** Since this is a pre-release version, some names, properties or namespaces might vary, we highly recommend to clone this repo and see included examples since they all are always up to date.

Or take a look to this exmaples too

* **[Plot Types not Values! (Mvvm Recommended method)](https://github.com/beto-rodriguez/Live-Charts/tree/master/ChartsTest/Line%20Examples/Mvvm)** By default series values are double, but live charts allows you to plot any type you need, in this case we plot a collection of SalesData class. click on the buttons bellow to see how live charts track your data changes, also chart will follow if you resize window, notice this chart implements a custom tooltip to display Rentability property too.
* **[In line Charting](https://github.com/beto-rodriguez/Live-Charts/tree/master/ChartsTest/Line%20Examples/Basic)** when you only need to plot easy and now!
* **[Partial Binding](https://github.com/beto-rodriguez/Live-Charts/tree/master/ChartsTest/Line%20Examples/Binding)** if you have a static number of series and need to change their Values this might help you, click on the buttons bellow to see how charts update automatically with their data
* **[Mvvm Lazy Data](https://github.com/beto-rodriguez/Live-Charts/tree/master/ChartsTest/Line%20Examples/LazyData)** this example shows a dynamic number of charts and dynamic values, click on the buttons of each chart to see how the change, click add new chart to add a new one.
* **[Zoomable Chart](https://github.com/beto-rodriguez/Live-Charts/tree/master/ChartsTest/Line%20Examples/Zoomable)** this examples shows how easly it is to support zooming and panning in a chart.
* **[Add Ui Elements](https://github.com/beto-rodriguez/Live-Charts/tree/master/ChartsTest/Line%20Examples/UiElements)** you can also add any UI element to a chart.
* **[Custom Style](https://github.com/beto-rodriguez/Live-Charts/tree/master/ChartsTest/Line%20Examples/Custom)** this is an example of how to change the default style of live charts
   * [See Custom Tooltip too](https://github.com/beto-rodriguez/Live-Charts/tree/master/ChartsTest/z.CustomTooltips)

# How to Contribute

* **Star** this repo
* Try it
* Report Issues and Improvements
* Pull request are well received

# Need examples?

Try [Live Charts Wiki](https://github.com/beto-rodriguez/Live-Charts/wiki), or cloning this repo, test project includes a lot of examples, copy and paste this link in your browser for a cloning shortcut
```
git-client://clone?repo=https%3A%2F%2Fgithub.com%2Fbeto-rodriguez%2FLive-Charts
```

# More Images

<p align="center">
<img src="https://dl.dropboxusercontent.com/u/40165535/LiveCharts/Tooltip.gif" />
</p>
<p align="center">
<img src="https://dl.dropboxusercontent.com/u/40165535/LiveCharts/multiseries.png" />
</p>
<p align="center">
<img src="https://dl.dropboxusercontent.com/u/40165535/LiveCharts/UiElements.png" />
</p>

# Perfomance Test

<p align="center">
<img src="https://dl.dropboxusercontent.com/u/40165535/livecharts%20perfomance.png" />
</p>
