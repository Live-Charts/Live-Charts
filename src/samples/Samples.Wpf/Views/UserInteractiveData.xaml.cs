using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using LiveCharts.Core.Charts;
using LiveCharts.Core.DataSeries;
using LiveCharts.Core.Defaults;
using LiveCharts.Core.Interaction.Points;

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

            var scatterSeries = (ScatterSeries<PointModel>) context.SeriesCollection[0];
            var values = (ObservableCollection<PointModel>) scatterSeries.Values;

            var scaled = Chart.ScaleFromUi(e.GetPosition(Chart));
            values.Add(new PointModel(scaled.X, scaled.Y));
        }

        private PointModel _draggingModel;

        private void Chart_OnDataMouseDown(
            IChartView chart, IChartPoint[] interactedPoints, EventArgs args)
        {
            var mbea = (MouseButtonEventArgs) args;

            // if the user clicked over a data point
            // handle the event so Chart_OnMouseDown is not called.
            mbea.Handled = true;

            // let save a reference to the point model that was clicked.
            _draggingModel = (PointModel) interactedPoints.FirstOrDefault()?.Model;
        }

        private void Chart_OnMouseMove(object sender, MouseEventArgs e)
        {
            if (_draggingModel == null) return;

            var scaled = Chart.ScaleFromUi(e.GetPosition(Chart));

            _draggingModel.X = scaled.X;
            _draggingModel.Y = scaled.Y;
        }

        private void Chart_OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            _draggingModel = null;
        }
    }
}
