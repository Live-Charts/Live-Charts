using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Assets.Commands;
using Menu = Samples.Wpf.Views.Menu;

namespace Samples.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            NavigationHandler = new NavigationHandler();
            DataContext = NavigationHandler;
        }

        public NavigationHandler NavigationHandler { get; set; }
    }

    public class NavigationHandler : INotifyPropertyChanged
    {
        private static readonly Type[] Types = typeof(NavigationHandler).Assembly.GetTypes();

        public NavigationHandler()
        {
            History = new List<FrameworkElement>
            {
                new Menu()
            };
            NavigateTo = new DelegateCommand(o =>
            {
                var path = (string) o;
                _navigateTo(path);
            });
            GoBack = new DelegateCommand(o => _goBack());
        }

        public FrameworkElement CurrentView => History[History.Count - 1];

        public List<FrameworkElement> History { get; }

        public ICommand NavigateTo { get; private set; }
        public ICommand GoBack { get; private set; }

        private void _navigateTo(string view)
        {
            var targetType = Types.First(x => x.Name == view);
            if (History.Last().GetType() == targetType) return;
            History.Add((FrameworkElement) Activator.CreateInstance(targetType));
            OnPropertyChanged(nameof(CurrentView));
        }

        private void _goBack()
        {
            var previousIndex = History.Count - 1;
            if (previousIndex <= 0) return;
            History.RemoveAt(History.Count - 1);
            OnPropertyChanged(nameof(CurrentView));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
