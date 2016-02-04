
<p align="center">
  <img src="https://dl.dropboxusercontent.com/u/40165535/LiveCharts/live.png" />
</p>

<p align="center">
  <img src="https://dl.dropboxusercontent.com/u/40165535/LiveCharts/LineChart.gif" width="200" />
  <img src="https://dl.dropboxusercontent.com/u/40165535/LiveCharts/BarChart.gif" width="200"/>
  <img src="https://dl.dropboxusercontent.com/u/40165535/LiveCharts/StackedBarChart.gif" width="200"/>
  <img src="https://dl.dropboxusercontent.com/u/40165535/LiveCharts/PieChart.gif" width="200"/>
</p>

[![Join the chat at https://gitter.im/beto-rodriguez/Live-Charts](https://badges.gitter.im/beto-rodriguez/Live-Charts.svg)](https://gitter.im/beto-rodriguez/Live-Charts?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)

Live charts is an easy way to build poweful charts, all charts are animated, they update every time you change your data, it also has an awesome performance. 

 - Good looking, animated and easy to customize charts, you can practically change all properties.
 - Autoupdate UI, you just create a SeriesCollection, then LiveCharts will handle everything.
 - MVVM Charting, Support for WPF Bindings.
 - Easy to maintain and create new charts, as you can see in the source code, some charts have almost no code.
 - Supports zooming and panning.
 - MIT License, permissive licensing.
 
This is the logic you use in every chart, there are just some litle properties or rules that change from each type of chart

```c#
//create a new SeriesCollection
var seriesCollection = new SeriesCollection();

//create some LineSeries if ypu need so
var charlesSeries = new LineSeries
{
  Title = "Charles",
  Values = new ChartValues<double> {10, 5, 7, 5, 7, 8}
};
var jamesSeries = new LineSeries
{
  Title = "James",
  Values = new ChartValues<double> { 5, 6, 9, 10, 11, 9 }
};

//add series to seriesCollection
seriesCollection.Add(charlesSeries);
seriesCollection.Add(jamesSeries);

//now just assing this seriesCollectionto your chart
//you can use wpf bindings if you need it
myChart.Series = seriesCollection

//create some labels if necessary
var labels = new string[] {"Jan", "Feb" , "Mar", "Apr", "May", "Jun", "Jul", "Ago", "Sep", "Oct", "Nov", "Dec"};
myChart.AxixX.Labels = labels;
```

# Installation

**1**. Install package from [**Nuget**](https://www.nuget.org/packages/LiveCharts) `Install-Package LiveCharts`

**2**. Add name space to your `XAML` 
```xml
xmlns:lvc="clr-namespace:LiveCharts;assembly=LiveCharts"
```
**3**. Thats it. You are ready.

# Interesting examples

* **[Live Data](https://github.com/beto-rodriguez/Live-Charts/wiki/91-Live-Data)**: a charts that adds new data each second, DateTime as X Axis.
* **[Filter records from a data base]**: a chart that pulls data from a data base according to a simple user filter.
* **[IObservableChartPoint]**: this chart uses a view model that implements `IObservableChartPoint`, this will update chart every time a desired property changes.
* There are much more examples in this solution, go to examples folder up here ^^^

# How to Contribute

* **Star** this repo
* Try it
* Report Issues and Improvements
* Pull request are well received

# Need examples?

Go to examples folder up there ^^^ or even better clone this repo and see included examples.

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
