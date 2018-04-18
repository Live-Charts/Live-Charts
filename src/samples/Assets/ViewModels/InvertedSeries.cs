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
    public class InvertedSeries
    {
        public InvertedSeries()
        {
            var values1 = new ObservableCollection<double>();
            values1.Add(5d);
            values1.Add(3d);
            values1.Add(6d);
            values1.Add(4d);

            var values2 = new ObservableCollection<double>();
            values2.Add(7d);
            values2.Add(8d);
            values2.Add(2d);
            values2.Add(8d);

            SeriesCollection = new ChartingCollection<ISeries>();

            SeriesCollection.Add(
                new LineSeries<double>
                {
                    Values = values1
                });

            SeriesCollection.Add(
                new LineSeries<double>
                {
                    Values = values2,
                    DefaultFillOpacity = 0.3f
                });
            // we bind the SeriesCollection property to the CartesianChart.Series property in XAML
        }

        public ChartingCollection<ISeries> SeriesCollection { get; set; }
    }
}
