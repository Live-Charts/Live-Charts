#region License
// The MIT License (MIT)
// 
// Copyright (c) 2016 Alberto Rodríguez Orozco & LiveCharts contributors
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy 
// of this software and associated documentation files (the "Software"), to deal 
// in the Software without restriction, including without limitation the rights to 
// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies 
// of the Software, and to permit persons to whom the Software is furnished to 
// do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all 
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, 
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES 
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND 
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT 
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, 
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING 
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR 
// OTHER DEALINGS IN THE SOFTWARE.
#endregion

#region

using System;
using LiveCharts.Core.Charts;
using LiveCharts.Core.Drawing;
using LiveCharts.Core.Drawing.Styles;
using LiveCharts.Core.Interaction.Dimensions;
using LiveCharts.Core.Interaction.Events;
#if NET45 || NET46
using Font = LiveCharts.Core.Drawing.Styles.Font;
#endif

#endregion

namespace LiveCharts.Core.Dimensions
{
    /// <summary>
    /// Defines a section in an axis.
    /// </summary>
    public class Section : IResource
    {
        private IPlaneViewProvider _viewProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="Section"/> class.
        /// </summary>
        public Section()
        {
            Font = Font.Empty;
            LabelHorizontalAlignment = HorizontalAlignment.Centered;
            LabelVerticalAlignment = VerticalAlignment.Centered;
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public double Value { get; set; }

        /// <summary>
        /// Gets or sets the length.
        /// </summary>
        /// <value>
        /// The length.
        /// </value>
        public double Length { get; set; }

        /// <summary>
        /// Gets or sets the stroke.
        /// </summary>
        /// <value>
        /// The stroke.
        /// </value>
        public Brush Stroke { get; set; }

        /// <summary>
        /// Gets or sets the stroke thickness.
        /// </summary>
        /// <value>
        /// The stroke thickness.
        /// </value>
        public double StrokeThickness { get; set; }

        /// <summary>
        /// Gets or sets the stroke dash array.
        /// </summary>
        /// <value>
        /// The stroke dash array.
        /// </value>
        public float[] StrokeDashArray { get; set; }

        /// <summary>
        /// Gets or sets the fill.
        /// </summary>
        /// <value>
        /// The fill.
        /// </value>
        public Brush Fill { get; set; }

        /// <summary>
        /// Gets or sets the font.
        /// </summary>
        /// <value>
        /// The font.
        /// </value>
        public Font Font { get; set; }

        /// <summary>
        /// Gets or sets the content of the label.
        /// </summary>
        /// <value>
        /// The content of the label.
        /// </value>
        public object LabelContent { get; set; }

        /// <summary>
        /// Gets or sets the label vertical alignment.
        /// </summary>
        /// <value>
        /// The label vertical alignment.
        /// </value>
        public VerticalAlignment LabelVerticalAlignment { get; set; }

        /// <summary>
        /// Gets or sets the label horizontal alignment.
        /// </summary>
        /// <value>
        /// The label horizontal alignment.
        /// </value>
        public HorizontalAlignment LabelHorizontalAlignment { get; set; }

        /// <summary>
        /// Gets the view.
        /// </summary>
        /// <value>
        /// The view.
        /// </value>
        public IPlaneSeparatorView View { get; internal set; }

        /// <summary>
        /// Gets or sets the view provider.
        /// </summary>
        /// <value>
        /// The view provider.
        /// </value>
        public IPlaneViewProvider ViewProvider
        {
            get => _viewProvider ?? (_viewProvider = DefaultViewProvider());
            set => _viewProvider = value;
        }

        /// <summary>
        /// Specifies the default ui provider.
        /// </summary>
        /// <returns></returns>
        protected virtual IPlaneViewProvider DefaultViewProvider()
        {
            return Charting.Settings.UiProvider.GetNewSection();
        }

        #region IResource implementation

        /// <inheritdoc />
        public event DisposingResourceHandler Disposed;

        object IResource.UpdateId { get; set; }

        void IResource.Dispose(IChartView view, bool force)
        {
            View.Dispose(view, force);
        }

        #endregion
    }
}