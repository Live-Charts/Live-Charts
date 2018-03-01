using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Assets.Models
{
    public class City: INotifyPropertyChanged
    {
        private float _population;

        public float Population
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