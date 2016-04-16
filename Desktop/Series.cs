using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using LiveChartsCore;

namespace Desktop
{
    public class Series : FrameworkElement, ISeriesView
    {
        public Series()
        {
            Model = new LiveChartsCore.Series();
        }

        public ISeriesModel Model { get; private set; }
    }
}
