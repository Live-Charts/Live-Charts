//The MIT License(MIT)

//Copyright(c) 2016 Alberto Rodriguez & LiveCharts Contributors

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
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.Windows.Forms.Integration.ElementHost" />
    [Designer("System.Windows.Forms.Design.ControlDesigner, System.Design")]
    [DesignerSerializer("System.ComponentModel.Design.Serialization.TypeCodeDomSerializer , System.Design", "System.ComponentModel.Design.Serialization.CodeDomSerializer, System.Design")]

    public class AngularGauge : ElementHost
    {
        /// <summary>
        /// The WPF base
        /// </summary>
        protected readonly Wpf.AngularGauge WpfBase = new Wpf.AngularGauge();

        /// <summary>
        /// Initializes a new instance of the <see cref="AngularGauge"/> class.
        /// </summary>
        public AngularGauge()
        {
            Child = WpfBase;

            //workaround for windows 7 focus issue
            //https://github.com/beto-rodriguez/Live-Charts/issues/515
            HostContainer.MouseEnter += (sender, args) =>
            {
                Focus();
            };
        }

        /// <summary>
        /// Gets the base.
        /// </summary>
        /// <value>
        /// The base.
        /// </value>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Wpf.AngularGauge Base
        {
            get { return WpfBase; }
        }

        /// <summary>
        /// Gets or sets the wedge.
        /// </summary>
        /// <value>
        /// The wedge.
        /// </value>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public double Wedge
        {
            get { return WpfBase.Wedge; }
            set { WpfBase.Wedge = value; }
        }

        /// <summary>
        /// Gets or sets the tick step.
        /// </summary>
        /// <value>
        /// The tick step.
        /// </value>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public double TickStep
        {
            get { return WpfBase.TicksStep; }
            set { WpfBase.TicksStep = value; }
        }

        /// <summary>
        /// Gets or sets the labels step.
        /// </summary>
        /// <value>
        /// The labels step.
        /// </value>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public double LabelsStep
        {
            get { return WpfBase.LabelsStep; }
            set { WpfBase.LabelsStep = value; }
        }

        /// <summary>
        /// Gets or sets from value.
        /// </summary>
        /// <value>
        /// From value.
        /// </value>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public double FromValue
        {
            get { return WpfBase.FromValue; }
            set { WpfBase.FromValue = value; }
        }

        /// <summary>
        /// Gets or sets to value.
        /// </summary>
        /// <value>
        /// To value.
        /// </value>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public double ToValue
        {
            get { return WpfBase.ToValue; }
            set { WpfBase.ToValue = value; }
        }

        /// <summary>
        /// Gets or sets the sections.
        /// </summary>
        /// <value>
        /// The sections.
        /// </value>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<AngularSection> Sections
        {
            get { return WpfBase.Sections; }
            set { WpfBase.Sections = value; }
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public double Value
        {
            get { return WpfBase.Value; }
            set { WpfBase.Value = value; }
        }

        /// <summary>
        /// Gets or sets the label formatter.
        /// </summary>
        /// <value>
        /// The label formatter.
        /// </value>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Func<double, string> LabelFormatter
        {
            get { return WpfBase.LabelFormatter; }
            set { WpfBase.LabelFormatter = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [disable animations].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [disable animations]; otherwise, <c>false</c>.
        /// </value>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool DisableAnimations
        {
            get { return WpfBase.DisableaAnimations; }
            set { WpfBase.DisableaAnimations = value; }
        }

        /// <summary>
        /// Gets or sets the animations speed.
        /// </summary>
        /// <value>
        /// The animations speed.
        /// </value>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public TimeSpan AnimationsSpeed
        {
            get { return WpfBase.AnimationsSpeed; }
            set { WpfBase.AnimationsSpeed = value; }
        }

        /// <summary>
        /// Gets or sets the ticks foreground.
        /// </summary>
        /// <value>
        /// The ticks foreground.
        /// </value>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Brush TicksForeground
        {
            get { return WpfBase.TicksForeground; }
            set { WpfBase.TicksForeground = value; }
        }

        /// <summary>
        /// Gets or sets the sections inner radius.
        /// </summary>
        /// <value>
        /// The sections inner radius.
        /// </value>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public double SectionsInnerRadius
        {
            get { return WpfBase.SectionsInnerRadius; }
            set { WpfBase.SectionsInnerRadius = value; }
        }

        /// <summary>
        /// Gets or sets the needle fill.
        /// </summary>
        /// <value>
        /// The needle fill.
        /// </value>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Brush NeedleFill
        {
            get { return WpfBase.NeedleFill; }
            set { WpfBase.NeedleFill = value; }
        }

        /// <summary>
        /// Gets or sets the labels effect.
        /// </summary>
        /// <value>
        /// The labels effect.
        /// </value>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Effect LabelsEffect
        {
            get { return WpfBase.LabelsEffect; }
            set { WpfBase.LabelsEffect = value; }
        }

        /// <summary>
        /// Gets or sets the ticks stroke thickness.
        /// </summary>
        /// <value>
        /// The ticks stroke thickness.
        /// </value>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public double TicksStrokeThickness
        {
            get { return WpfBase.TicksStrokeThickness; }
            set { WpfBase.TicksStrokeThickness = value; }
        }
    }
}
