using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveChartsCore
{
    public class LineSeries : Series
    {
        public LineSeries(ISeriesView view) : base(view)
        {
        }

        public LineSeries(ISeriesView view, SeriesConfiguration configuration) : base(view, configuration)
        {
        }
        public override void Update()
        {
            foreach (var chartPoint in Values.Points)
            {
                
            }
        }
    }
}
