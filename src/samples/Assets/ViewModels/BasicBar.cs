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
using LiveCharts.Core.DataSeries;

#endregion

namespace Assets.ViewModels
{
    public class BasicBar
    {
        public BasicBar()
        {
            var values = new ObservableCollection<double>();

            // add some values...
            values.Add(4);
            values.Add(7);
            values.Add(2);
            values.Add(9);
            values.Add(4);
            values.Add(7);
            values.Add(2);
            values.Add(9);
            values.Add(4);
            values.Add(7);
            values.Add(2);
            values.Add(9);
            values.Add(4);
            values.Add(7);
            values.Add(2);
            values.Add(9);
            values.Add(4);
            values.Add(7);
            values.Add(2);
            values.Add(9);
            values.Add(4);
            values.Add(7);
            values.Add(2);
            values.Add(9);
            values.Add(4);
            values.Add(7);
            values.Add(2);
            values.Add(9);
            values.Add(4);
            values.Add(7);
            values.Add(2);
            values.Add(9);
            values.Add(4);
            values.Add(7);
            values.Add(2);
            values.Add(9);
            values.Add(4);
            values.Add(7);
            values.Add(2);
            values.Add(9);

            var barSeries = new BarSeries<double>();

            barSeries.Values = values;

            // a custom fill and stroke, if we don't set these properties
            // LiveCharts will set them for us according to our theme.
            barSeries.StrokeThickness = 3f;
            barSeries.Stroke = LiveCharts.Core.Drawing.Brushes.Purple;
            barSeries.Fill = LiveCharts.Core.Drawing.Brushes.MediumPurple;

            // limit the column width to 65.
            barSeries.MaxColumnWidth = 65f;

            // do not display a label for every point
            barSeries.DataLabels = false;

            // create a collection to store our series.
            // you can use any IEnumerable, it is recommended to use
            // the ChartingCollection<T> class, it inherits from
            // ObservableCollection and adds the AddRange/RemoveRange methods
            SeriesCollection = new ObservableCollection<ISeries>();
            // finally lets add the series to our series collection,
            // we can add as many series as we need, in this case we will
            // only display one series.
            SeriesCollection.Add(barSeries);
        }

        public ObservableCollection<ISeries> SeriesCollection { get; set; }
    }
}