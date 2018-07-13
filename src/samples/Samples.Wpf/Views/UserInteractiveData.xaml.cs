using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using LiveCharts.Core.Charts;
using LiveCharts.Core.DataSeries;
using LiveCharts.Core.Defaults;
using LiveCharts.Core.Interaction.Points;
#if GEARED
using LiveCharts.Wpf.Geared.Rendering.Gemini.Framework.Controls;
#endif

namespace Samples.Wpf.Views
{
    /// <summary>
    /// Interaction logic for UserInteractiveData.xaml
    /// </summary>
    public partial class UserInteractiveData : UserControl
    {
        private PointModel _draggingModel;

        public UserInteractiveData()
        {
            InitializeComponent();

#if GEARED
            var content = (LiveCharts.Wpf.Geared.Controls.ChartContent) Chart.Content;
            content.HwndLButtonDown += OnContentOnHwndLButtonDown;
            content.HwndMouseMove += ContentOnHwndMouseMove;
            content.HwndLButtonUp += ContentOnHwndLButtonUp;
#else
            MouseDown += Chart_OnMouseDown;
            MouseMove += Chart_OnMouseMove;
            MouseUp += Chart_OnMouseUp;
#endif
        }

        private void Chart_OnDataMouseDown(
            IChartView chart, IChartPoint[] interactedPoints, EventArgs args)
        {
            // if the user clicked over a data point
            // handle the event so Chart_OnMouseDown is not called.
#if GEARED
            var mbea = (HwndMouseEventArgs) args;
            mbea.Handled = true;
#else
            var mbea = (MouseButtonEventArgs) args;
            mbea.Handled = true;
#endif
            // let save a reference to the point model that was clicked.
            _draggingModel = (PointModel) interactedPoints.FirstOrDefault()?.Model;
        }

#if GEARED
        private void OnContentOnHwndLButtonDown(object sender, HwndMouseEventArgs e)
        {
            var pointerLocation = e.GetPosition(Chart);
            AddPointAt(pointerLocation);
        }

        private void ContentOnHwndMouseMove(object sender, HwndMouseEventArgs e)
        {
            var pointerLocation = e.GetPosition(Chart);
            OnPointDragging(pointerLocation);
        }

        private void ContentOnHwndLButtonUp(object sender, HwndMouseEventArgs hwndMouseEventArgs)
        {
            _draggingModel = null;
        }
#else
        private void Chart_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            var pointerLocation = e.GetPosition(Chart);
            AddPointAt(pointerLocation);
        }

        private void Chart_OnMouseMove(object sender, MouseEventArgs e)
        {
            var pointerLocation = e.GetPosition(Chart);
            OnPointDragging(pointerLocation);
        }

        private void Chart_OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            _draggingModel = null;
        }
#endif

        private void AddPointAt(Point pointerLocation)
        {
            // we grab our data context that we specified in the XAML
            var context = (Assets.ViewModels.UserInteractiveData)DataContext;

            ScatterSeries<PointModel> scatterSeries = (ScatterSeries<PointModel>)context.SeriesCollection[0];
            ObservableCollection<PointModel> values = (ObservableCollection<PointModel>)scatterSeries.Values;

            var scaled = Chart.ScaleFromUi(pointerLocation);
            values.Add(new PointModel(scaled.X, scaled.Y));
        }

        private void OnPointDragging(Point pointerLocation)
        {
            if (_draggingModel == null) return;

            var scaled = Chart.ScaleFromUi(pointerLocation);

            _draggingModel.X = scaled.X;
            _draggingModel.Y = scaled.Y;
        }
    }
}
