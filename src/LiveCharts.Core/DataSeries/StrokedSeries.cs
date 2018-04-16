using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Abstractions.DataSeries;
using LiveCharts.Core.Charts;
using LiveCharts.Core.Drawing;
using Brush = LiveCharts.Core.Drawing.Brush;

namespace LiveCharts.Core.DataSeries
{
    /// <summary>
    /// An <see cref="ISeries"/> that has <see cref="Stroke"/> and <see cref="Fill"/> properties.
    /// </summary>
    /// <typeparam name="TModel">The type of the model.</typeparam>
    /// <typeparam name="TCoordinate">The type of the coordinate.</typeparam>
    /// <typeparam name="TViewModel">The type of the view model.</typeparam>
    /// <typeparam name="TSeries">The type of the series.</typeparam>
    /// <seealso cref="Series{TModel, TCoordinate, TViewModel, TSeries}" />
    /// <seealso cref="IStrokeSeries" />
    public abstract class StrokeSeries<TModel, TCoordinate, TViewModel, TSeries> 
        : Series<TModel, TCoordinate, TViewModel, TSeries>, IStrokeSeries
        where TCoordinate : ICoordinate
        where TSeries : class, ISeries
    {
        private Brush _stroke;
        private float _strokeThickness;
        private Brush _fill;
        private IEnumerable<double> _strokeDashArray;

        /// <inheritdoc />
        protected StrokeSeries()
        {
            Charting.BuildFromSettings<IStrokeSeries>(this);
        }

        /// <inheritdoc />
        public Brush Stroke
        {
            get => _stroke;
            set
            {
                _stroke = value;
                OnPropertyChanged();
            }
        }

        /// <inheritdoc />
        public float StrokeThickness
        {
            get => _strokeThickness;
            set
            {
                _strokeThickness = value;
                OnPropertyChanged();
            }
        }

        /// <inheritdoc />
        public IEnumerable<double> StrokeDashArray
        {
            get => _strokeDashArray;
            set
            {
                _strokeDashArray = value;
                OnPropertyChanged();
            }
        }

        /// <inheritdoc />
        public Brush Fill
        {
            get => _fill;
            set
            {
                _fill = value;
                OnPropertyChanged();
            }
        }

        /// <inheritdoc />
        public override SeriesStyle Style
        {
            get
            {
                return new SeriesStyle
                {
                    Fill = Fill,
                    Stroke = Stroke,
                    StrokeThickness = StrokeThickness,
                    StrokeDashArray = StrokeDashArray?.Select(x => (float) x)
                };
            }
        }

        /// <inheritdoc />
        protected override void SetDefaultColors(ChartModel chart)
        {
            if (!(Stroke == null || Fill == null)) return;

            var nextColor = chart.GetNextColor();

            if (Stroke == null)
            {
                Stroke = new SolidColorBrush(nextColor);
            }

            if (Fill == null)
            {
                Fill = new SolidColorBrush(nextColor.SetOpacity(DefaultFillOpacity));
            }
        }
    }
}