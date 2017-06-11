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
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Windows;
using System.Windows.Forms.Integration;
using System.Windows.Media;

namespace LiveCharts.WinForms
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.Windows.Forms.Integration.ElementHost" />
    [Designer("System.Windows.Forms.Design.ControlDesigner, System.Design")]
    [DesignerSerializer("System.ComponentModel.Design.Serialization.TypeCodeDomSerializer , System.Design", "System.ComponentModel.Design.Serialization.CodeDomSerializer, System.Design")]

    public class SolidGauge : ElementHost
    {
        /// <summary>
        /// The WPF base
        /// </summary>
        protected readonly Wpf.Gauge WpfBase = new Wpf.Gauge();

        /// <summary>
        /// Initializes a new instance of the <see cref="SolidGauge"/> class.
        /// </summary>
        public SolidGauge()
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
        public Wpf.Gauge Base
        {
            get { return WpfBase; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [uses360 mode].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [uses360 mode]; otherwise, <c>false</c>.
        /// </value>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool Uses360Mode
        {
            get { return WpfBase.Uses360Mode; }
            set { WpfBase.Uses360Mode = value; }
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
            get { return WpfBase.DisableAnimations; }
            set { WpfBase.DisableAnimations = value; }
        }

        /// <summary>
        /// Gets or sets from.
        /// </summary>
        /// <value>
        /// From.
        /// </value>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public double From
        {
            get { return WpfBase.From; }
            set { WpfBase.From = value; }
        }

        /// <summary>
        /// Gets or sets to.
        /// </summary>
        /// <value>
        /// To.
        /// </value>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public double To
        {
            get { return WpfBase.To; }
            set { WpfBase.To = value; }
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
        /// Gets or sets the inner radius.
        /// </summary>
        /// <value>
        /// The inner radius.
        /// </value>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public double? InnerRadius
        {
            get { return WpfBase.InnerRadius; }
            set { WpfBase.InnerRadius = value; }
        }

        /// <summary>
        /// Gets or sets the stroke.
        /// </summary>
        /// <value>
        /// The stroke.
        /// </value>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Brush Stroke
        {
            get { return WpfBase.Stroke; }
            set { WpfBase.Stroke = value; }
        }

        /// <summary>
        /// Gets or sets the stroke thickness.
        /// </summary>
        /// <value>
        /// The stroke thickness.
        /// </value>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public double StrokeThickness
        {
            get { return WpfBase.StrokeThickness; }
            set { WpfBase.StrokeThickness = value; }
        }

        /// <summary>
        /// Gets or sets to color.
        /// </summary>
        /// <value>
        /// To color.
        /// </value>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Color ToColor
        {
            get { return WpfBase.ToColor; }
            set { WpfBase.ToColor = value; }
        }

        /// <summary>
        /// Gets or sets from color.
        /// </summary>
        /// <value>
        /// From color.
        /// </value>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Color FromColor
        {
            get { return WpfBase.FromColor; }
            set { WpfBase.FromColor = value; }
        }

        /// <summary>
        /// Gets or sets the gauge background.
        /// </summary>
        /// <value>
        /// The gauge background.
        /// </value>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Brush GaugeBackground
        {
            get { return WpfBase.GaugeBackground; }
            set { WpfBase.GaugeBackground = value; }
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
        /// Gets or sets the size of the high font.
        /// </summary>
        /// <value>
        /// The size of the high font.
        /// </value>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public double? HighFontSize
        {
            get { return WpfBase.HighFontSize; }
            set { WpfBase.HighFontSize = value; }
        }

        /// <summary>
        /// Gets or sets the size of the font.
        /// </summary>
        /// <value>
        /// The size of the font.
        /// </value>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public double FontSize
        {
            get { return WpfBase.FontSize; }
            set { WpfBase.FontSize = value; }
        }

        /// <summary>
        /// Gets or sets the font family.
        /// </summary>
        /// <value>
        /// The font family.
        /// </value>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public FontFamily FontFamily
        {
            get { return WpfBase.FontFamily; }
            set { WpfBase.FontFamily = value; }
        }

        /// <summary>
        /// Gets or sets the font weight.
        /// </summary>
        /// <value>
        /// The font weight.
        /// </value>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public FontWeight FontWeight
        {
            get { return WpfBase.FontWeight; }
            set { WpfBase.FontWeight = value; }
        }

        /// <summary>
        /// Gets or sets the font style.
        /// </summary>
        /// <value>
        /// The font style.
        /// </value>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public FontStyle FontStyle
        {
            get { return WpfBase.FontStyle; }
            set { WpfBase.FontStyle = value; }
        }

        /// <summary>
        /// Gets or sets the font stretch.
        /// </summary>
        /// <value>
        /// The font stretch.
        /// </value>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public FontStretch FontStretch
        {
            get { return WpfBase.FontStretch; }
            set { WpfBase.FontStretch = value; }
        }

        /// <summary>
        /// Gets or sets the fore ground.
        /// </summary>
        /// <value>
        /// The fore ground.
        /// </value>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Brush ForeGround
        {
            get { return WpfBase.Foreground; }
            set { WpfBase.Foreground = value; }
        }
    }
}