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
using LiveCharts.Maps;

namespace LiveCharts.WinForms
{
    [Designer("System.Windows.Forms.Design.ControlDesigner, System.Design")]
    [DesignerSerializer("System.ComponentModel.Design.Serialization.TypeCodeDomSerializer , System.Design", "System.ComponentModel.Design.Serialization.CodeDomSerializer, System.Design")]

    public class GeoMap : ElementHost
    {
        protected readonly Wpf.GeoMap WpfBase = new Wpf.GeoMap();
        public GeoMap()
        {
            Child = WpfBase;

            WpfBase.LandClick += (o, point) =>
            {
                if (LandClick != null) LandClick.Invoke(o, point);
            };
        }

        public event Action<object, MapData> LandClick;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Wpf.GeoMap Base
        {
            get { return WpfBase; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Dictionary<string, string> LanguagePack
        {
            get { return WpfBase.LanguagePack; }
            set { WpfBase.LanguagePack = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Brush DefaultLandFill
        {
            get { return WpfBase.DefaultLandFill; }
            set { WpfBase.DefaultLandFill = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public double LandStrokeThickness
        {
            get { return WpfBase.LandStrokeThickness; }
            set { WpfBase.LandStrokeThickness = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Brush LandStroke
        {
            get { return WpfBase.LandStroke; }
            set { WpfBase.LandStroke = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool DisableAnimations
        {
            get { return WpfBase.DisableAnimations; }
            set { WpfBase.DisableAnimations = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public TimeSpan AnimationsSpeed
        {
            get { return WpfBase.AnimationsSpeed; }
            set { WpfBase.AnimationsSpeed = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool Hoverable
        {
            get { return WpfBase.Hoverable; }
            set { WpfBase.Hoverable = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Dictionary<string, double> HeatMap
        {
            get { return WpfBase.HeatMap; }
            set { WpfBase.HeatMap = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public GradientStopCollection GradientStopCollection
        {
            get { return WpfBase.GradientStopCollection; }
            set { WpfBase.GradientStopCollection = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string Source
        {
            get { return WpfBase.Source; }
            set { WpfBase.Source = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool EnableZoomingAndPanning
        {
            get { return WpfBase.EnableZoomingAndPanning; }
            set { WpfBase.EnableZoomingAndPanning = value; }
        }
    }
}
