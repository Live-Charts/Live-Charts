using System;
using System.ComponentModel;

namespace Wpf.CartesianChart.SectionsMouseMove
{
    public class ViewModel : INotifyPropertyChanged
    {
        private double _xPointer;
        private double _yPointer;

        public ViewModel()
        {
            //lets initialize in an invisible location
            XPointer = -5;
            YPointer = -5;

            //the formatter or labels property is shared 
            Formatter = x => x.ToString("N2");
        }

        public double XPointer
        {
            get { return _xPointer; }
            set
            {
                _xPointer = value;
                OnPropertyChanged("XPointer");
            }
        }

        public double YPointer
        {
            get { return _yPointer; }
            set
            {
                _yPointer = value;
                OnPropertyChanged("YPointer");
            }
        }

        public Func<double, string> Formatter { get; set; }

        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null) PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
