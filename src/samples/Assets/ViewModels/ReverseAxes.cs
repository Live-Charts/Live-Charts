using System.Collections.ObjectModel;
using System.ComponentModel;
using LiveCharts.Core.DataSeries;
using LiveCharts.Core.Dimensions;

namespace Assets.ViewModels
{
    public class ReverseAxes
    {
        private bool _reverseX;
        private bool _reverseY;

        public ReverseAxes()
        {
            SeriesCollection = new ObservableCollection<ISeries>
            {
                new BarSeries<double>
                {
                    Values = new ObservableCollection<double>
                    {
                        5,
                        3,
                        8,
                        2,
                        9,
                        5
                    }
                }
            };

            X = new ObservableCollection<Plane>
            {
                new Axis()
            };

            Y = new ObservableCollection<Plane>
            {
                new Axis()
            };
        }

        public ObservableCollection<ISeries> SeriesCollection { get; set; }

        public ObservableCollection<Plane> X { get; set; }

        public ObservableCollection<Plane> Y { get; set; }

        public bool ReverseX
        {
            get => _reverseX;
            set
            {
                _reverseX = value;
                X[0].Reverse = value;
            }
        }

        public bool ReverseY
        {
            get => _reverseY;
            set
            {
                _reverseY = value;
                Y[0].Reverse = value;
            }
        }
    }
}