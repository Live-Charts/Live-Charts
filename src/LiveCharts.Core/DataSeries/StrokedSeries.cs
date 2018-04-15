using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using LiveCharts.Core.Abstractions;
using LiveCharts.Core.Abstractions.DataSeries;
using LiveCharts.Core.Charts;
using LiveCharts.Core.Drawing;

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
        private Color _stroke;
        private float _strokeThickness;
        private Color _fill;
        private IEnumerable<double> _strokeDashArray;

        /// <inheritdoc />
        protected StrokeSeries()
        {
            Charting.BuildFromSettings<IStrokeSeries>(this);
        }

        /// <inheritdoc />
        public Color Stroke
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
        public Color Fill
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
                    StrokeDashArray = StrokeDashArray.Select(x => (float) x)
                };
            }
        }

        /// <inheritdoc />
        protected override void SetDefaultColors(ChartModel chart)
        {
            if (!(Stroke == Color.Empty || Fill == Color.Empty)) return;

            var nextColor = chart.GetNextColor();

            if (Stroke == Color.Empty)
            {
                Stroke = nextColor;
            }

            if (Fill == Color.Empty)
            {
                Fill = nextColor.SetOpacity(DefaultFillOpacity);
            }
        }
    }
}