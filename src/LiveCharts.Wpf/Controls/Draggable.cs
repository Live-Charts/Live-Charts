using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LiveCharts.Wpf.Controls
{
    /// <summary>
    /// The scroller class.
    /// </summary>
    /// <seealso cref="System.Windows.Controls.ContentControl" />
    public abstract class Draggable : ContentControl
    {
        private double _top;
        private double _left;

        static Draggable()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(Draggable),
                new FrameworkPropertyMetadata(typeof(Draggable)));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Draggable"/> class.
        /// </summary>
        protected Draggable()
        {
            MouseDown += OnMouseDown;
            MouseMove += OnMouseMove;
            MouseUp += OnMouseUp;
        }

        /// <summary>
        /// Occurs when [dragging].
        /// </summary>
        public event DraggableHandler Dragging;

        /// <summary>
        /// Gets or sets the left.
        /// </summary>
        /// <value>
        /// The left.
        /// </value>
        public double Left
        {
            get
            {
                if (HorizontalAlignment == HorizontalAlignment.Center)
                {
                    return _left - ActualWidth * .5;
                }

                return _left;
            }
            set
            {
                _left = value;
                if (HorizontalAlignment == HorizontalAlignment.Center)
                {
                    _left -= ActualWidth * .5;
                }
                Canvas.SetLeft(this, _left);
            }
        }

        /// <summary>
        /// Gets or sets the top.
        /// </summary>
        /// <value>
        /// The top.
        /// </value>
        public double Top
        {
            get
            {
                if (VerticalAlignment == VerticalAlignment.Center)
                {
                    return _top - ActualHeight * .5;
                }

                return _top;
            }
            set
            {
                _top = value;
                if (VerticalAlignment == VerticalAlignment.Center)
                {
                    _top -= ActualHeight * .5;
                }
                Canvas.SetTop(this, _top);
            }
        }

        /// <summary>
        /// Gets the start left offset.
        /// </summary>
        /// <value>
        /// The start left offset.
        /// </value>
        public double StartLeftOffset { get; private set; }

        /// <summary>
        /// Gets the start top offset.
        /// </summary>
        /// <value>
        /// The start top offset.
        /// </value>
        public double StartTopOffset { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is dragging.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is dragging; otherwise, <c>false</c>.
        /// </value>
        public bool IsDragging { get; private set; }

        /// <summary>
        /// The corner radius property
        /// </summary>
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
            nameof(CornerRadius), typeof(double), typeof(Draggable), new PropertyMetadata(default(double)));

        /// <summary>
        /// Gets or sets the corner radius.
        /// </summary>
        /// <value>
        /// The corner radius.
        /// </value>
        public double CornerRadius
        {
            get => (double) GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        private void OnMouseDown(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            IsDragging = true;
            var pos = mouseButtonEventArgs.GetPosition((IInputElement) Parent);
            StartLeftOffset = pos.X - Left;
            StartTopOffset = pos.Y - Top;
            mouseButtonEventArgs.Handled = true;
            CaptureMouse();
        }

        private void OnMouseMove(object sender, MouseEventArgs mouseEventArgs)
        {
            if (!IsDragging) return;
           mouseEventArgs.Handled = true;
            var position = mouseEventArgs.GetPosition((IInputElement) Parent);

            var args = new DraggableArgs
            {
                Point = position
            };

            OnDragging(args);
        }

        private void OnMouseUp(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            if (IsDragging)
            {
                mouseButtonEventArgs.Handled = true;
                ReleaseMouseCapture();
            }

            StartLeftOffset = 0;
            StartTopOffset = 0;
            IsDragging = false;
        }

        /// <summary>
        /// Called when [dragging event].
        /// </summary>
        /// <param name="args">The object.</param>
        protected virtual void OnDragging(DraggableArgs args)
        {
            Dragging?.Invoke(args);
        }
    }
}
