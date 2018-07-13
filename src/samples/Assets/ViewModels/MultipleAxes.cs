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
using LiveCharts.Core.Dimensions;

#endregion

namespace Assets.ViewModels
{
    public class MultipleAxes
    {
        public MultipleAxes()
        {
            // by default a cartesian chart axis contains only one element
            // and all the series are scaled in the default axis.
            // in this case we will only override the Y axis
            // the X axis will have the default value.
            var axis1 = new Axis{Position = AxisPosition.Right};
            var axis2 = new Axis{Position = AxisPosition.Right};

            // we create a collection to store our axes collection.
            YAxis = new ChartingCollection<Plane>();
            YAxis.Add(axis1);
            YAxis.Add(axis2);

            // we will bind in XAML Planes to CartesianChart.AxisY property

            // now we create some series.

            ObservableCollection<double> values1 = new ObservableCollection<double>();
            values1.Add(10d);
            values1.Add(5d);
            values1.Add(3d);
            values1.Add(6d);

            var series1 = new LineSeries<double>
            {
                Values = values1
            };

            ObservableCollection<double> values2 = new ObservableCollection<double>();
            values2.Add(350d);
            values2.Add(650d);
            values2.Add(125d);
            values2.Add(156d);

            var series2 = new LineSeries<double>
            {
                Values = values2
            };

            // we will scale series1 at axis1
            // axis1 is the first element in the Planes collection
            // then the index of axis1 in the Planes array is 0
            // so we will let the series know that is needs to be
            // scaled in Y at 0
            series1.ScalesYAt = 0;
            
            // axis2 index in the Planes collection is 1
            // we want the series2 to be scaled at axis2
            // then the series2 ScalesYAt property must match
            // the index of the axis2 instance in the Planes collection.
            series2.ScalesYAt = 1;

            // finally we add the series to our series collection.
            SeriesCollection = new ChartingCollection<ISeries>();
            SeriesCollection.Add(series1);
            SeriesCollection.Add(series2);

            // we bind the SeriesCollection property to the CartesianChart.Series property in XAML
        }

        public ChartingCollection<ISeries> SeriesCollection { get; set; }
        public ChartingCollection<Plane> YAxis { get; set; }
    }
}