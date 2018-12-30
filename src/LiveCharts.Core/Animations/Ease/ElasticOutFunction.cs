using System;

namespace LiveCharts.Animations.Ease
{
    /// <summary>
    /// An elsatic function based on https://github.com/d3/d3-ease/blob/master/src/elastic.js
    /// </summary>
    public class ElasticOutFunction : IEasingFunction
    {
        private readonly double _amplitude;
        private readonly double _s;
        private readonly double _frequency;

        /// <summary>
        /// Initializes a new instance of the <see cref="ElasticOutFunction"/> class.
        /// Calculates the progress based on d3 elasticIn function -> https://github.com/d3/d3-ease/blob/master/src/elastic.js#L18
        /// </summary>
        public ElasticOutFunction(double? tau = 2 * Math.PI, double? amplitude = 1, double? period = .3)
        {
            _amplitude = amplitude.Value;
            _frequency = period.Value / tau.Value;
            _s = Math.Asin(1 / _amplitude) * _frequency;
        }

        /// <inheritdoc></inheritdoc>
        public double GetProgress(double t)
        {
            return 1 - _amplitude * Math.Pow(2, -10 * t) * Math.Sin((_s + t) / _frequency);
        }
    }
}
