using System.Globalization;
using System.Windows;
using System.Windows.Media;
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Coordinates;
using LiveCharts.Core.Data;
using LiveCharts.Core.ViewModels;
using LiveCharts.Wpf.PointViews;
using LiveCharts.Wpf.Separators;
using Brushes = System.Windows.Media.Brushes;
using Font = LiveCharts.Core.Abstractions.Font;
using Rectangle = System.Windows.Shapes.Rectangle;
using Size = LiveCharts.Core.Drawing.Size;

namespace LiveCharts.Wpf
{
    /// <summary>
    /// Default WPF UI builder.
    /// </summary>
    public class UiProvider : IUiProvider
    {
        public Size MeasureControl(string context, Font font, object control)
        {
            var formattedText = new FormattedText(
                context,
                CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                font.AsTypeface(),
                font.Size,
                Brushes.Black
            );
            return new Size(formattedText.Width, formattedText.Height);
        }

        public IPlaneLabelControl AxisLabelProvider()
        {
            return new DefaultPlaneLabelControl();
        }

        public IDataLabelControl<TModel, TCoordinate, TViewModel> DataLabelProvider<TModel, TCoordinate, TViewModel>()
            where TCoordinate : ICoordinate
        {
            return new DefaultDataLabel<TModel, TCoordinate, TViewModel>();
        }

        public ISeparator AxisSeparatorProvider()
        {
            return new SeparatorView<DefaultPlaneLabelControl>();
        }

        public IPointView<TModel, Point<TModel, Point2D, ColumnViewModel>, Point2D, ColumnViewModel>
            ColumnViewProvider<TModel>()
        {
            return new ColumnPointView<
                TModel,
                Point<TModel, Point2D, ColumnViewModel>,
                Point2D,
                ColumnViewModel,
                Rectangle,
                DefaultDataLabel<TModel, Point2D, ColumnViewModel>>();
        }
    }
}
