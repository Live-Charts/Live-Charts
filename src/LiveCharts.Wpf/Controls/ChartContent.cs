using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Drawing;

namespace LiveCharts.Wpf.Controls
{
    /// <inheritdoc cref="IChartContent" />
    public class ChartContent : Canvas, IChartContent
    {
        private Rectangle _drawArea;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChartContent"/> class.
        /// </summary>
        public ChartContent()
        {
            TooltipPopup = new Popup
            {
                AllowsTransparency = true,
                Placement = PlacementMode.RelativePoint
            };
            Children.Add(TooltipPopup);
        }

        /// <summary>
        /// Gets or sets the tooltip popup.
        /// </summary>
        /// <value>
        /// The tooltip popup.
        /// </value>
        public Popup TooltipPopup { get; set; }

        /// <inheritdoc />
        public Rectangle DrawArea
        {
            get => _drawArea;
            set
            {
                _drawArea = value;
                SetTop(this, _drawArea.Top);
                SetLeft(this, _drawArea.Left);
                Width = _drawArea.Width;
                Height = _drawArea.Height;
            }
        }

        /// <inheritdoc />
        public void AddChild(object child)
        {
            Children.Add((UIElement) child);
        }

        /// <inheritdoc />
        public void MoveChild(object child, params double[] vector)
        {
            SetTop((UIElement) child, vector[0]);
            SetLeft((UIElement) child, vector[1]);
        }

        /// <inheritdoc />
        public void RemoveChild(object child)
        {
            Children.Remove((UIElement) child);
        }
    }
}