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
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Windows.Controls;
using System.Windows.Forms.Integration;
using LiveCharts.Events;
using LiveCharts.Wpf;

namespace LiveCharts.WinForms
{
    [Designer("System.Windows.Forms.Design.ControlDesigner, System.Design")]
    [DesignerSerializer("System.ComponentModel.Design.Serialization.TypeCodeDomSerializer , System.Design", "System.ComponentModel.Design.Serialization.CodeDomSerializer, System.Design")]
    public class PieChart : ElementHost
    {
        protected readonly Wpf.PieChart Chart = new Wpf.PieChart();

        public PieChart()
        {
            Child = Chart;
            Chart.DataClick += (o, point) =>
            {
                if (DataClick != null) DataClick.Invoke(o, point);
            };
        }

        public event DataClickHandler DataClick;

        #region ChartProperties

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]

        public AxesCollection AxisY
        {
            get { return Chart.AxisY; }
            set { Chart.AxisY = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]

        public AxesCollection AxisX
        {
            get { return Chart.AxisX; }
            set { Chart.AxisX = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]

        public UserControl DefaultLegend
        {
            get { return Chart.ChartLegend; }
            set { Chart.ChartLegend = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]

        public ZoomingOptions Zoom
        {
            get { return Chart.Zoom; }
            set { Chart.Zoom = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]

        public LegendLocation LegendLocation
        {
            get { return Chart.LegendLocation; }
            set { Chart.LegendLocation = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public SeriesCollection Series
        {
            get { return Chart.Series; }
            set { Chart.Series = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public TimeSpan AnimationsSpeed
        {
            get { return Chart.AnimationsSpeed; }
            set { Chart.AnimationsSpeed = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool DisableAnimations
        {
            get { return Chart.DisableAnimations; }
            set { Chart.DisableAnimations = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public UserControl DataTooltip
        {
            get { return Chart.DataTooltip; }
            set { Chart.DataTooltip = value; }
        }
        #endregion

        #region ThisChartProperties

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public double InnerRadius
        {
            get { return Chart.InnerRadius; }
            set { Chart.InnerRadius = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public double StartingRotationAngle
        {
            get { return Chart.StartingRotationAngle; }
            set { Chart.StartingRotationAngle = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public UpdaterState UpdaterState
        {
            get { return Chart.UpdaterState; }
            set { Chart.UpdaterState = value; }
        }
        #endregion
    }
}