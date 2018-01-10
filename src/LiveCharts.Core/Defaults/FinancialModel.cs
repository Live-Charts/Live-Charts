using System.ComponentModel;
using System.Runtime.CompilerServices;
using LiveCharts.Core.Data;
using LiveCharts.Core.Data.Points;

namespace LiveCharts.Core.Defaults
{
    public class ReferenceChartModel<TModel, TCoordinate, TViewModel, TChartPoint>
        : IReferenceChartPoint<TModel, TCoordinate, TViewModel, TChartPoint>
        where TChartPoint : ChartPoint<TModel, TCoordinate, TViewModel>, new()
    {
        public TChartPoint ChartPoint { get; set; }
    }

    /// <summary>
    /// Defines an observable financial point, this object notifies the chart to update when any property change.
    /// </summary>
    /// <seealso cref="System.ComponentModel.INotifyPropertyChanged" />
    public class FinancialModel : INotifyPropertyChanged
    {
        private double _open;
        private double _high;
        private double _low;
        private double _close;

        /// <summary>
        /// Initializes a new instance of the <see cref="FinancialModel"/> class.
        /// </summary>
        public FinancialModel()
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FinancialModel"/> class.
        /// </summary>
        /// <param name="open">The open.</param>
        /// <param name="high">The high.</param>
        /// <param name="low">The low.</param>
        /// <param name="close">The close.</param>
        public FinancialModel(double open, double high, double low, double close)
        {
            Open = open;
            High = high;
            Low = low;
            Close = close;
        }

        /// <summary>
        /// Gets or sets the open value.
        /// </summary>
        /// <value>
        /// The open.
        /// </value>
        public double Open
        {
            get => _open;
            set
            {
                _open = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the high value.
        /// </summary>
        /// <value>
        /// The high.
        /// </value>
        public double High
        {
            get => _high;
            set
            {
                _high = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the low value.
        /// </summary>
        /// <value>
        /// The low.
        /// </value>
        public double Low
        {
            get => _low;
            set
            {
                _low = value; 
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets the close value.
        /// </summary>
        /// <value>
        /// The close.
        /// </value>
        public double Close
        {
            get => _close;
            set
            {
                _close = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Occurs when [property changed].
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Called when Value property changes.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}