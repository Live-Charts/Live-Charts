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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using LiveCharts.Core;
using LiveCharts.Core.Collections;
using LiveCharts.Core.Coordinates;
using LiveCharts.Core.DataSeries;
using LiveCharts.Core.DataSeries.Data;
using LiveCharts.Core.Defaults;
using LiveCharts.Core.Themes;
using LiveCharts.Wpf;
using Point = LiveCharts.Core.Coordinates.Point;

#endregion

namespace Samples.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Charting.Settings(charting =>
            {
                charting
                    .LearnPrimitiveAndDefaultTypes()
                    .SetTheme(Themes.MaterialDesign)
                    .TargetsWpf();
            });

            
            Charting.Settings(charting =>
            {
                charting.LearnType<Student>(
                    (student, index) => new LiveCharts.Core.Coordinates.Point(index, student.Age));
            });

            var chart = new CartesianChart();

            var seriesCollection = new ChartingCollection<Series>();
            chart.Series = seriesCollection;

            var ageSeries = new BarSeries<Student>();

            ageSeries.Add(new Student
            {
                Age = 22,
                Name = "Charles"
            });
            ageSeries.Add(new Student
            {
                Age = 25,
                Name = "Frida"
            });

            seriesCollection.Add(ageSeries);
        }

        public class Student : INotifyPropertyChanged
        {
            private int _age;
            public string Name { get; set; }

            public int Age
            {
                get { return _age; }
                set
                {
                    _age = value;
                    OnPropertyChanged();
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;

            protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
