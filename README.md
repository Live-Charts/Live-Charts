<p align="center">
  <a href="http://lvcharts.net/"><img src="http://lvcharts.net/Content/Images/Logos/LiveChartsLogo.png" /></a>
</p>

<p align="center">
  
  <a href="http://lvcharts.net/App/examples/wpf/Constant%20Changes">
    <img src="https://cloud.githubusercontent.com/assets/10853349/15808855/3b93ee82-2b48-11e6-946c-b064e7e1d1f2.gif" />
  </a>
  <a href="http://lvcharts.net/App/examples/wpf/Doughnut%20Chart">
    <img src="https://cloud.githubusercontent.com/assets/10853349/15808857/3b9f0024-2b48-11e6-87fa-f52f1c2458b4.gif" />
  </a>
  <a href="http://lvcharts.net/App/examples/wpf/IObservableChartPoint">
    <img src="https://cloud.githubusercontent.com/assets/10853349/15808859/3ba1c638-2b48-11e6-830f-822e37b74b91.gif" />
  </a>
  <a href="http://lvcharts.net/App/examples/wpf/180%20Gauge">
    <img src="https://cloud.githubusercontent.com/assets/10853349/15808856/3b9499ae-2b48-11e6-91a8-f74f6a1fd6dc.gif" />
  </a>
  <a href="http://lvcharts.net/App/examples/wpf/Zooming%20and%20panning">
    <img src="https://cloud.githubusercontent.com/assets/10853349/15808858/3ba164cc-2b48-11e6-9390-057b80ed92fb.gif" />
  </a>
  <a href="http://lvcharts.net/App/examples/wpf/Bubble%20Chart">
    <img src="https://cloud.githubusercontent.com/assets/10853349/15808860/3ba2da78-2b48-11e6-9599-aca30ea61ae2.gif" />
  </a>
</p>

[![GitHub license](https://img.shields.io/github/license/beto-rodriguez/Live-Charts.svg?style=flat-square)](https://github.com/beto-rodriguez/Live-Charts/blob/master/LICENSE.TXT)
[![AppVeyor](https://ci.appveyor.com/api/projects/status/707m8sye0ggbfrcq)](https://ci.appveyor.com/project/beto-rodriguez/live-charts)
[![GitHub issues](https://img.shields.io/github/issues/beto-rodriguez/Live-Charts.svg?style=flat-square)](https://github.com/beto-rodriguez/Live-Charts/issues)
[![Gitter](https://img.shields.io/gitter/room/beto-rodriguez/Live-Charts.svg?style=flat-square)](https://gitter.im/beto-rodriguez/Live-Charts?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)

* **[WebSite](http://lvcharts.net/)**
* **[Chat](https://gitter.im/beto-rodriguez/Live-Charts)**
* **Questions, try [Stack Overflow](http://stackoverflow.com/), tag your question as LiveCharts**

Animated, MVVM orientated .Net charts, for WPF, WinForms under MIT Licence, written in C#, LiveCharts listens for any change in your data automatically and updates UI.

Live charts makes your data alive, as easy as manipulating any .net list, the library animates and updates every time your data changes.

 - Good looking, animated and easy to customize charts, you can practically change all properties.
 - Autoupdate UI, you just create a SeriesCollection, then LiveCharts will handle everything.
 - MVVM Charting, Support for WPF Bindings.
 - Supports zooming and panning.
 - MIT License, permissive licensing, yes free.
 
**Why is LiveCharts different to other charting libraries?**

First: animations and good looking charts by default, second: responsive charts these charts automatically update when your data changes also when you resize your chart, finally LiveCharts MVVM is different and for me it feels better than other libraries, becuase LiveCharts uses generics and with it strongly typed modeling, others use reflection to get the property value.

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

### Is live charts what you are looking for? see these interesting examples.

* **[Live Data](http://lvcharts.net/App/examples/wpf/Constant%20Changes)**: a charts that adds new data each 300ms, DateTime as X Axis.
* **[Observable Points](http://lvcharts.net/App/examples/v1/wpf/IObservableChartPoint)**: notify the chart to update every time a property changes
* **[Simple Bar Chart](http://lvcharts.net/App/examples/wpf/Basic%20Column)**: a simple bar chart.
* *Do not need animations*? ok disable them, performance will be increased also, `Chart.DisableAnimations = true;`

###Support

WPF and Winforms, currenlty the library is in the process to become a cross net library...

###Net Version

.Net 4.0.3 or greater, Windows XP SP3 at least, for more info see [#212](https://github.com/beto-rodriguez/Live-Charts/issues/212)

### Installation

Verify your project uses .Net 4.0.3 or greater, then follow these stepts.

* [Wpf](http://lvcharts.net/App/examples/wpf/Install)
* [WinForms](http://lvcharts.net/App/examples/wf/Install)

To verify if you are using a supported .net version, go to SolutionExplorer > Right click on your project > Properties > Application, then verify Taret framework is set at least to .net 4.0.3 

### Migrating from older versions?

Please see [https://github.com/beto-rodriguez/Live-Charts/releases](https://github.com/beto-rodriguez/Live-Charts/releases)

### How to Contribute

* **Star** this repo
* Try it
* Report Issues and Improvements
* Pull request are well received

You can also buy me a beer

<form action="https://www.paypal.com/cgi-bin/webscr" method="post" target="_top">
<input type="hidden" name="cmd" value="_s-xclick">
<input type="hidden" name="hosted_button_id" value="J86WDLSS9PWGL">
<input type="image" src="https://www.paypalobjects.com/en_GB/i/btn/btn_donate_LG.gif" border="0" name="submit" alt="PayPal – The safer, easier way to pay online.">
<img alt="" border="0" src="https://www.paypalobjects.com/es_XC/i/scr/pixel.gif" width="1" height="1">
</form>
[![paypal](https://www.paypalobjects.com/en_US/i/btn/btn_donateCC_LG.gif)](https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=J86WDLSS9PWGL)

### Examples?

The [web site](http://lvcharts.net/App/examples/wpf/start) has a nice set, they are also built in the examples folder up here ^^^^

### High perfomance?

LiveCharts will also handle high performance, it is not ready yet but the current tests allows the library to plot 3,000,000 in 1 second, yes even with aniations.

### Road Map

* Build at least all the features any other charting library does, in WPF and WinForms
* Build High performance algorithms
* Expand the library to at least Xamarin and UWP (should not be that hard, the code is already designed to support this point)
* release 1.0 in WPF and WinForms, the other platforms will be beta, in case something went wrong.
