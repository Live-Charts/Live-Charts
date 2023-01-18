using System.ComponentModel;
using System.Windows.Controls;

namespace Wpf.CartesianChart.DynamicVisibility
{
    public partial class DynamicVisibilityExample : UserControl, INotifyPropertyChanged
    {
        private bool _mariaSeriesVisibility;
        private bool _charlesSeriesVisibility;
        private bool _johnSeriesVisibility;

        public DynamicVisibilityExample()
        {
            InitializeComponent();

            MariaSeriesVisibility = true;
            CharlesSeriesVisibility = true;
            JohnSeriesVisibility = false;

            DataContext = this;
        }

        public bool MariaSeriesVisibility
        {
            get { return _mariaSeriesVisibility; }
            set
            {
                _mariaSeriesVisibility = value;
                OnPropertyChanged("MariaSeriesVisibility");
            }
        }

        public bool CharlesSeriesVisibility
        {
            get { return _charlesSeriesVisibility; }
            set
            {
                _charlesSeriesVisibility = value;
                OnPropertyChanged("CharlesSeriesVisibility");
            }
        }

        public bool JohnSeriesVisibility
        {
            get { return _johnSeriesVisibility; }
            set
            {
                _johnSeriesVisibility = value;
                OnPropertyChanged("JohnSeriesVisibility");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
