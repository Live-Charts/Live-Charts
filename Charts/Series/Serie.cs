//The MIT License(MIT)

//Copyright(c) 2015 Alberto Rodriguez

//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using LiveCharts.Charts;

namespace LiveCharts.Series
{
    public abstract class Serie : DependencyObject
    {
        private Color? _color;
        protected List<Shape> Shapes = new List<Shape>();
        private Chart _chart;
        private int _colorId;

        protected Serie()
        {
            StrokeThickness = 2.5;
            PointRadius = 4;
            ColorId = -1;
            Name = "An Unnamed Serie";
        }

        abstract public ObservableCollection<double> PrimaryValues { get; set; }
        public abstract void Plot(bool animate = true);
        public virtual void Erase()
        {
            foreach (var s in Shapes)
                Chart.Canvas.Children.Remove(s);
            Shapes.Clear();

            var hoverableShapes = Chart.HoverableShapes.Where(x => x.Serie == this).ToList();
            foreach (var hs in hoverableShapes)
            {
                Chart.Canvas.Children.Remove(hs.Shape);
                Chart.HoverableShapes.Remove(hs);
            }
        }

        public static readonly DependencyProperty NameProperty = DependencyProperty.Register(
            "Name", typeof (string), typeof (Serie), new PropertyMetadata(default(string)));
        /// <summary>
        /// Gets or sets serie name
        /// </summary>
        public string Name
        {
            get { return (string) GetValue(NameProperty); }
            set { SetValue(NameProperty, value); }
        }

        public Chart Chart
        {
            get { return _chart; }
            set
            {
                if (_chart != null) throw new Exception("can't set chart property twice.");
                _chart = value;
            }
        }

        public double StrokeThickness { get; set; }
        public double PointRadius { get; set; }
        public int ColorId
        {
            get { return _colorId + Chart.ColorStartIndex; }
            set { _colorId = value; }
        }
        public Color Color
        {
            get
            {
                if (_color != null) return _color.Value;
                return Chart.Colors[
                    (int) (ColorId - Chart.Colors.Count*Math.Truncate(ColorId/(decimal) Chart.Colors.Count))];
            }
            set { _color = value; }
        }

        protected Color GetColorByIndex(int index)
        {
            return Chart.Colors[
                    (int)(index - Chart.Colors.Count * Math.Truncate(index / (decimal)Chart.Colors.Count))];
        }

        /// <summary>
        /// Scales a graph value to screen according to an axis. 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="axis"></param>
        /// <returns></returns>
        protected double ToPlotArea(double value, AxisTags axis)
        {
            return Methods.ToPlotArea(value, axis, Chart);
        }
        /// <summary>
        /// Scales a graph point to screen.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected Point ToPlotArea(Point value)
        {
            return new Point(ToPlotArea(value.X, AxisTags.X), ToPlotArea(value.Y, AxisTags.Y));
        }
    }
}