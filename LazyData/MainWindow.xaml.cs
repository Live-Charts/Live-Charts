using System.Windows;
using LC_Demo;

namespace LazyData
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainWindowViewModel _viewModel;
        public MainWindow()
        {
            _viewModel = new MainWindowViewModel();

            InitializeComponent();

            DataContext = _viewModel;
        }

        private void FetchDataOnClick(object sender, RoutedEventArgs e)
        {
            //I don´t know why the binding is not working correcty.
            //this line fixes is but I really dont know why it does not detects the change by itself,
            //SecondaryAxis.Labels is a dependency property
            //note: this breaks MVVM

            //SecondaryAxis.Labels = new[] { "w", "x", "y", "z" };

            //this only works when called twice
            //SecondaryAxis.Labels = _viewModel.DummyLabels;

            _viewModel.FetchData();
        }
    }
}
