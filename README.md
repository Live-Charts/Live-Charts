<p align="center">
  <img src="http://lvcharts.net/Content/Images/Logos/LiveChartsLogo.png" />
</p>

<p align="center">
  <img src="http://lvcharts.net/Content/Images/Samples/linemove.gif" width="400" />
  <img src="http://lvcharts.net/Content/Images/Samples/pie.gif" width="400"/>
  <img src="http://lvcharts.net/Content/Images/Samples/barsmove.gif" width="400"/>
  <img src="https://cloud.githubusercontent.com/assets/10853349/14480111/65c18a98-00eb-11e6-8ce2-bc7cf3d0fdfc.gif" width="400" />
  <img src="https://cloud.githubusercontent.com/assets/10853349/15451982/2041fb7a-1fa3-11e6-9d25-4471c09b3cb2.gif" width="400" />
  <img src="https://cloud.githubusercontent.com/assets/10853349/15452005/2792e230-1fa4-11e6-8bd3-5aed219d3256.gif" width="400" />
  <img src="https://cloud.githubusercontent.com/assets/10853349/15451981/1d37d0e4-1fa3-11e6-8c79-825f580f6819.png" width="400" />
  <img src="https://cloud.githubusercontent.com/assets/10853349/15451980/1bf8b482-1fa3-11e6-8945-e0e08f33a347.gif" width="400" />
</p>

[![GitHub license](https://img.shields.io/github/license/beto-rodriguez/Live-Charts.svg?style=flat-square)](https://github.com/beto-rodriguez/Live-Charts/blob/master/LICENSE.TXT)
[![AppVeyor](https://ci.appveyor.com/api/projects/status/707m8sye0ggbfrcq)](https://ci.appveyor.com/project/beto-rodriguez/live-charts)
[![GitHub issues](https://img.shields.io/github/issues/beto-rodriguez/Live-Charts.svg?style=flat-square)](https://github.com/beto-rodriguez/Live-Charts/issues)
[![NuGet](https://img.shields.io/nuget/dt/LiveCharts.svg?style=flat-square)](https://www.nuget.org/packages/LiveCharts/)
[![Gitter](https://img.shields.io/gitter/room/beto-rodriguez/Live-Charts.svg?style=flat-square)](https://gitter.im/beto-rodriguez/Live-Charts?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)

* **[WebSite](http://lvcharts.net/)**
* **[Chat](https://gitter.im/beto-rodriguez/Live-Charts)**
* **Questions, try [Stack Overflow](http://stackoverflow.com/), tag your question as LiveCharts**

#### 0.7.0 is here, if you come from an older version, its recommended to read [this](https://github.com/beto-rodriguez/Live-Charts/releases/tag/0.7.0), examples in this repo are updated to 0.7.0, web site update is in progress, 0.6.6 examples [here](https://github.com/beto-rodriguez/Live-Charts/tree/12fbb648c93e147dc05fc30b6ed65816db305c70/WPFExamples)

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

## Is live charts what you are looking for? see these interesting examples.

* **[Live Data](https://github.com/beto-rodriguez/Live-Charts/wiki/91-Live-Data)**: a charts that adds new data each second, DateTime as X Axis.
* **[Filter records from a data base](https://github.com/beto-rodriguez/Live-Charts/wiki/92-Filtered-Data)**: a chart that pulls data from a data base according to a simple user filter.
* **[IObservableChartPoint](https://github.com/beto-rodriguez/Live-Charts/wiki/93-IObservableChartPoint)**: this chart uses a view model that implements `IObservableChartPoint`, this will update chart every time a desired property changes.
* There are much more examples in this solution, go to examples folder up here ^^^
* High performance charts are almost ready, current test can draw 3 million points in only 1 second
* Find more at http://lvcharts.net/

#Supported Platforms

Only WPF and WinForms for now, I am playing with the code to find an easy way to extend it as PCL

# Installation

**1**. Install package from [**Nuget**](https://www.nuget.org/packages/LiveCharts) `Install-Package LiveCharts.Wpf`

**2**. Add name space

To your `XAML` if using wpf
```xml
xmlns:lvc="clr-namespace:LiveCharts.Wpf;assembly=LiveCharts.Wpf"
```
Or WinForms and WPF code behind
```
using LiveCharts;
```
**3**. Thats it. You are ready.

**Note:** To install it for windows forms you will need some extra steps, plese see http://lvcharts.net/#/examples/v1/install-wf?path=WF-Install

# How to Contribute

<form action="https://www.paypal.com/cgi-bin/webscr" method="post" target="_top">
<input type="hidden" name="cmd" value="_s-xclick">
<input type="hidden" name="hosted_button_id" value="J86WDLSS9PWGL">
<input type="image" src="https://www.paypalobjects.com/en_GB/i/btn/btn_donate_LG.gif" border="0" name="submit" alt="PayPal – The safer, easier way to pay online.">
<img alt="" border="0" src="https://www.paypalobjects.com/es_XC/i/scr/pixel.gif" width="1" height="1">
</form>

[![paypal](https://www.paypalobjects.com/en_US/i/btn/btn_donateCC_LG.gif)](https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=J86WDLSS9PWGL)

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

This test is in progress and is not ready yet, here is the example I made the tests but is not ready yet!

https://github.com/beto-rodriguez/Live-Charts/tree/master/WPFExamples/HighPerformance

<p align="center">
<img src="https://dl.dropboxusercontent.com/u/40165535/livecharts%20perfomance.png" />
</p>
