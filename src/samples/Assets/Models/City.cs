using System.ComponentModel;
using System.Runtime.CompilerServices;
using LiveCharts.Core.Data;

namespace Assets.Models
{
    public class City: INotifyPropertyChanged
    {
        private double _population;

        public double Population
        {
            get => _population;
            set
            {
                _population = value;
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