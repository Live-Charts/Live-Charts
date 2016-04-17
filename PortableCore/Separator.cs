using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveChartsCore
{
    public class Separator: ISeparatorModel
    {
        public Separator(ISeparatorView view)
        {
            View = view;
        }

        public ISeparatorView View { get; set; }
        public bool IsEnabled { get; set; }
        public int StrokeThickness { get; set; }
        public double? Step { get; set; }
    }
}
