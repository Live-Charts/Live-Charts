using System.Collections.ObjectModel;
using System.ComponentModel;
using LiveCharts;

namespace LC_Demo
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public MainWindowViewModel()
        {
            //Expected behavior: I should not be forced to initialize empty collections
            //_dummySeries = new ObservableCollection<Series> ();
            //_dummyLabels = new[] { "" };

            //Fixed
        }

        public void FetchData()
        {
            //labels comes from data too, so I expect to be refreshed with databinding}
            DummyLabels = new[] { "w", "x", "y", "z" };
            
            //my data comes from a DB
            DummySeries = new ObservableCollection<Series>
            {
                 new LineSeries
                {
                    Title = "Demo",
                    PrimaryValues = new ObservableCollection<double>(new[] {2d, 4, 7, 1, 5})
                }
            };
        }

        private ObservableCollection<Series> _dummySeries;
        public ObservableCollection<Series> DummySeries
        {
            get { return _dummySeries; }
            set
            {
                _dummySeries = value;
                OnPropertyChanged("DummySeries");
            }
        }

        private string[] _dummyLabels;
        public string[] DummyLabels
        {
            get { return _dummyLabels; }
            set
            {
                _dummyLabels = value;
                OnPropertyChanged("DummyLabels");
            }
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
