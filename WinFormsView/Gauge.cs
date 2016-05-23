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
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.Windows.Media;

namespace WinFormsView
{
    public class Gauge : UserControl
    {
        private readonly LiveCharts.Wpf.Gauge _gauge = new LiveCharts.Wpf.Gauge();

        public Gauge()
        {
            var eh = new ElementHost
            {
                Dock = DockStyle.Fill,
                Child = _gauge
            };
            Controls.Add(eh);
        }

        public bool Uses360Mode
        {
            get { return _gauge.Uses360Mode; }
            set { _gauge.Uses360Mode = value; }
        }

        public double From
        {
            get { return _gauge.From; }
            set { _gauge.From = value; }
        }

        public double To
        {
            get { return _gauge.To; }
            set { _gauge.To = value; }
        }

        public double Value
        {
            get { return _gauge.Value; }
            set { _gauge.Value = value; }
        }

        public string Title
        {
            get { return _gauge.Title; }
            set { _gauge.Title = value; }
        }

        public double? InnerRadius
        {
            get { return _gauge.InnerRadius; }
            set { _gauge.InnerRadius = value; }
        }

        public Brush Stroke
        {
            get { return _gauge.Stroke; }
            set { _gauge.Stroke = value; }
        }

        public double StrokeThickness
        {
            get { return _gauge.StrokeThickness; }
            set { _gauge.StrokeThickness = value; }
        }

        public Color ToColor
        {
            get { return _gauge.ToColor; }
            set { _gauge.ToColor = value; }
        }

        public Color FromColor
        {
            get { return _gauge.FromColor; }
            set { _gauge.FromColor = value; }
        }

        public Brush GaugeBackground
        {
            get { return _gauge.GaugeBackground; }
            set { _gauge.GaugeBackground = value; }
        }

        public TimeSpan AnimationsSpeed
        {
            get { return _gauge.AnimationsSpeed; }
            set { _gauge.AnimationsSpeed = value; }
        }

        public Func<double, string> LabelFormatter
        {
            get { return _gauge.LabelFormatter; }
            set { _gauge.LabelFormatter = value; }
        }

        public double? HighFontSize
        {
            get { return _gauge.HighFontSize; }
            set { _gauge.HighFontSize = value; }
        }
    }
}