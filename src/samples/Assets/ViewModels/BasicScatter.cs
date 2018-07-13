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

using System;
using System.Collections.ObjectModel;
using LiveCharts.Core.Collections;
using LiveCharts.Core.DataSeries;
using LiveCharts.Core.Defaults;
using LiveCharts.Core.Drawing;

#endregion

namespace Assets.ViewModels
{
    public class BasicScatter
    {
        public BasicScatter()
        {
            // feed the values

            ObservableCollection<PointModel> values2017 = new ObservableCollection<PointModel>();
            
            //                               ( x, y)
            values2017.Add(new PointModel(10, 6));
            values2017.Add(new PointModel(4, 3));
            values2017.Add(new PointModel(3, 9));
            values2017.Add(new PointModel(6, 2));

            var r = new Random();
            for (int i = 0; i < 5000; i++)
            {
                values2017.Add(
                    new PointModel(r.NextDouble() * 10, r.NextDouble() * 10));
            }

            ObservableCollection<PointModel> values2018 = new ObservableCollection<PointModel>();

            //                               (x, y)
            values2018.Add(new PointModel(5, 8));
            values2018.Add(new PointModel(5, 4));
            values2018.Add(new PointModel(9, 6));
            values2018.Add(new PointModel(3, 1));

            var series2018 = new ScatterSeries<PointModel> {Values = values2018};
            var series2017 = new ScatterSeries<PointModel> {Values = values2017};

            // some custom style.
            series2018.Geometry = Geometry.Diamond;
            series2017.Geometry = Geometry.Circle;

            // create a collection to store our series.
            // you can use any IEnumerable, it is recommended to use
            // the ChartingCollection<T> class, it inherits from
            // ObservableCollection and adds the AddRange/RemoveRange methods
            SeriesCollection = new ChartingCollection<ISeries>();
            // finally lets add the series to our series collection.
            SeriesCollection.Add(series2018);
            SeriesCollection.Add(series2017);
            // we bind the SeriesCollection property to the CartesianChart.Series property in XAML
        }

        public ChartingCollection<ISeries> SeriesCollection { get; set; }
    }
}