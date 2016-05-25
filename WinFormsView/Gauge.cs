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
using System.Windows.Forms.Integration;
using System.Windows.Media;

namespace LiveCharts.WinForms
{
    [Designer("System.Windows.Forms.Design.ControlDesigner, System.Design")]
    [DesignerSerializer("System.ComponentModel.Design.Serialization.TypeCodeDomSerializer , System.Design", "System.ComponentModel.Design.Serialization.CodeDomSerializer, System.Design")]

    public class Gauge : ElementHost
    {
        protected readonly Wpf.Gauge WpfBase = new Wpf.Gauge();

        public Gauge()
        {
            Child = WpfBase;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool Uses360Mode
        {
            get { return WpfBase.Uses360Mode; }
            set { WpfBase.Uses360Mode = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public double From
        {
            get { return WpfBase.From; }
            set { WpfBase.From = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public double To
        {
            get { return WpfBase.To; }
            set { WpfBase.To = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public double Value
        {
            get { return WpfBase.Value; }
            set { WpfBase.Value = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string Title
        {
            get { return WpfBase.Title; }
            set { WpfBase.Title = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public double? InnerRadius
        {
            get { return WpfBase.InnerRadius; }
            set { WpfBase.InnerRadius = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Brush Stroke
        {
            get { return WpfBase.Stroke; }
            set { WpfBase.Stroke = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public double StrokeThickness
        {
            get { return WpfBase.StrokeThickness; }
            set { WpfBase.StrokeThickness = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Color ToColor
        {
            get { return WpfBase.ToColor; }
            set { WpfBase.ToColor = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Color FromColor
        {
            get { return WpfBase.FromColor; }
            set { WpfBase.FromColor = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Brush GaugeBackground
        {
            get { return WpfBase.GaugeBackground; }
            set { WpfBase.GaugeBackground = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public TimeSpan AnimationsSpeed
        {
            get { return WpfBase.AnimationsSpeed; }
            set { WpfBase.AnimationsSpeed = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Func<double, string> LabelFormatter
        {
            get { return WpfBase.LabelFormatter; }
            set { WpfBase.LabelFormatter = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public double? HighFontSize
        {
            get { return WpfBase.HighFontSize; }
            set { WpfBase.HighFontSize = value; }
        }
    }
}