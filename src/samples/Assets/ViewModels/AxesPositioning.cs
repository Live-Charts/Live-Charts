using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using LiveCharts.Core.DataSeries;
using LiveCharts.Core.Dimensions;

namespace Assets.ViewModels
{
    public class AxisPositioning : INotifyPropertyChanged
    {
        private AxisPosition _xPosition;
        private AxisPosition _yPosition;
        private bool _invert;
        private double _yRotation;
        private double _xRotation;

        public AxisPositioning()
        {
            SeriesCollection = new List<ISeries>
            {
                new LineSeries<double>
                {
                    Values = new List<double>
                    {
                        6,
                        3,
                        9,
                        2
                    }
                }
            };

            VerticalPositions = new List<AxisPosition>
            {
                AxisPosition.Top,
                AxisPosition.Bottom
            };

            HorizontalPositions = new List<AxisPosition>
            {
                AxisPosition.Left,
                AxisPosition.Right
            };

            X = new List<Plane>
            {
                new Axis()
            };

            Y = new List<Plane>
            {
                new Axis()
            };
        }

        public List<Plane> X { get; set; }

        public List<Plane> Y { get; set; }

        public AxisPosition XPosition
        {
            get => _xPosition;
            set
            {
                _xPosition = value;
                ((Axis) X[0]).Position = value;
            }
        }

        public AxisPosition YPosition
        {
            get => _yPosition;
            set
            {
                _yPosition = value;
                ((Axis) Y[0]).Position = value;
            }
        }

        public double XRotation
        {
            get => _xRotation;
            set
            {
                _xRotation = value;
                ((Axis) X[0]).LabelsRotation = value;
                OnPropertyChanged();
            }
        }

        public double YRotation
        {
            get => _yRotation;
            set
            {
                _yRotation = value;
                ((Axis) Y[0]).LabelsRotation = value;
                OnPropertyChanged();
            }
        }

        public List<AxisPosition> VerticalPositions { get; set; }

        public List<AxisPosition> HorizontalPositions { get; set; }

        public bool Invert
        {
            get => _invert;
            set
            {
                _invert = value;
                OnPropertyChanged();
            }
        }

        public List<ISeries> SeriesCollection { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}