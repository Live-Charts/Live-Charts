
using System;

namespace LiveChartsCore
{
    public static class Motor
    {
        public static Type Updater;
        private static Action _configuration;

        static Motor()
        {
            Updater = typeof (ChartUpdater);
        }

        public static Action Configuration
        {
            get { return _configuration; }
            set
            {
                _configuration = value;
                _configuration();
            }
        }

        public static IChartUpdater GetUpdater(IChartModel chart)
        {
            return (IChartUpdater) Activator.CreateInstance(Updater, chart);
        }
    }
}
