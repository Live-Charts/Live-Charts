using System;
using System.Windows.Input;
using LiveCharts;

namespace Wpf.PieChart.DropDowns
{
    public class DropDownCommand : ICommand
    {
        private Action<DropDownPoint> _action;

        public DropDownCommand(Action<DropDownPoint> action)
        {
            _action = action;
        }

        public void Execute(object parameter)
        {
            var chartPoint = (ChartPoint) parameter;
            var dropDownPoint = (DropDownPoint) chartPoint.Instance;

            _action(dropDownPoint);
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;
    }
}