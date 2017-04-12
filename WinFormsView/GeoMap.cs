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
using LiveCharts.Maps;

namespace LiveCharts.WinForms
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.Windows.Forms.Integration.ElementHost" />
    [Designer("System.Windows.Forms.Design.ControlDesigner, System.Design")]
    [DesignerSerializer("System.ComponentModel.Design.Serialization.TypeCodeDomSerializer , System.Design", "System.ComponentModel.Design.Serialization.CodeDomSerializer, System.Design")]

    public class GeoMap : ElementHost
    {
        /// <summary>
        /// The WPF base
        /// </summary>
        protected readonly Wpf.GeoMap WpfBase = new Wpf.GeoMap();
        /// <summary>
        /// Initializes a new instance of the <see cref="GeoMap"/> class.
        /// </summary>
        public GeoMap()
        {
            Child = WpfBase;

            //workaround for windows 7 focus issue
            //https://github.com/beto-rodriguez/Live-Charts/issues/515
            HostContainer.MouseEnter += (sender, args) =>
            {
                Focus();
            };

            WpfBase.LandClick += (o, point) =>
            {
                if (LandClick != null) LandClick.Invoke(o, point);
            };
        }

        /// <summary>
        /// Occurs when [land click].
        /// </summary>
        public event Action<object, MapData> LandClick;

        /// <summary>
        /// Gets the base.
        /// </summary>
        /// <value>
        /// The base.
        /// </value>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Wpf.GeoMap Base
        {
            get { return WpfBase; }
        }

        /// <summary>
        /// Gets or sets the language pack.
        /// </summary>
        /// <value>
        /// The language pack.
        /// </value>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Dictionary<string, string> LanguagePack
        {
            get { return WpfBase.LanguagePack; }
            set { WpfBase.LanguagePack = value; }
        }

        /// <summary>
        /// Gets or sets the default land fill.
        /// </summary>
        /// <value>
        /// The default land fill.
        /// </value>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Brush DefaultLandFill
        {
            get { return WpfBase.DefaultLandFill; }
            set { WpfBase.DefaultLandFill = value; }
        }

        /// <summary>
        /// Gets or sets the land stroke thickness.
        /// </summary>
        /// <value>
        /// The land stroke thickness.
        /// </value>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public double LandStrokeThickness
        {
            get { return WpfBase.LandStrokeThickness; }
            set { WpfBase.LandStrokeThickness = value; }
        }

        /// <summary>
        /// Gets or sets the land stroke.
        /// </summary>
        /// <value>
        /// The land stroke.
        /// </value>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Brush LandStroke
        {
            get { return WpfBase.LandStroke; }
            set { WpfBase.LandStroke = value; }
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
        /// Gets or sets a value indicating whether this <see cref="GeoMap"/> is hoverable.
        /// </summary>
        /// <value>
        ///   <c>true</c> if hoverable; otherwise, <c>false</c>.
        /// </value>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool Hoverable
        {
            get { return WpfBase.Hoverable; }
            set { WpfBase.Hoverable = value; }
        }

        /// <summary>
        /// Gets or sets the heat map.
        /// </summary>
        /// <value>
        /// The heat map.
        /// </value>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Dictionary<string, double> HeatMap
        {
            get { return WpfBase.HeatMap; }
            set { WpfBase.HeatMap = value; }
        }

        /// <summary>
        /// Gets or sets the gradient stop collection.
        /// </summary>
        /// <value>
        /// The gradient stop collection.
        /// </value>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public GradientStopCollection GradientStopCollection
        {
            get { return WpfBase.GradientStopCollection; }
            set { WpfBase.GradientStopCollection = value; }
        }

        /// <summary>
        /// Gets or sets the source.
        /// </summary>
        /// <value>
        /// The source.
        /// </value>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string Source
        {
            get { return WpfBase.Source; }
            set { WpfBase.Source = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [enable zooming and panning].
        /// </summary>
        /// <value>
        /// <c>true</c> if [enable zooming and panning]; otherwise, <c>false</c>.
        /// </value>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool EnableZoomingAndPanning
        {
            get { return WpfBase.EnableZoomingAndPanning; }
            set { WpfBase.EnableZoomingAndPanning = value; }
        }
    }
}
