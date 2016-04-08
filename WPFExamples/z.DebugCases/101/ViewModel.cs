using System.ComponentModel;
using System.Runtime.CompilerServices;
using LiveCharts.Annotations;

namespace ChartsTest.z.DebugCases._101
{
    public class ViewModel : INotifyPropertyChanged
    {
        private double[] _chartData;

        public double[] ChartData
        {
            get { return _chartData; }
            set
            {
                _chartData = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
