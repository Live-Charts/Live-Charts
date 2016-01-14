
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

Live charts is an easy way to build useful charts, all charts are animated, they update every time you change your data, or when you rezise the chart, also since 0.5 we are working to support huge amounts of data, right now this is on test and only implemented in line chart, but in the included examples it is able to draw 1'000,000 points in a really short period of time.

 - MVVM Charting, Support for WPF Binding, All charts update when data changes.
 - Good looking, animated and easy to customize charts, you can practically change all properties.
 - Easy to maintain and create new charts, as you can see in the source code, some charts have almost no code.
 - Supports zooming and panning.
 - MIT License, permissive licensing.
 
This is the logic you use in every chart, there are just some litle properties or rules that change from each type of chart. Use the sintax that better fits your needs.

## In Line Charting 

Useful when you need to plot easly static series and static values.

**XAML** [see full file](https://github.com/beto-rodriguez/Live-Charts/blob/master/ChartsTest/Line%20Examples/BasicLine.xaml)
```xml
 <liveCharts:LineChart Name="Chart">
  <liveCharts:LineChart.Series>
    <liveCharts:SeriesCollection>
      <liveCharts:LineSeries Title="Maria" PrimaryValues="20, 40, 45, 60, 55, 60, 65, 70" />
      <liveCharts:LineSeries Title="John" PrimaryValues="30, 35, 43, 68, 65 ,70, 55, 60" />
    </liveCharts:SeriesCollection>
  </liveCharts:LineChart.Series>
</liveCharts:LineChart>
```

## Partial Binding

Useful to keep your view models simple and when you have a static number of series.

**XAML** [see full file](https://github.com/beto-rodriguez/Live-Charts/blob/master/ChartsTest/Line%20Examples/BindingLine.xaml)
```xml
<liveCharts:LineChart>
  <liveCharts:LineChart.Series>
   <liveCharts:SeriesCollection>
      <liveCharts:LineSeries PrimaryValues="{Binding ViewModel.FirstSeries}" />
      <liveCharts:LineSeries PrimaryValues="{Binding ViewModel.SecondSeries}" />
    </liveCharts:SeriesCollection>
  </liveCharts:LineChart.Series>
</liveCharts:LineChart>
```
**CodeBehind** [see full file](https://github.com/beto-rodriguez/Live-Charts/blob/master/ChartsTest/Line%20Examples/BindingLine.xaml.cs)

*view model*
```c#
public class BindingLineViewModel
{
  public ObservableCollection<double> FirstSeries { get; set; }
  public ObservableCollection<double> SecondSeries { get; set; }
}
```
*view constructor*
```c#
ViewModel = new BindingLineViewModel
{
  FirstSeries = new ObservableCollection<double> { 2, 4, double.NaN, 7, 8, 6, 2, 4, 2, 5 },
  SecondSeries = new ObservableCollection<double> { 7, 3, 4, 1, 5, 6, 8, 5, 1, 3 }
};
DataContext = this;
```

## Full Binding

Useful when you need to change the number of series and the values of each serie.

**XAML** [see full file][https://github.com/beto-rodriguez/Live-Charts/blob/master/ChartsTest/Line%20Examples/MvvmLine.xaml]
```xml
<liveCharts:LineChart Series="{Binding Sales.Salesmen}" />
```
**Code Behind** [see full file](https://github.com/beto-rodriguez/Live-Charts/blob/master/ChartsTest/Line%20Examples/MvvmLine.xaml.cs)
*view model*
```c#
public class SalesViewModel
{
  public SalesViewModel()
  {
    Salesmen = new ObservableCollection<Series>
    {
      new LineSeries
      {
        Title = "John",
        PrimaryValues = new ObservableCollection<double>(new[] {2d, 4, 7, 1, 5})
      },
      new LineSeries
      {
        Title = "Maria",
        PrimaryValues = new ObservableCollection<double>(new[] {5d, 3, 2, 4, 7})
      }
    };
 }

 public ObservableCollection<Series> Salesmen { get; set; }

}
```
*view contructor*
```c#
Sales = new SalesViewModel();
DataContext = this;
```

**Full Code Behind or WinForms**

```c#
Chart.Series.Add(new LineSeries
{
    Title = "Charles",
    PrimaryValues = new ObservableCollection<double> { 5, 8, 1, 9}
});
Chart.Series.Add(new LineSeries
{
    Title = "Maria",
    PrimaryValues = new ObservableCollection<double> { 4, 1, 2, 7}
});
```

# Installation

**1**. Install package from [**Nuget**](https://www.nuget.org/packages/LiveCharts) `Install-Package LiveCharts`


**2**. Add name space to your `XAML` 
```
xmlns:liveCharts="clr-namespace:LiveCharts;assembly=LiveCharts"
```
**3**. Thats it. You are ready.

**Note:** Since this is a pre-release version, some names, properties or namespaces might vary, we highly recommend to clone this repo and see included examples since they all are always up to date.

# How to Contribute

* Star this repo
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
