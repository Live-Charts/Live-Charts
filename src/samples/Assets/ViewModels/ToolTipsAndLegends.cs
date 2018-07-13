using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using LiveCharts.Core.DataSeries;
using LiveCharts.Core.Interaction.Controls;

namespace Assets.ViewModels
{
    public class ToolTipsAndLegends : INotifyPropertyChanged
    {
        private LegendPosition _legendPosition;
        private ToolTipPosition _toolTipPosition;
        private ToolTipSelectionMode _toolTipSelectionMode;
        private bool _snapToClosest;

        public ToolTipsAndLegends()
        {
            SeriesCollection = new ObservableCollection<ISeries>
            {
                new LineSeries<double>
                {
                    Values = new ObservableCollection<double> {4, 7, 2, 7, 4, 8, 5}
                },
                new LineSeries<double>
                {
                    Values = new ObservableCollection<double> {8, 2, 6, 4, 2, 9, 5}
                }
            };

            LegendPosition = LegendPosition.Left;
            ToolTipPosition = ToolTipPosition.Top;
            ToolTipSelectionMode = ToolTipSelectionMode.SharedXy;
            SnapToClosest = false;
        }

        public ObservableCollection<ISeries> SeriesCollection { get; set; }

        public LegendPosition LegendPosition
        {
            get => _legendPosition;
            set
            {
                _legendPosition = value;
                OnPropertyChanged();
            }
        }

        public ToolTipPosition ToolTipPosition
        {
            get => _toolTipPosition;
            set
            {
                _toolTipPosition = value;
                OnPropertyChanged();
            }
        }

        public ToolTipSelectionMode ToolTipSelectionMode
        {
            get => _toolTipSelectionMode;
            set
            {
                _toolTipSelectionMode = value;
                OnPropertyChanged();
            }
        }

        public bool SnapToClosest
        {
            get => _snapToClosest;
            set
            {
                _snapToClosest = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}