using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Assets.Commands;
using Samples.Wpf.Views;

namespace Samples.Wpf
{
    public class NavigationHandler : INotifyPropertyChanged
    {
        private static readonly Type[] Types = typeof(NavigationHandler).Assembly.GetTypes();
        private bool _isMenuHidden;
        private FrameworkElement _currentView;

        public NavigationHandler()
        {
            CurrentView = new Menu();
            NavigateTo = new DelegateCommand(o =>
            {
                var path = (string) o;
                _navigateTo(path);
            });
            ShowMenu = new DelegateCommand(o =>
            {
                _showMenu();
            });
        }

        public FrameworkElement CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged(nameof(CurrentView));
            }
        }

        public bool IsMenuHidden
        {
            get => _isMenuHidden;
            set
            {
                var changed = _isMenuHidden != value;
                if (!changed) return;
                _isMenuHidden = value;
                OnPropertyChanged(nameof(IsMenuHidden));
            }
        }

        public ICommand NavigateTo { get; }
        public ICommand ShowMenu { get; }

        private void _navigateTo(string view)
        {
            var targetType = Types.First(x => x.Name == view);
            var newView = (FrameworkElement) Activator.CreateInstance(targetType);
            CurrentView = newView;
            IsMenuHidden = view != "Menu";
        }

        private void _showMenu()
        {
            _navigateTo("Menu");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}