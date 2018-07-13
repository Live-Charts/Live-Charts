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
    public class BasicPie
    {
        public BasicPie()
        {
            ObservableCollection<double> chromeValues = new ObservableCollection<double>();
            chromeValues.Add(12d);

            ObservableCollection<double> fireFoxValues = new ObservableCollection<double>();
            fireFoxValues.Add(8d);

            ObservableCollection<double> explorerValues = new ObservableCollection<double>();
            explorerValues.Add(6d);

            // some custom style..
            //explorerSeries.CornerRadius = 6;
            //explorerSeries.PushOut = 10;

            // create a collection to store our series.
            SeriesCollection = new ChartingCollection<ISeries>();
            // add the series to our collection
            SeriesCollection.Add(
                new PieSeries<double>
                {
                    Values = chromeValues
                });
            SeriesCollection.Add(
                new PieSeries<double>
                {
                    Values = fireFoxValues
                });
            SeriesCollection.Add(
                new PieSeries<double>
                {
                    Values = explorerValues
                });
            // we bind the SeriesCollection property to the PieChart.Series property in XAML
        }

        public ChartingCollection<ISeries> SeriesCollection { get; set; }
    }
}