using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using LiveCharts;

namespace Wpf.CartesianChart.ThreadSafe
{
    public class ThreadSafeViewModel
    {
        private double _trend;
        private double _min;
        private double _count;

        public ThreadSafeViewModel()
        {
            Values = new ChartValues<double>();
            ReadCommand = new RelayCommand(Read);
        }

        public ChartValues<double> Values { get; set; }
        public RelayCommand ReadCommand { get; set; }
        public double Min
        {
            get { return _min; }
            set
            {
                _min = value;
                OnPropertyChanged("Min");
            }
        }

        public double Count
        {
            get { return _count; }
            set
            {
                _count = value;
                OnPropertyChanged("Count");
            }
        }

        private void Read()
        {
            //lets keep in memory only the last 200 records,
            //to keep everything running faster
            const int keepRecords = 200;

            Action readFromTread = () =>
            {
                while (true)
                {
                    Thread.Sleep(1);
                    var r = new Random();
                    _trend += (r.NextDouble() < 0.8 ? 1 : -1) * r.Next(0, 10);
                    Values.Add(_trend);
                    Min = Values.Count - keepRecords;
                    if (Values.Count > keepRecords) Values.RemoveAt(0);
                }
                // ReSharper disable once FunctionNeverReturns
            };

            //are 8 task enough adding a new value every ms?
            Task.Factory.StartNew(readFromTread);
            Task.Factory.StartNew(readFromTread);
            Task.Factory.StartNew(readFromTread);
            Task.Factory.StartNew(readFromTread);
            Task.Factory.StartNew(readFromTread);
            Task.Factory.StartNew(readFromTread);
            Task.Factory.StartNew(readFromTread);
            Task.Factory.StartNew(readFromTread);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
