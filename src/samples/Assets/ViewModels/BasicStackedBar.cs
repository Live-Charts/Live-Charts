#region License
// The MIT License (MIT)
// 
// Copyright (c) 2016 Alberto Rodríguez Orozco & LiveCharts contributors
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy 
// of this software and associated documentation files (the "Software"), to deal 
// in the Software without restriction, including without limitation the rights to 
// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies 
// of the Software, and to permit persons to whom the Software is furnished to 
// do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all 
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, 
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES 
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND 
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT 
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, 
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING 
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR 
// OTHER DEALINGS IN THE SOFTWARE.
#endregion

#region

using System.Collections.ObjectModel;
using LiveCharts.Core.Collections;
using LiveCharts.Core.DataSeries;

#endregion

namespace Assets.ViewModels
{
    public class BasicStackedBar
    {
        public BasicStackedBar()
        {
            // let's feed the series...
            
            ObservableCollection<double> charlesValues = new ObservableCollection<double>();
            charlesValues.Add(4d); // *
            charlesValues.Add(8d);
            charlesValues.Add(-3d);

            ObservableCollection<double> fridaValues = new ObservableCollection<double>();
            fridaValues.Add(5d); // *
            fridaValues.Add(-3d);
            fridaValues.Add(8d);

            ObservableCollection<double> abrahamValues = new ObservableCollection<double>();
            abrahamValues.Add(5d); // *
            abrahamValues.Add(2d);
            abrahamValues.Add(-7d);

            // The stack by default is done based on the index of each element 
            // so charles, frida and abraham at index = 0 share 4, 5 and 5 values (see * mark)
            // the total height of the bar when index = 0 will be 4 + 5 + 5 = 14

            // create a collection to store our series.
            // you can use any IEnumerable, it is recommended to use
            // the ChartingCollection<T> class, it inherits from
            // ObservableCollection and adds the AddRange/RemoveRange methods
            SeriesCollection = new ChartingCollection<ISeries>();
            // finally lets add the series to our series collection,
            // we can add as many series as we need, in this case we will
            // only display one series.
            SeriesCollection.Add(
                new StackedBarSeries<double>
                {
                    Values = charlesValues
                });
            SeriesCollection.Add(
                new StackedBarSeries<double>
                {
                    Values = fridaValues
                });
            SeriesCollection.Add(
                new StackedBarSeries<double>
                {
                    Values = abrahamValues
                });
            // we bind the SeriesCollection property to the CartesianChart.Series property in XAML
        }

        public ChartingCollection<ISeries> SeriesCollection { get; set; }
    }
}