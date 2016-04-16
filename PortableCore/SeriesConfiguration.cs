using System;

namespace LiveChartsCore
{
    public class SeriesConfiguration<T> : SeriesConfiguration
    {
        public SeriesConfiguration()
        {
            XValueMapper = (value, index) => index;
            YValueMapper = (value, index) => index;
        }

        /// <summary>
        /// Gets or sets optimization method
        /// </summary>
        public IDataOptimization<T> DataOptimization { get; set; }

        /// <summary>
        /// Gets or sets the current function that pulls X value from T
        /// </summary>
        internal Func<T, int, double> XValueMapper { get; set; }

        /// <summary>
        /// Gets or sets the current function that pulls Y value from T
        /// </summary>
        internal Func<T, int, double> YValueMapper { get; set; }

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

        public SeriesConfiguration<T> HasHighPerformanceMethod(IDataOptimization<T> optimization)
        {
            DataOptimization = optimization;
            return this;
        }
    }

    public class SeriesConfiguration
    {
        
    }
}