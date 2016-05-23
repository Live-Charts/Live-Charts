//The MIT License(MIT)

//copyright(c) 2016 Alberto Rodriguez

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
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using LiveCharts;
using LiveCharts.Wpf;

namespace WinFormsView
{
    public class CartesianChart : UserControl
    {
        private readonly LiveCharts.Wpf.CartesianChart _chart = new LiveCharts.Wpf.CartesianChart();
        
        public CartesianChart()
        {
            var eh = new ElementHost
            {
                Dock = DockStyle.Fill,
                Child = new LiveCharts.Wpf.CartesianChart()
            };
            Controls.Add(eh);
        }

        #region ChartProperties

        public List<Axis> AxisY
        {
            get { return _chart.AxisY; }
            set { _chart.AxisY = value; }
        }

        public List<Axis> AxisX
        {
            get { return _chart.AxisX; }
            set { _chart.AxisX = value; }
        }

        public DefaultLegend DefaultLegend
        {
            get { return _chart.ChartLegend; }
            set { _chart.ChartLegend = value; }
        }

        public ZoomingOptions Zoom
        {
            get { return _chart.Zoom; }
            set { _chart.Zoom = value; }
        }

        public LegendLocation LegendLocation
        {
            get { return _chart.LegendLocation; }
            set { _chart.LegendLocation = value; }
        }


        public SeriesCollection Series
        {
            get { return _chart.Series; }
            set { _chart.Series = value; }
        }

        public TimeSpan AnimationsSpeed
        {
            get { return _chart.AnimationsSpeed; }
            set { _chart.AnimationsSpeed = value; }
        }

        public bool DisableAnimations
        {
            get { return _chart.DisableAnimations; }
            set { _chart.DisableAnimations = value; }
        }

        public DefaultTooltip DataTooltip
        {
            get { return _chart.DataTooltip; }
            set { _chart.DataTooltip = value; }
        }
        #endregion

        #region ThisChartProperties

        #endregion
    }
}