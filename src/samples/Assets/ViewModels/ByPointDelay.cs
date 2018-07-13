using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using LiveCharts.Core.Animations;
using LiveCharts.Core.DataSeries;

namespace Assets.ViewModels
{
    public class ByPointDelay : INotifyPropertyChanged
    {
        private DelayRules _delayRule;

        public ByPointDelay()
        {
            SeriesCollection = new ObservableCollection<ISeries>
            {
                new BarSeries<double>
                {
                    Values = new ObservableCollection<double> {3, 7, 1, 9, 4, 3, 3, 9, 3, 9, 4, 0, 6, 7, 8},
                    DelayRule = DelayRules.LeftToRight
                }
            };

            DelayRule = DelayRules.LeftToRight;
        }

        public ObservableCollection<ISeries> SeriesCollection { get; set; }

        public DelayRules DelayRule
        {
            get => _delayRule;
            set
            {
                _delayRule = value;
                SeriesCollection[0].DelayRule = value;
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
