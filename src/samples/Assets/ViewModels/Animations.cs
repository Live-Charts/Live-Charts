using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using LiveCharts.Core.Animations;
using LiveCharts.Core.DataSeries;

namespace Assets.ViewModels
{
    public class Animations : INotifyPropertyChanged
    {
        private IEnumerable<Frame> _timeLine;

        public Animations()
        {
            SeriesCollection = new ObservableCollection<ISeries>
            {
                new LineSeries<double>
                {
                    Values = new ObservableCollection<double>
                    {
                        1,
                        7,
                        2,
                        4
                    }
                },
                new BarSeries<double>
                {
                    Values = new ObservableCollection<double>
                    {
                        5,
                        2,
                        8,
                        2
                    }
                }
            };

            TimeLine = TimeLines.Ease;

            PredefinedTimeLines = new[]
            {
                TimeLines.DisableAnimations,
                TimeLines.Lineal,
                TimeLines.Ease,
                TimeLines.EaseIn,
                TimeLines.EaseOut,
                TimeLines.EaseInOut,
                TimeLines.BounceSmall,
                TimeLines.BounceMedium,
                TimeLines.BounceLarge,
                TimeLines.BounceExtraLarge
            };
        }

        public ObservableCollection<ISeries> SeriesCollection { get; set; }

        public IEnumerable<Frame> TimeLine
        {
            get => _timeLine;
            set
            {
                _timeLine = value;
                OnPropertyChanged();
            }
        }

        public IEnumerable<Frame>[] PredefinedTimeLines { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}