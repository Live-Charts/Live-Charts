using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Input;
using LiveCharts.Core.DataSeries;
using LiveCharts.Core.Defaults;

namespace Samples.Wpf.Views
{
    /// <summary>
    /// Interaction logic for UserInteractiveData.xaml
    /// </summary>
    public partial class UserInteractiveData : UserControl
    {
        public UserInteractiveData()
        {
            InitializeComponent();
        }

        private void Chart_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            // we grab our data context that we specified in the XAML
            var context = (Assets.ViewModels.UserInteractiveData) DataContext;

            var scatterSeries = (ScatterSeries<PointModel>) context.SeriesCollection[1];
            var values = (ObservableCollection<PointModel>) scatterSeries.Values;

            var scaled = Chart.ScaleFromUi(e.GetPosition(Chart));
            values.Add(new PointModel(scaled.X, scaled.Y));
        }
    }
}
