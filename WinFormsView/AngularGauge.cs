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
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Windows.Forms.Integration;
using System.Windows.Media;
using System.Windows.Media.Effects;
using LiveCharts.Wpf;

namespace LiveCharts.WinForms
{
    [Designer("System.Windows.Forms.Design.ControlDesigner, System.Design")]
    [DesignerSerializer("System.ComponentModel.Design.Serialization.TypeCodeDomSerializer , System.Design", "System.ComponentModel.Design.Serialization.CodeDomSerializer, System.Design")]

    public class AngularGauge : ElementHost
    {
        protected readonly Wpf.AngularGauge WpfBase = new Wpf.AngularGauge();

        public AngularGauge()
        {
            Child = WpfBase;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Wpf.AngularGauge Base
        {
            get { return WpfBase; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public double Wedge
        {
            get { return WpfBase.Wedge; }
            set { WpfBase.Wedge = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public double TickStep
        {
            get { return WpfBase.TicksStep; }
            set { WpfBase.TicksStep = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public double LabelsStep
        {
            get { return WpfBase.LabelsStep; }
            set { WpfBase.LabelsStep = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public double FromValue
        {
            get { return WpfBase.FromValue; }
            set { WpfBase.FromValue = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public double ToValue
        {
            get { return WpfBase.ToValue; }
            set { WpfBase.ToValue = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<AngularSection> Sections
        {
            get { return WpfBase.Sections; }
            set { WpfBase.Sections = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public double Value
        {
            get { return WpfBase.Value; }
            set { WpfBase.Value = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Func<double, string> LabelFormatter
        {
            get { return WpfBase.LabelFormatter; }
            set { WpfBase.LabelFormatter = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool DisableAnimations
        {
            get { return WpfBase.DisableaAnimations; }
            set { WpfBase.DisableaAnimations = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public TimeSpan AnimationsSpeed
        {
            get { return WpfBase.AnimationsSpeed; }
            set { WpfBase.AnimationsSpeed = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Brush TicksForeground
        {
            get { return WpfBase.TicksForeground; }
            set { WpfBase.TicksForeground = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public double SectionsInnerRadius
        {
            get { return WpfBase.SectionsInnerRadius; }
            set { WpfBase.SectionsInnerRadius = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Brush NeedleFill
        {
            get { return WpfBase.NeedleFill; }
            set { WpfBase.NeedleFill = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Effect LabelsEffect
        {
            get { return WpfBase.LabelsEffect; }
            set { WpfBase.LabelsEffect = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public double TicksStrokeThickness
        {
            get { return WpfBase.TicksStrokeThickness; }
            set { WpfBase.TicksStrokeThickness = value; }
        }
    }
}
