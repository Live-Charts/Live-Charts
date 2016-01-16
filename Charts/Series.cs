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
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using lvc.Charts;
using lvc.TypeConverters;

namespace lvc
{
	public abstract class Series : FrameworkElement
	{
		protected List<Shape> Shapes = new List<Shape>();
	    private Chart _chart;
	    internal bool RequiresAnimation;
	    internal bool RequiresPlot;

        #region Dependency Properties
	    public static readonly DependencyProperty ValuesProperty = DependencyProperty.Register(
	        "Values", typeof (IChartValues), typeof (Series), new PropertyMetadata(default(IChartValues), ValuesCallBack));

        [TypeConverter(typeof(IndexedChartValuesConverter))]
        public IChartValues Values
	    {
	        get
	        {
	            var values = (IChartValues) GetValue(ValuesProperty);
#if DEBUG
                if (DesignerProperties.GetIsInDesignMode(this))
                    if (values == null) values = new IndexedChartValues();
#endif
                return values;
	        }
	        set { SetValue(ValuesProperty, value); }
	    }

        public static readonly DependencyProperty TitleProperty =
           DependencyProperty.Register("Title", typeof(string), typeof(Series), new PropertyMetadata("An Unnamed Serie"));
        /// <summary>
        /// Gets or sets serie name
        /// </summary>
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set
            {
                SetValue(TitleProperty, value);
            }
        }

		public static readonly DependencyProperty StrokeProperty =
			DependencyProperty.Register("Stroke", typeof(Brush), typeof(Series), new PropertyMetadata(null));

		public Brush Stroke
		{
		    get { return ((Brush) GetValue(StrokeProperty)); }
			set { SetValue(StrokeProperty, value); }
		}

		public static readonly DependencyProperty FillProperty =
			DependencyProperty.Register("Fill", typeof(Brush), typeof(Series), new PropertyMetadata(null));

		public Brush Fill
		{
			get
			{
			    return (Brush) GetValue(FillProperty);
			}
			set { SetValue(FillProperty, value); }
		}
		#endregion

		#region Properties
        public Chart Chart
        {
            get { return _chart; }
            internal set
            {
                //if (_chart != null) throw new InvalidOperationException("Can't set chart property twice.");
                _chart = value;
            }
        }
        #endregion

        #region PublicMethods
        public static Color GetColorByIndex(int index)
        {
            return Chart.Colors[(int) (index - Chart.Colors.Count*Math.Truncate(index/(decimal) Chart.Colors.Count))];
        }
        #endregion

        #region Abstracts
        public abstract void Plot(bool animate = true);
        #endregion

        #region PrivateMethods
        private static void ValuesCallBack(DependencyObject o, DependencyPropertyChangedEventArgs args)
        {
            var series = o as Series;
            if (series == null) return;

            var observable = series.Values as INotifyCollectionChanged;
            if (observable == null) return;
            if (series.Chart == null) return;

            observable.CollectionChanged += series.Chart.OnDataSeriesChanged;
        }
        #endregion

        #region Virtual Methods
        public virtual void Erase()
        {
            foreach (var s in Shapes)
                Chart.Canvas.Children.Remove(s);
            Shapes.Clear();

            var hoverableShapes = Chart.HoverableShapes.Where(x => x.Series == this).ToList();
            foreach (var hs in hoverableShapes)
            {
                Chart.Canvas.Children.Remove(hs.Shape);
                Chart.HoverableShapes.Remove(hs);
            }
        }
        #endregion

        #region ProtectedMethods
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
        #endregion
    }
}
