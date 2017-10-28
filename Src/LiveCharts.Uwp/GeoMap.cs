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
using System.Globalization;
using System.Linq;
using Windows.Devices.Input;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using LiveCharts.Maps;
using LiveCharts.Uwp.Components;
using LiveCharts.Uwp.Components.MultiBinding;
using LiveCharts.Uwp.Maps;
using Microsoft.Xaml.Interactivity;
using Path = Windows.UI.Xaml.Shapes.Path;

namespace LiveCharts.Uwp
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Windows.UI.Xaml.Controls.UserControl" />
    public class GeoMap : UserControl
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="GeoMap"/> class.
        /// </summary>
        public GeoMap()
        {
            Canvas = new Canvas();
            Map = new Canvas();
            
            Canvas.Children.Add(Map);
            Content = Canvas;

            Canvas.SetBinding(WidthProperty,
                new Binding { Path = new PropertyPath("ActualWidth"), Source = this });
            Canvas.SetBinding(HeightProperty,
                new Binding { Path = new PropertyPath("ActualHeight"), Source = this });

            Lands = new Dictionary<string, MapData>();

            this.SetIfNotSet(BackgroundProperty, new SolidColorBrush(Color.FromArgb(150, 96, 125, 138)));
            /*Current*/
            SetValue(GradientStopCollectionProperty, new GradientStopCollection
            {
                new GradientStop()
                {
                    Color = Color.FromArgb(100, 2, 119, 188),
                    Offset = 0d
                },
                new GradientStop()
                {
                    Color = Color.FromArgb(255, 2, 119, 188),
                    Offset = 1d
                },
            });
            this.SetIfNotSet(HeatMapProperty, new Dictionary<string, double>());
            /*Current*/
            SetValue(GeoMapTooltipProperty, new DefaultGeoMapTooltip {Visibility = Visibility.Collapsed}); //Visibility.Hidden});
            Canvas.Children.Add(GeoMapTooltip);

            SizeChanged += (sender, e) =>
            {
                Draw();
            };

            //MouseWheel += (sender, e) =>
            //{
            //    if (!EnableZoomingAndPanning) return;

            //    e.Handled = true;
            //    var rt = Map.RenderTransform as ScaleTransform;
            //    var p = rt == null ? 1 : rt.ScaleX;
            //    p += e.Delta > 0 ? .05 : -.05;
            //    p = p < 1 ? 1 : p;
            //    var o = e.GetPosition(this);
            //    if (e.Delta > 0) Map.RenderTransformOrigin = new Point(o.X/ActualWidth,o.Y/ActualHeight);
            //    Map.RenderTransform = new ScaleTransform(p, p);
            //};

            //MouseDown += (sender, e) =>
            //{
            //    if (!EnableZoomingAndPanning) return;

            //    DragOrigin = e.GetPosition(this);
            //};

            //MouseUp += (sender, e) =>
            //{
            //    if (!EnableZoomingAndPanning) return;

            //    var end = e.GetPosition(this);
            //    var delta = new Point(DragOrigin.X - end.X, DragOrigin.Y - end.Y);

            //    var l = Canvas.GetLeft(Map) - delta.X;
            //    var t = Canvas.GetTop(Map) - delta.Y;

            //    if (DisableAnimations)
            //    {
            //        Canvas.SetLeft(Map, l);
            //        Canvas.SetTop(Map, t);
            //    }
            //    else
            //    {
            //        Map.CreateCanvasStoryBoardAndBegin(l, t, AnimationsSpeed);
            //    }
            //};
        }

        #endregion

        #region Events

        /// <summary>
        /// Occurs when [land click].
        /// </summary>
        public event Action<object, MapData> LandClick;

        #endregion

        #region Properties

        private Canvas Canvas { get; }
        private Canvas Map { get; }
        private Point DragOrigin { get; set; }
        private Point OriginalPosition { get; set; }
        private bool IsDrawn { get; set; }
        private bool IsWidthDominant { get; set; }
        private Dictionary<string, MapData> Lands { get; }

        private static readonly DependencyProperty GeoMapTooltipProperty = DependencyProperty.Register(
            "GeoMapTooltip", typeof (DefaultGeoMapTooltip), typeof (GeoMap), new PropertyMetadata(default(DefaultGeoMapTooltip)));
        private DefaultGeoMapTooltip GeoMapTooltip
        {
            get { return (DefaultGeoMapTooltip) GetValue(GeoMapTooltipProperty); }
            set { SetValue(GeoMapTooltipProperty, value); }
        }

        /// <summary>
        /// The language pack property
        /// </summary>
        public static readonly DependencyProperty LanguagePackProperty = DependencyProperty.Register(
            "LanguagePack", typeof (Dictionary<string, string>), typeof (GeoMap), new PropertyMetadata(default(Dictionary<string, string>)));
        /// <summary>
        /// Gets or sets the language dictionary
        /// </summary>
        public Dictionary<string, string> LanguagePack
        {
            get { return (Dictionary<string, string>) GetValue(LanguagePackProperty); }
            set { SetValue(LanguagePackProperty, value); }
        }

        /// <summary>
        /// The default land fill property
        /// </summary>
        public static readonly DependencyProperty DefaultLandFillProperty = DependencyProperty.Register(
            "DefaultLandFill", typeof (Brush), typeof (GeoMap), new PropertyMetadata(new SolidColorBrush(Color.FromArgb(200, 255, 255, 255))));
        /// <summary>
        /// Gets or sets default land fill
        /// </summary>
        public Brush DefaultLandFill
        {
            get { return (Brush) GetValue(DefaultLandFillProperty); }
            set { SetValue(DefaultLandFillProperty, value); }
        }

        /// <summary>
        /// The land stroke thickness property
        /// </summary>
        public static readonly DependencyProperty LandStrokeThicknessProperty = DependencyProperty.Register(
            "LandStrokeThickness", typeof (double), typeof (GeoMap), new PropertyMetadata(1.3d));
        /// <summary>
        /// Gets or sets every land stroke thickness property
        /// </summary>
        public double LandStrokeThickness
        {
            get { return (double) GetValue(LandStrokeThicknessProperty); }
            set { SetValue(LandStrokeThicknessProperty, value); }
        }

        /// <summary>
        /// The land stroke property
        /// </summary>
        public static readonly DependencyProperty LandStrokeProperty = DependencyProperty.Register(
            "LandStroke", typeof (Brush), typeof (GeoMap), new PropertyMetadata(new SolidColorBrush(Color.FromArgb(30, 55, 55, 55))));
        /// <summary>
        /// Gets or sets every land stroke
        /// </summary>
        public Brush LandStroke
        {
            get { return (Brush) GetValue(LandStrokeProperty); }
            set { SetValue(LandStrokeProperty, value); }
        }

        /// <summary>
        /// The disable animations property
        /// </summary>
        public static readonly DependencyProperty DisableAnimationsProperty = DependencyProperty.Register(
            "DisableAnimations", typeof (bool), typeof (GeoMap), new PropertyMetadata(default(bool)));
        /// <summary>
        /// Gets or sets whether the chart is animated
        /// </summary>
        public bool DisableAnimations
        {
            get { return (bool) GetValue(DisableAnimationsProperty); }
            set { SetValue(DisableAnimationsProperty, value); }
        }

        /// <summary>
        /// The animations speed property
        /// </summary>
        public static readonly DependencyProperty AnimationsSpeedProperty = DependencyProperty.Register(
            "AnimationsSpeed", typeof (TimeSpan), typeof (GeoMap), new PropertyMetadata(TimeSpan.FromMilliseconds(500)));
        /// <summary>
        /// Gets or sets animations speed
        /// </summary>
        public TimeSpan AnimationsSpeed
        {
            get { return (TimeSpan) GetValue(AnimationsSpeedProperty); }
            set { SetValue(AnimationsSpeedProperty, value); }
        }

        /// <summary>
        /// The hoverable property
        /// </summary>
        public static readonly DependencyProperty HoverableProperty = DependencyProperty.Register(
            "Hoverable", typeof (bool), typeof (GeoMap), new PropertyMetadata(default(bool)));
        /// <summary>
        /// Gets or sets whether the chart reacts when a user moves the mouse over a land
        /// </summary>
        public bool Hoverable
        {
            get { return (bool) GetValue(HoverableProperty); }
            set { SetValue(HoverableProperty, value); }
        }

        /// <summary>
        /// The heat map property
        /// </summary>
        public static readonly DependencyProperty HeatMapProperty = DependencyProperty.Register(
            "HeatMap", typeof (Dictionary<string, double>), typeof (GeoMap), 
            new PropertyMetadata(default(Dictionary<string, double>), OnHeapMapChanged));
        /// <summary>
        /// Gets or sets the current heat map
        /// </summary>
        public Dictionary<string, double> HeatMap
        {
            get { return (Dictionary<string, double>) GetValue(HeatMapProperty); }
            set { SetValue(HeatMapProperty, value); }
        }

        /// <summary>
        /// The gradient stop collection property
        /// </summary>
        public static readonly DependencyProperty GradientStopCollectionProperty = DependencyProperty.Register(
            "GradientStopCollection", typeof(GradientStopCollection), typeof(GeoMap), new PropertyMetadata(default(GradientStopCollection)));
        /// <summary>
        /// Gets or sets the gradient stop collection, use every gradient offset and color properties to define your gradient.
        /// </summary>
        public GradientStopCollection GradientStopCollection
        {
            get { return (GradientStopCollection)GetValue(GradientStopCollectionProperty); }
            set { SetValue(GradientStopCollectionProperty, value); }
        }

        /// <summary>
        /// The source property
        /// </summary>
        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register(
            "Source", typeof (string), typeof (GeoMap), new PropertyMetadata(default(string)));
        /// <summary>
        /// Gets or sets the map source
        /// </summary>
        public string Source
        {
            get { return (string) GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        /// <summary>
        /// The enable zooming and panning property
        /// </summary>
        public static readonly DependencyProperty EnableZoomingAndPanningProperty = DependencyProperty.Register(
            "EnableZoomingAndPanning", typeof (bool), typeof (GeoMap), new PropertyMetadata(default(bool)));
        /// <summary>
        /// Gets or sets whether the map allows zooming and panning
        /// </summary>
        public bool EnableZoomingAndPanning
        {
            get { return (bool) GetValue(EnableZoomingAndPanningProperty); }
            set { SetValue(EnableZoomingAndPanningProperty, value); }
        }

        #endregion

        #region Publics

        ///// <summary>
        ///// Moves the map to a specif area
        ///// </summary>
        ///// <param name="data">target area</param>
        //public void MoveTo(MapData data)
        //{
        //    var s = (Path) data.Shape;
        //    var area = s.Data.Bounds;
        //    var t = (ScaleTransform) s.RenderTransform;
            


        //    //if (DisableAnimations)
        //    //{
            
        //    double scale;
        //    double cx = 0;
        //    double cy = 0;

        //    if (IsWidthDominant)
        //    {
        //        scale = data.LvcMap.DesiredWidth / area.Width;
        //    }
        //    else
        //    {
        //        scale = data.LvcMap.DesiredHeight / area.Height;
        //    }

        //    Map.RenderTransformOrigin = new Point(0, 0);
        //    Map.RenderTransform = new ScaleTransform(scale, scale);

        //    Canvas.SetLeft(Map, -area.X*t.ScaleX*scale + cx);
        //    Canvas.SetTop(Map, -area.Y*t.ScaleY*scale + cy);

        //    //}
        //    //else
        //    //{
        //    //    Map.BeginAnimation(Canvas.LeftProperty, new DoubleAnimation(-area.X, AnimationsSpeed));
        //    //    //Map.BeginAnimation(Canvas.TopProperty, new DoubleAnimation(area.Y, AnimationsSpeed));
        //    //}
        //}

        /// <summary>
        /// Restarts the current map view
        /// </summary>
        public void Restart()
        {
            Map.RenderTransform = new ScaleTransform() {ScaleX = 1, ScaleY = 1};
            if (DisableAnimations)
            {
                Canvas.SetLeft(Map, OriginalPosition.X);
                Canvas.SetTop(Map, OriginalPosition.Y);
            }
            else
            {
                Map.CreateCanvasStoryBoardAndBegin(OriginalPosition.X, OriginalPosition.Y, TimeSpan.FromMilliseconds(1));
            }
        }

        /// <summary>
        /// Sets a heat map value with a given key, then updates every land heat color
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">new value</param>
        public void UpdateKey(string key, double value)
        {
            HeatMap[key] = value;
            ShowMeSomeHeat();
        }

        #endregion

        #region Privates

        private void Draw()
        {
            IsDrawn = true;

            Map.Children.Clear();

            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                Map.Children.Add(new TextBlock
                {
                    Text = "Designer preview is not currently available",
                    Foreground = new SolidColorBrush(Colors.White),
                    FontWeight = FontWeights.Bold,
                    FontSize = 12,
                    //Effect = new DropShadowEffect
                    //{
                    //    ShadowDepth = 2,
                    //    RenderingBias = RenderingBias.Performance
                    //}
                });
                return;
            }

            var map = MapResolver.Get(Source);
            if (map == null) return;

            var desiredSize = new Size(map.DesiredWidth, map.DesiredHeight);
            var r = desiredSize.Width/desiredSize.Height;

            var wr = ActualWidth/desiredSize.Width;
            var hr = ActualHeight/desiredSize.Height;
            double s;

            if (wr < hr)
            {
                IsWidthDominant = true;
                Map.Width = ActualWidth;
                Map.Height = Map.Width/r;
                s = wr;
                OriginalPosition = new Point(0, (ActualHeight - Map.Height)*.5);
                Canvas.SetLeft(Map, OriginalPosition.X);
                Canvas.SetTop(Map, OriginalPosition.Y);
            }
            else
            {
                IsWidthDominant = false;
                Map.Height = ActualHeight;
                Map.Width = r*ActualHeight;
                s = hr;
                OriginalPosition = new Point((ActualWidth - Map.Width)*.5, 0d);
                Canvas.SetLeft(Map, OriginalPosition.X);
                Canvas.SetTop(Map, OriginalPosition.Y);
            }

            var t = new ScaleTransform() {ScaleX = s, ScaleY = s};

            foreach (var land in map.Data)
            {
                var p = new Path
                {
                    Data = GeometryHelper.Parse(land.Data),
                    RenderTransform = t
                };

                land.Shape = p;
                Lands[land.Id] = land;
                Map.Children.Add(p);

                //p.MouseEnter += POnMouseEnter;
                //p.MouseLeave += POnMouseLeave;
                //p.MouseMove += POnMouseMove;
                //p.MouseDown += POnMouseDown;

                p.SetBinding(Shape.StrokeProperty,
                    new Binding { Path = new PropertyPath("LandStroke"), Source = this });
                var behavior = new MultiBindingBehavior()
                {
                    Converter = new ScaleStrokeConverter(),
                    PropertyName = "StrokeThickness"
                };
                behavior.Items.Add(new MultiBindingItem() {Parent = behavior.Items, Value = new Binding() { Path = new PropertyPath("LandStrokeThickness"), Source = this } });
                behavior.Items.Add(new MultiBindingItem() {Parent = behavior.Items, Value = new Binding() { Path = new PropertyPath("ScaleX"), Source = t } });
                Interaction.SetBehaviors(p, new BehaviorCollection() {behavior});
            }      

            ShowMeSomeHeat();
        }

        private void ShowMeSomeHeat()
        {
            var max = double.MinValue;
            var min = double.MaxValue;

            foreach (var i in HeatMap.Values)
            {
                max = i > max ? i : max;
                min = i < min ? i : min;
            }

            foreach (var land in Lands)
            {
                double temperature;
                var shape = ((Shape) land.Value.Shape);

                shape.SetBinding(Shape.FillProperty,
                    new Binding {Path = new PropertyPath("DefaultLandFill"), Source = this});

                if (!HeatMap.TryGetValue(land.Key, out temperature)) continue;

                temperature = LinealInterpolation(0, 1, min, max, temperature);
                var color = ColorInterpolation(temperature);

                if (DisableAnimations)
                {
                    shape.Fill = new SolidColorBrush(color);
                }
                else
                {
                    shape.Fill = new SolidColorBrush();
                    ((SolidColorBrush) shape.Fill).BeginColorAnimation(nameof(SolidColorBrush.Color), color, AnimationsSpeed);
                }
            }
        }

        private void POnMouseDown(object sender, PointerRoutedEventArgs mouseButtonEventArgs)
        {
            var land = Lands.Values.FirstOrDefault(x => x.Shape == sender);
            if (land == null) return;

            if (LandClick != null) LandClick.Invoke(sender, land);
        }

        private void POnMouseLeave(object sender, MouseEventArgs mouseEventArgs)
        {
            var path = (Path)sender;
            path.Opacity = 1;

            GeoMapTooltip.Visibility = Visibility.Collapsed;//Visibility.Hidden;
        }

        private void POnMouseEnter(object sender, MouseEventArgs mouseEventArgs)
        {
            var path = (Path) sender;
            path.Opacity = .8;

            var land = Lands.Values.FirstOrDefault(x => x.Shape == sender);
            if (land == null) return;

            double value;

            if (!HeatMap.TryGetValue(land.Id, out value)) return;
            if (!Hoverable) return;

            GeoMapTooltip.Visibility = Visibility.Visible;

            string name = null;

            if (LanguagePack != null) LanguagePack.TryGetValue(land.Id, out name);

            GeoMapTooltip.GeoData = new GeoData
            {
                Name = name ?? land.Name,
                Value = value
            };
        }

        //private void POnMouseMove(object sender, MouseEventArgs mouseEventArgs)
        //{
        //    var location = mouseEventArgs.GetPosition(this);
        //    Canvas.SetTop(GeoMapTooltip, location.Y + 5);
        //    Canvas.SetLeft(GeoMapTooltip, location.X + 5);
        //}

        private Color ColorInterpolation(double weight)
        {
            Color from = Color.FromArgb(255, 0, 0, 0), to = Color.FromArgb(255, 0, 0, 0);
            double fromOffset = 0, toOffset = 0;

            for (var i = 0; i < GradientStopCollection.Count-1; i++)
            {
                // ReSharper disable once InvertIf
                if (GradientStopCollection[i].Offset <= weight && GradientStopCollection[i + 1].Offset >= weight)
                {
                    from = GradientStopCollection[i].Color;
                    to = GradientStopCollection[i + 1].Color;

                    fromOffset = GradientStopCollection[i].Offset;
                    toOffset = GradientStopCollection[i + 1].Offset;

                    break;
                }
            }

            return Color.FromArgb(
                (byte) LinealInterpolation(from.A, to.A, fromOffset, toOffset, weight),
                (byte) LinealInterpolation(from.R, to.R, fromOffset, toOffset, weight),
                (byte) LinealInterpolation(from.G, to.G, fromOffset, toOffset, weight),
                (byte) LinealInterpolation(from.B, to.B, fromOffset, toOffset, weight));
        }

        private static double LinealInterpolation(double fromComponent, double toComponent,
            double fromOffset, double toOffset, double value)
        {
            var p1 = new Point(fromOffset, fromComponent);
            var p2 = new Point(toOffset, toComponent);

            var deltaX = p2.X - p1.X;
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            var m = (p2.Y - p1.Y) / (deltaX == 0 ? double.MinValue : deltaX);

            return m * (value - p1.X) + p1.Y;
        }

        private static void OnHeapMapChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var geoMap = (GeoMap)o;

            if (!geoMap.IsDrawn) return;

            geoMap.ShowMeSomeHeat();
        }
        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="LiveCharts.Uwp.Components.MultiBinding.MultiValueConverterBase" />
    public class ScaleStrokeConverter : MultiValueConverterBase
    {
        /// <summary>
        /// Modifies the source data before passing it to the target for display in the UI.
        /// </summary>
        /// <param name="values">The source data being passed to the target.</param>
        /// <param name="targetType">The <see cref="T:System.Type" /> of data expected by the target dependency property.</param>
        /// <param name="parameter">An optional parameter to be used in the converter logic.</param>
        /// <param name="culture">The culture of the conversion.</param>
        /// <returns>
        /// The value to be passed to the target dependency property.
        /// </returns>
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return (double) values[0]/(double) values[1];
        }

        /// <summary>
        /// Modifies the target data before passing it to the source object. This method is called only in <see cref="F:System.Windows.Data.BindingMode.TwoWay" /> bindings.
        /// </summary>
        /// <param name="value">The target data being passed to the source.</param>
        /// <param name="targetType">The <see cref="T:System.Type" /> of data expected by the source object.</param>
        /// <param name="parameter">An optional parameter to be used in the converter logic.</param>
        /// <param name="culture">The culture of the conversion.</param>
        /// <returns>
        /// The value to be passed to the source object.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override Object[] ConvertBack(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}