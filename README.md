
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

Live charts is an easy way to build useful charts, all charts are animated, they update every time you change your data, or when you rezise the chart, it also has an awesome performance. 

 - **Good looking, animated and easy to customize charts, you can practically change all properties.**
 - **3,000,000 points in 1 second, (coming soon, its practically finished)**
 - **MVVM Charting, Support for WPF Binding, All charts update when data changes.**
 - **Easy to maintain and create new charts, as you can see in the source code, some charts have almost no code.**
 - **Supports zooming and panning. (right now we have made some changes here, so be patient)**
 - **MIT License, permissive licensing.**
 
This is the logic you use in every chart, there are just some litle properties or rules that change from each type of chart. You can also see examples here: [Winforms](https://github.com/beto-rodriguez/Live-Charts/tree/master/WinForms), [WPF](https://github.com/beto-rodriguez/Live-Charts/tree/master/ChartsTest)

Add as many series and values as you need, they can also change dynamically, dont worry, LiveCharts handle it.
```c#
var series = new SeriesCollection();

var charlesSeries = new LineSeries
{
  Title = "Charles",
  Values = new ChartValues<double> {10, 5, 7, 5, 7, 8}
};

var jamesSeries = new LineSeries
{
  Title = "James",
  Values = new ChartValues<double> {5, 6, 9, 10, 11, 9}
};

series.Add(charlesSeries);
series.Add(jamesSeries);
```
Now just Set Series property of your chart to this `SeriesCollection` you just created

*WPF* [see full example here](https://github.com/beto-rodriguez/Live-Charts/tree/master/ChartsTest/Line%20Examples/Basic)
<lvc:LineChart Series{Binding Series}>

*WinForms* [see full example here](https://github.com/beto-rodriguez/Live-Charts/tree/master/WinForms/LineExamples/Basic)
```c#
linechart1.Series = series;
```

# Installation

**1**. Install package from [**Nuget**](https://www.nuget.org/packages/LiveCharts) `Install-Package LiveCharts`

**2**. Add name space to your `XAML` 
```xml
xmlns:lvc="clr-namespace:LiveCharts;assembly=LiveCharts"
```
**3**. Thats it. You are ready.

**Note:** Since this is a pre-release version, some names, properties or namespaces might vary, we highly recommend to clone this repo and see included examples since they all are always up to date.

Or take a look to this exmaples too

 * [Winforms](https://github.com/beto-rodriguez/Live-Charts/tree/master/WinForms)
 * [WPF](https://github.com/beto-rodriguez/Live-Charts/tree/master/ChartsTest)

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
