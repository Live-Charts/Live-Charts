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
using LiveCharts.Core.Defaults;
using LiveCharts.Core.Drawing;

#endregion

namespace Assets.ViewModels
{
    public class BasicBubble
    {
        public BasicBubble()
        {
            var values = new ObservableCollection<WeightedModel>();

            // add some values to the series...
            //                              x, y, weight
            values.Add(new WeightedModel(0, 4, 23));
            values.Add(new WeightedModel(3, 2, 55));
            values.Add(new WeightedModel(2, 0, 17));
            values.Add(new WeightedModel(1, 3, 46));

            // we added 4 points
            // the first point has an X eq to 0, a Y eq to 4 and
            // a weight eq to 23
            // every bubble size will be scaled according to its weight
            // in our case 55 will be the bigger bubble, and 17 the smaller
            // now lets set the max and min size for our bubbles

            var bubbleSeries = new BubbleSeries<WeightedModel>();

            bubbleSeries.Values = values;

            bubbleSeries.MaxGeometrySize = 400; // when w = 55, our geometry size will be 400
            bubbleSeries.MinGeometrySize = 150;  // when w = 17 our geometry size will be 150

            // lets set a custom geometry, a square
            bubbleSeries.Geometry = Geometry.Square;

            // create a collection to store our series.
            // you can use any IEnumerable, it is recommended to use
            // the ChartingCollection<T> class, it inherits from
            // ObservableCollection and adds the AddRange/RemoveRange methods
            SeriesCollection = new ChartingCollection<ISeries>();
            // finally lets add the series to our series collection,
            // we can add as many series as we need, in this case we will
            // only display one series.
            SeriesCollection.Add(bubbleSeries);
            // we bind the SeriesCollection property to the CartesianChart.Series property in XAML
        }

        public ChartingCollection<ISeries> SeriesCollection { get; set; }
    }
}