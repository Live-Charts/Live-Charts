using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Media;
using LiveCharts;
using LiveCharts.Helpers;
using LiveCharts.Wpf;

namespace Wpf.PieChart.DropDowns
{
    public class DropDownViewModel : INotifyPropertyChanged
    {
        private SeriesCollection _series;

        public DropDownViewModel()
        {
            var navigation = new List<SeriesCollection>();
            var initialValues = DataProvider.Values.ToArray();

            Series = GroupSeriesByTheshold(content: initialValues, threshold: initialValues.Max() * .7);

            SliceClickCommand = new DropDownCommand(dropDownPoint =>
            {
                //if the point has no content to display...
                if (dropDownPoint.Content.Length == 1) return;

                navigation.Add(Series.Select(x => new PieSeries
                {
                    Values = x.Values,
                    Title = x.Title,
                    Fill = ((Series) x).Fill
                }).AsSeriesCollection());

                Series = GroupSeriesByTheshold(content: dropDownPoint.Content, threshold: dropDownPoint.Content.Max() * .7);
            });

            GoBackCommand = new RelayCommand(() =>
            {
                if (!navigation.Any())return;
                var previous = navigation.Last();
                if(previous == null) return;
                Series = previous;
                navigation.Remove(previous);
            });

            Formatter = x => x.ToString("N1");
        }

        public SeriesCollection Series
        {
            get { return _series; }
            set
            {
                _series = value;
                OnPropertyChanged("Series");
            }
        }
        public DropDownCommand SliceClickCommand { get; set; }
        public RelayCommand GoBackCommand { get; set; }
        public Func<double, string> Formatter { get; set; }
        
        private static SeriesCollection GroupSeriesByTheshold(double[] content, double threshold)
        {
            var series = content
                .Where(x => x > threshold)
                .Select((value, index) => new PieSeries
                {
                    Values = new ChartValues<DropDownPoint>
                    {
                        new DropDownPoint {Content = new[] {value}}
                    },
                    Title = "Series " + index
                }).AsSeriesCollection();

            var thresholdSeries = new PieSeries
            {
                Values = new ChartValues<DropDownPoint>
                {
                    new DropDownPoint
                    {
                        Content = content.Where(x => x < threshold).ToArray()
                    }
                },
                Title = "DropDown Slice",
                Fill = new SolidColorBrush(Color.FromRgb(30,30,30)),
                PushOut = 5
            };
            series.Add(thresholdSeries);

            return series;
        }

        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}