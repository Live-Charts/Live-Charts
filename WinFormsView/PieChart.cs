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
        protected readonly Wpf.PieChart WpfBase = new Wpf.PieChart();

        public PieChart()
        {
            Child = WpfBase;
            WpfBase.DataClick += (o, point) =>
            {
                if (DataClick != null) DataClick.Invoke(o, point);
            };
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            {
                WpfBase.Series = WpfBase.GetDesignerModeCollection();
            }
        }

        public event DataClickHandler DataClick;

        #region ChartProperties

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]

        public AxesCollection AxisY
        {
            get { return WpfBase.AxisY; }
            set { WpfBase.AxisY = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]

        public AxesCollection AxisX
        {
            get { return WpfBase.AxisX; }
            set { WpfBase.AxisX = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]

        public UserControl DefaultLegend
        {
            get { return WpfBase.ChartLegend; }
            set { WpfBase.ChartLegend = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]

        public ZoomingOptions Zoom
        {
            get { return WpfBase.Zoom; }
            set { WpfBase.Zoom = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]

        public LegendLocation LegendLocation
        {
            get { return WpfBase.LegendLocation; }
            set { WpfBase.LegendLocation = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public SeriesCollection Series
        {
            get { return WpfBase.Series; }
            set { WpfBase.Series = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public TimeSpan AnimationsSpeed
        {
            get { return WpfBase.AnimationsSpeed; }
            set { WpfBase.AnimationsSpeed = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool DisableAnimations
        {
            get { return WpfBase.DisableAnimations; }
            set { WpfBase.DisableAnimations = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public UserControl DataTooltip
        {
            get { return WpfBase.DataTooltip; }
            set { WpfBase.DataTooltip = value; }
        }
        #endregion

        #region ThisChartProperties

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public double InnerRadius
        {
            get { return WpfBase.InnerRadius; }
            set { WpfBase.InnerRadius = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public double StartingRotationAngle
        {
            get { return WpfBase.StartingRotationAngle; }
            set { WpfBase.StartingRotationAngle = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public UpdaterState UpdaterState
        {
            get { return WpfBase.UpdaterState; }
            set { WpfBase.UpdaterState = value; }
        }
        #endregion

        #region Methods

        public void Update(bool restartView, bool force)
        {
            WpfBase.Update(restartView, force);
        }
        #endregion
    }
}