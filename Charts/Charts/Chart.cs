//The MIT License(MIT)

//Copyright(c) 2015 Alberto Rodriguez

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
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Threading;
using LiveCharts.Tooltip;

namespace LiveCharts.Charts
{
	public abstract class Chart : UserControl
	{
		internal Rect PlotArea;
		internal Point Max;
		internal Point Min;
		internal Point S;
		internal int ColorStartIndex;
		internal bool RequiresScale;
		internal List<Series> EraseSerieBuffer = new List<Series>();

		protected double CurrentScale;
		protected ShapeHoverBehavior ShapeHoverBehavior;
		protected bool IgnoresLastLabel;
		protected bool AlphaLabel;
		protected readonly DispatcherTimer TooltipTimer;

		private static readonly Random Randomizer;
		private readonly DispatcherTimer _resizeTimer;
		private readonly DispatcherTimer _serieValuesChanged;
		private readonly DispatcherTimer _seriesChanged;
		private Point _panOrigin;
		private bool _isDragging;
		private UIElement _dataToolTip;

		public event Action<Chart> Plot;

		static Chart()
		{
			Colors = new List<Color>
			{
				Color.FromRgb(41, 127, 184),
				Color.FromRgb(230, 76, 60),
				Color.FromRgb(240, 195, 15),
				Color.FromRgb(26, 187, 155),
				Color.FromRgb(87, 213, 140),
				Color.FromRgb(154, 89, 181),
				Color.FromRgb(92, 109, 126),
				Color.FromRgb(22, 159, 132),
				Color.FromRgb(39, 173, 96),
				Color.FromRgb(92, 171, 225),
				Color.FromRgb(141, 68, 172),
				Color.FromRgb(229, 126, 34),
				Color.FromRgb(210, 84, 0),
				Color.FromRgb(191, 57, 43)
			};
			Randomizer = new Random();
		}

		protected Chart()
		{
			var b = new Border {ClipToBounds = true};
			Canvas = new Canvas {RenderTransform = new TranslateTransform(0, 0)};
			b.Child = Canvas;
			Content = b;

			if (RandomizeStartingColor) ColorStartIndex = Randomizer.Next(0, Colors.Count - 1);

			AnimatesNewPoints = false;
			CurrentScale = 1;

			PerformanceConfiguration = new PerformanceConfiguration {Enabled = false};
			Series = new ObservableCollection<Series>();
			DataToolTip = new IndexedToolTip();
			Shapes = new List<FrameworkElement>();
			HoverableShapes = new List<HoverableShape>();
			PointHoverColor = System.Windows.Media.Colors.White;

			//it requieres a background so it detect mouse down/up events.
			Background = Brushes.Transparent;

			SizeChanged += Chart_OnsizeChanged;
			MouseWheel += MouseWheelOnRoll;
			MouseLeftButtonDown += MouseDownForPan;
			MouseLeftButtonUp += MouseUpForPan;
			MouseMove += MouseMoveForPan;

			_resizeTimer = new DispatcherTimer
			{
				Interval = TimeSpan.FromMilliseconds(100)
			};
			_resizeTimer.Tick += (sender, e) =>
			{
				_resizeTimer.Stop();
				ClearAndPlot();
			};
			TooltipTimer = new DispatcherTimer
			{
				Interval = TimeSpan.FromMilliseconds(1000)
			};
			TooltipTimer.Tick += TooltipTimerOnTick;

			_serieValuesChanged = new DispatcherTimer {Interval = TimeSpan.FromMilliseconds(100)};
			_serieValuesChanged.Tick += UpdateModifiedDataSeries;

			_seriesChanged = new DispatcherTimer {Interval = TimeSpan.FromMilliseconds(100)};
			_seriesChanged.Tick += UpdateSeries;
		}

		#region Abstracts

		protected abstract void Scale();
		protected abstract bool ScaleChanged { get; }

		#endregion

		#region StaticProperties

		/// <summary>
		/// List of Colors series will use, yu can change this list to your own colors.
		/// </summary>
		public static List<Color> Colors { get; set; }

		/// <summary>
		/// indicates wether each instance of chart you create needs to randomize starting color
		/// </summary>
		public static bool RandomizeStartingColor { get; set; }

		#endregion

		#region Dependency Properties

		public static readonly DependencyProperty HoverableProperty = DependencyProperty.Register(
			"Hoverable", typeof (bool), typeof (Chart));

		/// <summary>
		/// Indicates weather chart is hoverable or not
		/// </summary>
		public bool Hoverable
		{
			get { return (bool) GetValue(HoverableProperty); }
			set { SetValue(HoverableProperty, value); }
		}

		public static readonly DependencyProperty PointHoverColorProperty = DependencyProperty.Register(
			"PointHoverColor", typeof (Color), typeof (Chart));

		/// <summary>
		/// Indicates Point hover color.
		/// </summary>
		public Color PointHoverColor
		{
			get { return (Color) GetValue(PointHoverColorProperty); }
			set { SetValue(PointHoverColorProperty, value); }
		}

		public static readonly DependencyProperty DisableAnimationProperty = DependencyProperty.Register(
			"DisableAnimation", typeof (bool), typeof (Chart));

		/// <summary>
		/// Indicates weather to show animation or not.
		/// </summary>
		public bool DisableAnimation
		{
			get { return (bool) GetValue(DisableAnimationProperty); }
			set { SetValue(DisableAnimationProperty, value); }
		}

		public static readonly DependencyProperty SeriesProperty = DependencyProperty.Register(
			"Series", typeof (ObservableCollection<Series>), typeof (Chart),
			new PropertyMetadata(new ObservableCollection<Series>(), SeriesChangedCallback ));
		
		public ObservableCollection<Series> Series
		{
			get { return (ObservableCollection<Series>) GetValue(SeriesProperty); }
			set { SetValue(SeriesProperty, value); }
		}
		#endregion

		#region Properties
		public Canvas Canvas { get; internal set; }
		public double XOffset { get; internal set; }
		public List<FrameworkElement> Shapes { get; internal set; }
		public List<HoverableShape> HoverableShapes { get; internal set; } 

		public Axis PrimaryAxis { get; set; }
		public Axis SecondaryAxis { get; set; }
		public UIElement DataToolTip
		{
			get { return _dataToolTip; }
			set
			{
				_dataToolTip = value;
				if (value == null) return;
				Panel.SetZIndex(_dataToolTip, int.MaxValue);
				Canvas.SetLeft(_dataToolTip,0);
				Canvas.SetTop(_dataToolTip, 0);
				_dataToolTip.Visibility = Visibility.Hidden;
				Canvas.Children.Add(_dataToolTip);
			}
		}
		public PerformanceConfiguration PerformanceConfiguration { get; set; }
		public bool Zooming { get; set; }
		public double AreaOpacity { get; set; }
		#endregion

		#region ProtectedProperties
		protected bool AnimatesNewPoints { get; set; }
		#endregion

		#region Public Methods
		public void ClearAndPlot()
		{
			_seriesChanged.Stop();
			_seriesChanged.Start();
			PrepareCanvas(true);
		}

		public void ZoomIn()
		{
			CurrentScale += .1;
			ForceRedrawNow();
			PreventGraphToBeVisible();
		}

		public void ZoomOut()
		{
			CurrentScale -= .1;
			if (CurrentScale <= 1) CurrentScale = 1;
			ForceRedrawNow();
			PreventGraphToBeVisible();
		}

		/// <summary>
		/// Scales a graph value to screen according to an axis. 
		/// </summary>
		/// <param name="value"></param>
		/// <param name="axis"></param>
		/// <returns></returns>
		public double ToPlotArea(double value, AxisTags axis)
		{
			return EnsureDouble(Methods.ToPlotArea(value, axis, this));
		}

		/// <summary>
		/// Scales a graph point to screen.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public Point ToPlotArea(Point value)
		{
			return new Point(ToPlotArea(value.X, AxisTags.X), ToPlotArea(value.Y, AxisTags.Y));
		}
		#endregion

		#region ProtectedMethods
		/// <summary>
		/// Axis Y: is Horizontal axis, Axis X: Vertical, confusing but this is how it is!
		/// </summary>
		/// <param name="range"></param>
		/// <param name="axis"></param>
		/// <returns></returns>
		protected double CalculateSeparator(double range, AxisTags axis)
		{
			//based on:
			//http://stackoverflow.com/questions/361681/algorithm-for-nice-grid-line-intervals-on-a-graph

			var m = axis == AxisTags.Y ? Min.Y : Min.X;
			if (Math.Abs(range) < m * .01) range = m;

			var ft = axis == AxisTags.Y
				? new FormattedText(
					"A label",
					CultureInfo.CurrentUICulture,
					FlowDirection.LeftToRight,
					new Typeface(PrimaryAxis.FontFamily, PrimaryAxis.FontStyle, PrimaryAxis.FontWeight,
						PrimaryAxis.FontStretch), PrimaryAxis.FontSize, Brushes.Black)
				: new FormattedText(
					"A label",
					CultureInfo.CurrentUICulture,
					FlowDirection.LeftToRight,
					new Typeface(SecondaryAxis.FontFamily, SecondaryAxis.FontStyle, SecondaryAxis.FontWeight,
						SecondaryAxis.FontStretch), SecondaryAxis.FontSize, Brushes.Black);

			var separations = axis == AxisTags.Y
				? Math.Round(PlotArea.Height / ((ft.Height) * PrimaryAxis.CleanFactor), 0)
				: Math.Round(PlotArea.Width / ((ft.Width) * SecondaryAxis.CleanFactor), 0);

			separations = separations < 2 ? 2 : separations;

			var minimum = range / separations;
			var magnitude = Math.Pow(10, Math.Floor(Math.Log(minimum) / Math.Log(10)));
			var residual = minimum / magnitude;
			double tick;
			if (residual > 5)
				tick = 10 * magnitude;
			else if (residual > 2)
				tick = 5 * magnitude;
			else if (residual > 1)
				tick = 2 * magnitude;
			else
				tick = magnitude;
			return tick;
		}

		protected void ConfigureSmartAxis(Axis axis)
		{
			axis.PrintLabels = axis.Labels != null;
			if (axis.Labels == null || !axis.PrintLabels) return;
			var m = axis.Labels.OrderByDescending(x => x.Length);
			var longestYLabel = new FormattedText(m.First(), CultureInfo.CurrentUICulture, FlowDirection.LeftToRight,
				new Typeface(axis.FontFamily, axis.FontStyle, axis.FontWeight, axis.FontStretch), axis.FontSize,
				Brushes.Black);
			axis.Separator.Step = (longestYLabel.Width * Max.X) * 1.25 > PlotArea.Width
				? null
				: (int?)1;
		}

		protected Point GetLongestLabelSize(Axis axis)
		{
			if (!axis.PrintLabels) return new Point(0, 0);
			var label = "";
			var from = Equals(axis, PrimaryAxis) ? Min.Y : Min.X;
			var to = Equals(axis, PrimaryAxis) ? Max.Y : Max.X;
			var s = Equals(axis, PrimaryAxis) ? S.Y : S.X;
			for (var i = from; i <= to; i += s)
			{
				var iL = axis.LabelFormatter == null
					? i.ToString(CultureInfo.InvariantCulture)
					: axis.LabelFormatter(i);
				if (label.Length < iL.Length)
				{
					label = iL;
				}
			}
			var longestLabel = new FormattedText(label, CultureInfo.CurrentUICulture, FlowDirection.LeftToRight,
				new Typeface(axis.FontFamily, axis.FontStyle, axis.FontWeight,
				axis.FontStretch), axis.FontSize, Brushes.Black);
			return new Point(longestLabel.Width, longestLabel.Height);
		}

		protected Point GetLabelSize(Axis axis, double value)
		{
			if (!axis.PrintLabels) return new Point(0, 0);

			var labels = axis.Labels?.ToArray();
			var fomattedValue = labels == null
				? (SecondaryAxis.LabelFormatter == null
					? Min.X.ToString(CultureInfo.InvariantCulture)
					: SecondaryAxis.LabelFormatter(value))
				: (labels.Length > value
					? labels[(int)value]
					: "");
			var uiLabelSize = new FormattedText(fomattedValue, CultureInfo.CurrentUICulture, FlowDirection.LeftToRight,
				new Typeface(axis.FontFamily, axis.FontStyle, axis.FontWeight, axis.FontStretch),
				axis.FontSize, Brushes.Black);
			return new Point(uiLabelSize.Width, uiLabelSize.Height);
		}

		protected Point GetLabelSize(Axis axis, string value)
		{
			if (!axis.PrintLabels) return new Point(0, 0);
			var fomattedValue = value;
			var uiLabelSize = new FormattedText(fomattedValue, CultureInfo.CurrentUICulture, FlowDirection.LeftToRight,
				new Typeface(axis.FontFamily, axis.FontStyle, axis.FontWeight, axis.FontStretch),
				axis.FontSize, Brushes.Black);
			return new Point(uiLabelSize.Width, uiLabelSize.Height);
		}
		#endregion

		#region Virtual Methods
		protected virtual void DrawAxis()
		{
			foreach (var l in Shapes) Canvas.Children.Remove(l);
			//Titles
			var titleY = 0d;
			if (!string.IsNullOrWhiteSpace(PrimaryAxis.Title))
			{
				var ty = GetLabelSize(PrimaryAxis, PrimaryAxis.Title);
				var yLabel = new TextBlock
				{
					FontFamily = PrimaryAxis.FontFamily,
					FontSize = PrimaryAxis.FontSize,
					FontStretch = PrimaryAxis.FontStretch,
					FontStyle = PrimaryAxis.FontStyle,
					FontWeight = PrimaryAxis.FontWeight,
					Foreground = PrimaryAxis.Foreground,
					Text = PrimaryAxis.Title,
					RenderTransform = new RotateTransform(-90)
				};
				Shapes.Add(yLabel);
				Canvas.Children.Add(yLabel);
				Canvas.SetLeft(yLabel, 5);
				Canvas.SetTop(yLabel, Canvas.DesiredSize.Height * .5 + ty.X * .5);
				titleY += ty.Y + 5;
			}
			var titleX = 0d;
			if (!string.IsNullOrWhiteSpace(SecondaryAxis.Title))
			{
				var tx = GetLabelSize(SecondaryAxis, SecondaryAxis.Title);
				var yLabel = new TextBlock
				{
					FontFamily = SecondaryAxis.FontFamily,
					FontSize = SecondaryAxis.FontSize,
					FontStretch = SecondaryAxis.FontStretch,
					FontStyle = SecondaryAxis.FontStyle,
					FontWeight = SecondaryAxis.FontWeight,
					Foreground = SecondaryAxis.Foreground,
					Text = SecondaryAxis.Title
				};
				Shapes.Add(yLabel);
				Canvas.Children.Add(yLabel);
				Canvas.SetLeft(yLabel, Canvas.DesiredSize.Width * .5 - tx.X * .5);
				Canvas.SetTop(yLabel, Canvas.DesiredSize.Height - tx.Y - 5);
				titleX += tx.Y;
			}

			PlotArea.X += titleY;
			PlotArea.Width -= titleY;
			PlotArea.Height -= titleX;

			//drawing primary axis
			var ly = PrimaryAxis.Separator.Enabled || PrimaryAxis.PrintLabels
				? Max.Y
				: Min.Y - 1;
			var longestYLabelSize = GetLongestLabelSize(PrimaryAxis);
			for (var i = Min.Y; i <= ly; i += S.Y)
			{
				var y = ToPlotArea(i, AxisTags.Y);
				if (PrimaryAxis.Separator.Enabled)
				{
					var l = new Line
					{
						Stroke = new SolidColorBrush { Color = PrimaryAxis.Separator.Color },
						StrokeThickness = PrimaryAxis.Separator.Thickness,
						X1 = ToPlotArea(Min.X, AxisTags.X),
						Y1 = y,
						X2 = ToPlotArea(Max.X, AxisTags.X),
						Y2 = y
					};
					Canvas.Children.Add(l);
					Shapes.Add(l);
				}

				if (PrimaryAxis.PrintLabels)
				{
					var t = PrimaryAxis.LabelFormatter == null
						? i.ToString(CultureInfo.InvariantCulture)
						: PrimaryAxis.LabelFormatter(i);
					var label = new TextBlock
					{
						FontFamily = PrimaryAxis.FontFamily,
						FontSize = PrimaryAxis.FontSize,
						FontStretch = PrimaryAxis.FontStretch,
						FontStyle = PrimaryAxis.FontStyle,
						FontWeight = PrimaryAxis.FontWeight,
						Foreground = PrimaryAxis.Foreground,
						Text = t
					};
					var fl = new FormattedText(t, CultureInfo.CurrentUICulture, FlowDirection.LeftToRight,
						new Typeface(PrimaryAxis.FontFamily, PrimaryAxis.FontStyle, PrimaryAxis.FontWeight,
							PrimaryAxis.FontStretch), PrimaryAxis.FontSize, Brushes.Black);
					Canvas.Children.Add(label);
					Shapes.Add(label);
					Canvas.SetLeft(label, titleY + (5 + longestYLabelSize.X) - fl.Width);
					Canvas.SetTop(label, ToPlotArea(i, AxisTags.Y) - longestYLabelSize.Y * .5);
				}
			}

			//drawing secondary axis
			var lx = SecondaryAxis.Separator.Enabled || SecondaryAxis.PrintLabels
				? Max.X + (IgnoresLastLabel ? -1 : 0)
				: Min.X - 1;

			for (var i = Min.X; i <= lx; i += S.X)
			{
				var x = ToPlotArea(i, AxisTags.X);
				if (SecondaryAxis.Separator.Enabled)
				{
					var l = new Line
					{
						Stroke = new SolidColorBrush { Color = SecondaryAxis.Separator.Color },
						StrokeThickness = SecondaryAxis.Separator.Thickness,
						X1 = x,
						Y1 = ToPlotArea(Max.Y, AxisTags.Y),
						X2 = x,
						Y2 = ToPlotArea(Min.Y, AxisTags.Y)
					};
					Canvas.Children.Add(l);
					Shapes.Add(l);
				}

				if (SecondaryAxis.PrintLabels)
				{
					var labels = SecondaryAxis.Labels?.ToArray();
					var t = labels == null
						? (SecondaryAxis.LabelFormatter == null
							? i.ToString(CultureInfo.InvariantCulture)
							: SecondaryAxis.LabelFormatter(i))
						: (labels.Length > i
							? labels[(int)i]
							: "");
					var label = new TextBlock
					{
						FontFamily = SecondaryAxis.FontFamily,
						FontSize = SecondaryAxis.FontSize,
						FontStretch = SecondaryAxis.FontStretch,
						FontStyle = SecondaryAxis.FontStyle,
						FontWeight = SecondaryAxis.FontWeight,
						Foreground = SecondaryAxis.Foreground,
						Text = t
					};
					var fl = new FormattedText(t, CultureInfo.CurrentUICulture, FlowDirection.LeftToRight,
						new Typeface(SecondaryAxis.FontFamily, SecondaryAxis.FontStyle, SecondaryAxis.FontWeight,
							SecondaryAxis.FontStretch), SecondaryAxis.FontSize, Brushes.Black);
					Canvas.Children.Add(label);
					Shapes.Add(label);
					Canvas.SetLeft(label, ToPlotArea(i, AxisTags.X) - fl.Width * .5 + XOffset);
					Canvas.SetTop(label, PlotArea.Y + PlotArea.Height + 5);
				}
			}

			//drawing ceros.
			if (Max.Y >= 0 && Min.Y <= 0 && PrimaryAxis.IsEnabled)
			{
				var l = new Line
				{
					Stroke = new SolidColorBrush { Color = PrimaryAxis.Color },
					StrokeThickness = PrimaryAxis.Thickness,
					X1 = ToPlotArea(Min.X, AxisTags.X),
					Y1 = ToPlotArea(0, AxisTags.Y),
					X2 = ToPlotArea(Max.X, AxisTags.X),
					Y2 = ToPlotArea(0, AxisTags.Y)
				};
				Canvas.Children.Add(l);
				Shapes.Add(l);
			}

			if (Max.X >= 0 && Min.X <= 0 && SecondaryAxis.IsEnabled)
			{
				var l = new Line
				{
					Stroke = new SolidColorBrush { Color = SecondaryAxis.Color },
					StrokeThickness = SecondaryAxis.Thickness,
					X1 = ToPlotArea(0, AxisTags.X),
					Y1 = ToPlotArea(Min.Y, AxisTags.Y),
					X2 = ToPlotArea(0, AxisTags.X),
					Y2 = ToPlotArea(Max.Y, AxisTags.Y)
				};
				Canvas.Children.Add(l);
				Shapes.Add(l);
			}
		}

		public virtual void DataMouseEnter(object sender, MouseEventArgs e)
		{
			if (DataToolTip == null) return;

			DataToolTip.Visibility = Visibility.Visible;
			TooltipTimer.Stop();

			var senderShape = HoverableShapes.FirstOrDefault(s => Equals(s.Shape, sender));
			if (senderShape == null) return;
			var sibilings = HoverableShapes
				.Where(s => Math.Abs(s.Value.X - senderShape.Value.X) < S.X*.001).ToList();

			var first = sibilings.Count > 0 ? sibilings[0] : null;
			var labels = SecondaryAxis.Labels?.ToArray();
			var vx = first?.Value.X ?? 0;
			vx = AlphaLabel ? (int) (vx/(360d/Series.First().PrimaryValues.Count)) : vx;

			foreach (var sibiling in sibilings)
			{
				if (ShapeHoverBehavior == ShapeHoverBehavior.Dot)
				{
					sibiling.Target.Stroke = sibiling.Series.Stroke;
					sibiling.Target.Fill = new SolidColorBrush {Color = PointHoverColor};
				}
				else
				{
					sibiling.Target.Opacity = .8;
				}
			}

			var indexedToolTip = DataToolTip as IndexedToolTip;
			if (indexedToolTip != null)
			{
				indexedToolTip.Header = labels == null
						? (SecondaryAxis.LabelFormatter == null
							? vx.ToString(CultureInfo.InvariantCulture)
							: SecondaryAxis.LabelFormatter(vx))
						: (labels.Length > vx
							? labels[(int) vx]
							: "");
				indexedToolTip.Data = sibilings.Select(x => new IndexedTooltipData
					{
						Fill = x.Series.Fill,
						Stroke = x.Series.Stroke,
						Title = x.Series.Title,
						Value = PrimaryAxis.LabelFormatter == null
							        ? x.Value.Y.ToString(CultureInfo.InvariantCulture)
							        : PrimaryAxis.LabelFormatter(x.Value.Y)
					}).ToArray();
			}

			var p = GetToolTipPosition(senderShape, sibilings);

			DataToolTip.BeginAnimation(Canvas.LeftProperty, new DoubleAnimation
			{
				To = p.X,
				Duration = TimeSpan.FromMilliseconds(200)
			});
			DataToolTip.BeginAnimation(Canvas.TopProperty, new DoubleAnimation
			{
				To = p.Y,
				Duration = TimeSpan.FromMilliseconds(200)
			});
		}

		public virtual void DataMouseLeave(object sender, MouseEventArgs e)
		{
			var s = sender as Shape;
			if (s == null) return;

			var shape = HoverableShapes.FirstOrDefault(x => Equals(x.Shape, s));
			if (shape == null) return;

			var sibilings = HoverableShapes
				.Where(x => Math.Abs(x.Value.X - shape.Value.X) < .001 * S.X).ToList();

			foreach (var p in sibilings)
			{
				if (ShapeHoverBehavior == ShapeHoverBehavior.Dot)
				{
					p.Target.Fill = p.Series.Stroke;
					p.Target.Stroke = new SolidColorBrush { Color = PointHoverColor };
				}
				else
				{
					p.Target.Opacity = 1;
				}
			}
			TooltipTimer.Stop();
			TooltipTimer.Start();
		}
		protected virtual Point GetToolTipPosition(HoverableShape sender, List<HoverableShape> sibilings)
		{
			DataToolTip.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
			var x = sender.Value.X > (Min.X + Max.X) / 2
				? ToPlotArea(sender.Value.X, AxisTags.X) - 10 - DataToolTip.DesiredSize.Width
				: ToPlotArea(sender.Value.X, AxisTags.X) + 10;
			var y = ToPlotArea(sibilings.Select(s => s.Value.Y).DefaultIfEmpty(0).Sum()
							   / sibilings.Count, AxisTags.Y);
			y = y + DataToolTip.DesiredSize.Height > ActualHeight
				? y - (y + DataToolTip.DesiredSize.Height - ActualHeight) - 5
				: y;
			return new Point(x, y);
		}
		#endregion

		#region Private Methods
		private double EnsureDouble(double d)
		{
			return double.IsNaN(d) ? 0 : d;
		}

		private void ForceRedrawNow()
		{
			PrepareCanvas();
			UpdateSeries(null, null);
		}

		private void PrepareCanvas(bool animate = false)
		{
			foreach (var shape in Shapes) Canvas.Children.Remove(shape);
			foreach (var shape in HoverableShapes.Select(x => x.Shape).ToList())
				Canvas.Children.Remove(shape);
			foreach (var serie in Series) Canvas.Children.Remove(serie);
			HoverableShapes = new List<HoverableShape>();
			Shapes = new List<FrameworkElement>();
			foreach (var serie in Series)
			{
				Canvas.Children.Add(serie);
				EraseSerieBuffer.Add(serie);
				serie.RequiresAnimation = animate;
				serie.RequiresPlot = true;
			}
			Canvas.Width = ActualWidth * CurrentScale;
			Canvas.Height = ActualHeight * CurrentScale;
			PlotArea = new Rect(0, 0, ActualWidth * CurrentScale, ActualHeight * CurrentScale);
			RequiresScale = true;
		}

		private void PreventGraphToBeVisible()
		{
			var tt = Canvas.RenderTransform as TranslateTransform;
			if (tt == null) return;
			var eX = tt.X;
			var eY = tt.Y;
			var xOverflow = -tt.X + ActualWidth - Canvas.Width;
			var yOverflow = -tt.Y + ActualHeight - Canvas.Height;

			if (eX > 0)
			{
				//Cant understand why with I cant animate this...
				//Pan stops working when I do animation on overflow

				//var y = new DoubleAnimation(tt.Y, 0, TimeSpan.FromMilliseconds(150));
				//var x = new DoubleAnimation(tt.X, 0, TimeSpan.FromMilliseconds(150));

				//I even try this... but nope
				//y.Completed += (o, args) => { tt.Y = 0; };
				//x.Completed += (o, args) => { tt.X = 0; };

				//Canvas.RenderTransform.BeginAnimation(TranslateTransform.YProperty, y);
				//Canvas.RenderTransform.BeginAnimation(TranslateTransform.XProperty, x);
				tt.X = 0;
			}

			if (eY > 0)
			{
				tt.Y = 0;
			}

			if (xOverflow > 0)
			{
				tt.X = tt.X + xOverflow;
			}

			if (yOverflow > 0)
			{
				tt.Y = tt.Y + yOverflow;
			}
		}

		private void Chart_OnsizeChanged(object sender, SizeChangedEventArgs e)
		{
			_resizeTimer.Stop();
			_resizeTimer.Start();
		}

		private static void SeriesChangedCallback(DependencyObject o, DependencyPropertyChangedEventArgs eventArgs)
		{
			var chart = o as Chart;
			if (chart == null) return;

			foreach (var serie in chart.Series)
			{
				serie.Chart = chart;
				serie.ColorIndex = chart.Series.Max(x => x.ColorIndex) + 1;
				serie.RequiresPlot = true;
				serie.RequiresAnimation = true;
				var observable = serie.PrimaryValues as INotifyCollectionChanged;
				if (observable != null)
					observable.CollectionChanged += chart.OnDataSeriesChanged;
			}

			chart.Series.CollectionChanged += (sender, args) =>
			{               
				chart._seriesChanged.Stop();
				chart._seriesChanged.Start();

				if (args.OldItems != null)
					foreach (var serie in args.OldItems.Cast<Series>())
					{
						chart.EraseSerieBuffer.Add(serie);
					}

				var newElements = args.NewItems?.Cast<Series>() ?? new List<Series>();

				if (chart.ScaleChanged)
				{
					chart.RequiresScale = true;
					foreach (var serie in chart.Series.Where(x => !newElements.Contains(x)))
					{
						chart.EraseSerieBuffer.Add(serie);
						serie.RequiresPlot = true;
					}
				}

				if (args.NewItems != null)
					foreach (var serie in newElements)
					{
						serie.Chart = chart;
						serie.ColorIndex = chart.Series.Max(x => x.ColorIndex) + 1;
						serie.RequiresPlot = true;
						serie.RequiresAnimation = true;
						var observable = serie.PrimaryValues as INotifyCollectionChanged;
						if (observable != null)
							observable.CollectionChanged += chart.OnDataSeriesChanged;
					}
			};
		}

		private void UpdateSeries(object sender, EventArgs e)
		{
			_seriesChanged.Stop();
			if (PlotArea.Width < 15 || PlotArea.Height < 15) return;

			if (RequiresScale)
			{
				Scale();
				RequiresScale = false;
			}
			foreach (var serie in EraseSerieBuffer.GroupBy(x => x))
			{
				serie.First().Erase();
			}
			EraseSerieBuffer.Clear();
			var toPlot = Series.Where(x => x.RequiresPlot).ToList();
			foreach (var serie in toPlot)
			{
				serie.CalculatePoints();
				serie.Plot(serie.RequiresAnimation);
				serie.RequiresPlot = false;
				serie.RequiresAnimation = false;
			}

			Plot?.Invoke(this);
#if DEBUG
			Trace.WriteLine("Series Updated!");
#endif
		}

		private void OnDataSeriesChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			_serieValuesChanged.Stop();
			_serieValuesChanged.Start();
		}

		private void UpdateModifiedDataSeries(object sender, EventArgs e)
		{
			_serieValuesChanged.Stop();
			if (ScaleChanged) Scale();
			foreach (var serie in Series)
			{
				serie.Erase();
				serie.Plot(AnimatesNewPoints);
			}
		}

		private void MouseWheelOnRoll(object sender, MouseWheelEventArgs e)
		{
			if (!Zooming) return;
			e.Handled = true;
			if (e.Delta > 0) ZoomIn();
			else ZoomOut();
		}

		private void MouseDownForPan(object sender, MouseEventArgs e)
		{
			if (!Zooming) return;
			_panOrigin = e.GetPosition(this);
			_isDragging = true;
		}

		private void MouseMoveForPan(object sender, MouseEventArgs e)
		{
			if (!_isDragging) return;
			var tt = Canvas.RenderTransform as TranslateTransform;
			if (tt == null) return;

			var movePoint = e.GetPosition(this);
			var dif = _panOrigin - movePoint;

			tt.X = tt.X - dif.X;
			tt.Y = tt.Y - dif.Y;

			_panOrigin = movePoint;
		}

		private void MouseUpForPan(object sender, MouseEventArgs e)
		{
			if (!Zooming) return;
			_isDragging = false;
			PreventGraphToBeVisible();
		}

		private void TooltipTimerOnTick(object sender, EventArgs e)
		{
			DataToolTip.Visibility = Visibility.Hidden;
		}
		#endregion
	}
}