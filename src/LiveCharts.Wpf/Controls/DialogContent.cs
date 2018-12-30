using System.Windows;
using System.Windows.Controls;
using LiveCharts.Interaction.Controls;

namespace LiveCharts.Wpf.Controls
{
    public class DialogContent : ContentControl
    {
        static DialogContent()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(DialogContent),
                new FrameworkPropertyMetadata(typeof(DialogContent)));
        }

        public static readonly DependencyProperty WedgeProperty = DependencyProperty.Register(
            "Wedge", typeof(double), typeof(DialogContent), 
            new FrameworkPropertyMetadata(75d, FrameworkPropertyMetadataOptions.AffectsRender));

        public double Wedge
        {
            get => (double) GetValue(WedgeProperty);
            set => SetValue(WedgeProperty, value);
        }

        public static readonly DependencyProperty WedgeHypotenuseProperty = DependencyProperty.Register(
            "WedgeHypotenuse", typeof(double), typeof(DialogContent), 
            new FrameworkPropertyMetadata(12d, FrameworkPropertyMetadataOptions.AffectsRender));

        public double WedgeHypotenuse
        {
            get => (double) GetValue(WedgeHypotenuseProperty);
            set => SetValue(WedgeHypotenuseProperty, value);
        }

        public static readonly DependencyProperty PositionProperty = DependencyProperty.Register(
            "Position", typeof(ToolTipPosition), typeof(DialogContent),
            new FrameworkPropertyMetadata(
                ToolTipPosition.Bottom, FrameworkPropertyMetadataOptions.AffectsRender));

        public ToolTipPosition Position
        {
            get => (ToolTipPosition) GetValue(PositionProperty);
            set => SetValue(PositionProperty, value);
        }

        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
            "CornerRadius", typeof(double), typeof(DialogContent), new PropertyMetadata(default(double)));

        public double CornerRadius
        {
            get => (double) GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }
    }
}
