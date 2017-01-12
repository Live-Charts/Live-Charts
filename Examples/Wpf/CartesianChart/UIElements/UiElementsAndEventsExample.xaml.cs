using System;
using System.Windows.Controls;
using System.Windows.Input;
using LiveCharts;
using LiveCharts.Wpf;

namespace Wpf.CartesianChart.UIElements
{
    public partial class UiElementsAndEventsExample : UserControl
    {
        public UiElementsAndEventsExample()
        {
            InitializeComponent();

            DataContext = new ViewModel();
        }

        //using Chart.DataClick event
        private void ChartOnDataClick(object sender, ChartPoint p)
        {
            var asPixels = Chart.ConvertToPixels(p.AsPoint());
            Console.WriteLine("[EVENT] You clicked (" + p.X + ", " + p.Y + ") in pixels (" +
                            asPixels.X + ", " + asPixels.Y + ")");
        }

        //using Chart.DataHover event
        private void Chart_OnDataHover(object sender, ChartPoint p)
        {
            Console.WriteLine("[EVENT] you hovered over " + p.X + ", " + p.Y);
        }

        private void ChartMouseMove(object sender, MouseEventArgs e)
        {
            var point = Chart.ConvertToChartValues(e.GetPosition(Chart));

            X.Text = point.X.ToString("N");
            Y.Text = point.Y.ToString("N");
        }
    }

    public class ViewModel
    {
        public ViewModel()
        {
            DataClickCommand = new ChartPointCommandHandler(
                p => Console.WriteLine("[COMMAND] You clicked " + p.X + ", " + p.Y));
            DataHoverCommand = new ChartPointCommandHandler(
                p => Console.WriteLine("[COMMAND] You hovered over " + p.X + ", " + p.Y));
        }

        public ChartPointCommandHandler DataHoverCommand { get; set; }
        public ChartPointCommandHandler DataClickCommand { get; set; }
    }

    public class ChartPointCommandHandler : ICommand
    {
        private Action<ChartPoint> _action;
         
        public ChartPointCommandHandler(Action<ChartPoint> action)
        {
            _action = action;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _action((ChartPoint) parameter);
        }

        public event EventHandler CanExecuteChanged;
    }
}
