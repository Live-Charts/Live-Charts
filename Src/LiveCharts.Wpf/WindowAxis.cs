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

using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using LiveCharts.Charts;
using LiveCharts.Definitions.Charts;
using LiveCharts.Wpf.Components;

namespace LiveCharts.Wpf
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="LiveCharts.Wpf.Axis" />
    /// <seealso cref="LiveCharts.Definitions.Charts.IWindowAxisView" />
    public class WindowAxis : Axis, IWindowAxisView
    {
        public static readonly DependencyProperty WindowsProperty = DependencyProperty.Register("Windows", typeof(AxisWindowCollection), typeof(WindowAxis),new PropertyMetadata(default(AxisWindowCollection)));
        public static readonly DependencyProperty SelectedWindowProperty = DependencyProperty.Register("SelectedWindow", typeof(IAxisWindow), typeof(WindowAxis), new PropertyMetadata(null));
        public static readonly DependencyProperty HeaderFontWeightProperty = DependencyProperty.Register("HeaderFontWeight", typeof(FontWeight), typeof(WindowAxis), new PropertyMetadata(FontWeights.ExtraBold));
        public static readonly DependencyProperty HeaderForegroundProperty = DependencyProperty.Register("HeaderForeground", typeof(Brush), typeof(WindowAxis), new PropertyMetadata(new SolidColorBrush(Color.FromRgb(130, 130, 130))));

        /// <summary>
        /// 
        /// </summary>
        public AxisWindowCollection Windows
        {
            get { return (AxisWindowCollection)GetValue(WindowsProperty); }
            set { SetValue(WindowsProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public IAxisWindow SelectedWindow
        {
            get { return (IAxisWindow)GetValue(SelectedWindowProperty); }
            set { SetValue(SelectedWindowProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public WindowAxis()
        {
            SetCurrentValue(WindowsProperty, new AxisWindowCollection());
        }

        /// <summary>
        /// Gets or sets labels font weight
        /// </summary>
        public FontWeight HeaderFontWeight
        {
            get { return (FontWeight)GetValue(HeaderFontWeightProperty); }
            set { SetValue(HeaderFontWeightProperty, value); }
        }

        /// <summary>
        /// Gets or sets labels text color.
        /// </summary>
        public Brush HeaderForeground
        {
            get { return (Brush)GetValue(HeaderForegroundProperty); }
            set { SetValue(HeaderForegroundProperty, value); }
        }

        public override AxisCore AsCoreElement(ChartCore chart, AxisOrientation source)
        {
            if (Model == null) Model = new WindowAxisCore(this);
            Model.ShowLabels = ShowLabels;
            Model.Chart = chart;
            Model.IsMerged = IsMerged;
            Model.Labels = Labels;
            Model.LabelFormatter = LabelFormatter;
            Model.MaxValue = MaxValue;
            Model.MinValue = MinValue;
            Model.Title = Title;
            Model.Position = Position;
            Model.Separator = Separator.AsCoreElement(Model, source);
            Model.DisableAnimations = DisableAnimations;
            Model.Sections = Sections.Select(x => x.AsCoreElement(Model, source)).ToList();
            ((WindowAxisCore) Model).Windows = Windows.ToList();
            return Model;
        }
                        
        public override void RenderSeparator(SeparatorElementCore model, ChartCore chart)
        {
            base.RenderSeparator(model, chart);

            // Review whetehr hould we not implement this with a trigger instead of resetting property bindings
            var element = (AxisSeparatorElement)model.View;
            if (((DateSeparatorElementCore)model).IsHeader)
            {
                element.TextBlock.SetBinding(TextBlock.FontWeightProperty, new Binding
                {
                    Path = new PropertyPath(HeaderFontWeightProperty),
                    Source = this
                });
                element.TextBlock.SetBinding(TextBlock.ForegroundProperty, new Binding
                {
                    Path = new PropertyPath(HeaderForegroundProperty),
                    Source = this
                });
            }
            else
            {
                element.TextBlock.SetBinding(TextBlock.FontWeightProperty, new Binding
                {
                    Path = new PropertyPath(FontWeightProperty),
                    Source = this
                });
                element.TextBlock.SetBinding(TextBlock.ForegroundProperty, new Binding
                {
                    Path = new PropertyPath(ForegroundProperty),
                    Source = this
                });
            }
        }

        public void SetSelectedWindow(IAxisWindow window)
        {
            SetCurrentValue(SelectedWindowProperty, window);
        }
    }
}