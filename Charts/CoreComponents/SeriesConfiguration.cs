using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using LiveCharts.Components;

namespace lvc
{
    public class SeriesConfiguration<T> : ISeriesConfiguration
    {
        private int _xIndexer;
        private int _yIndexer;
        public SeriesConfiguration()
        {
            XValueMapper = (value, index) => index;
            OptimizationMethod = values =>
            {
                _xIndexer = 0;
                _yIndexer = 0;
                return values.Select(v => new ChartPoint
                {
                    X = XValueMapper(v, _xIndexer++),
                    Y = YValueMapper(v, _yIndexer++),
                    Instance = v
                });
            };
        }

        /// <summary>
        /// Gets or sets optimization method
        /// </summary>
        internal Func<IEnumerable<T>, IEnumerable<ChartPoint>> OptimizationMethod { get; set; }
       
        /// <summary>
        /// Gets or sets the current function that pulls X value from T
        /// </summary>
        private Func<T, int, double> XValueMapper { get; set; }

        /// <summary>
        /// Gets or sets the current function that pulls Y value from T
        /// </summary>
        private Func<T, int, double> YValueMapper { get; set; }

        /// <summary>
        /// Maps X value
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public SeriesConfiguration<T> X(Func<T, double> predicate)
        {
            XValueMapper = (x, i) => predicate(x);
            return this;
        }

        /// <summary>
        /// Maps Y Value
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public SeriesConfiguration<T> X(Func<T, int, double> predicate)
        {
            XValueMapper = predicate;
            return this;
        }

        /// <summary>
        /// Maps Y Value
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public SeriesConfiguration<T> Y(Func<T, double> predicate)
        {
            YValueMapper = (x, i) => predicate(x);
            return this;
        }

        /// <summary>
        /// Max X Value
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public SeriesConfiguration<T> Y(Func<T, int, double> predicate)
        {
            YValueMapper = predicate;
            return this;
        }

        public SeriesConfiguration<T> HasOptimization(Func<IEnumerable<T>, IEnumerable<ChartPoint>> predicate)
        {
            OptimizationMethod = predicate;
            return this;
        }
    }
}