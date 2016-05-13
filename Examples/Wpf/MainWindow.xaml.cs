using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using LiveCharts.Wpf;
using Wpf.Annotations;
using Wpf.CartesianChart;

namespace Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : INotifyPropertyChanged
    {
        private UserControl _cartesianView;

        public MainWindow()
        {
            InitializeComponent();

            CartesianExamples = new List<UserControl>
            {
                new Welcome(), new ResponsiveExample(), new CustomTypesPlotting(),
                new LineExample(), new BarExample(), new BubblesExample(),
                new StackedAreaExample(), new FinancialExample(), new StackedBarExample(),
                new SectionsExample(), new MixingTypes()
            };

            CartesianView = CartesianExamples != null && CartesianExamples.Count > 0 ? CartesianExamples[0] : null;

            DataContext = this;
        }

        private void Hyperlink_OnRequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

        private void NextCartesianOnClick(object sender, MouseButtonEventArgs e)
        {
            if (CartesianView == null) return;
            var current = CartesianExamples.IndexOf(CartesianView);
            current++;
            CartesianView = CartesianExamples.Count > current ? CartesianExamples[current] : CartesianExamples[0];
        }

        private void PreviousCartesianOnClick(object sender, MouseButtonEventArgs e)
        {
            if (CartesianView == null) return;
            var current = CartesianExamples.IndexOf(CartesianView);
            current--;
            CartesianView = current >= 0 ? CartesianExamples[current] : CartesianExamples[CartesianExamples.Count-1];
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public UserControl CartesianView
        {
            get { return _cartesianView; }
            set
            {
                _cartesianView = value;
                OnPropertyChanged();
            }
        }

        public List<UserControl> CartesianExamples { get; set; }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
