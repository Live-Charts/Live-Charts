using System;

namespace LiveCharts.Core
{
    /// <summary>
    /// An Exception thrown when there is an error related with LiveCharts.
    /// </summary>
    /// <seealso cref="System.Exception" />
    public sealed class LiveChartsException : Exception
    {
        private static readonly string _baseErrorUri = "http://lvcharts.net/error/";

        /// <summary>
        /// Initializes a new instance of the <see cref="LiveChartsException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="code">the error code</param>
        public LiveChartsException(string message, int code)
            : base(message)
        {
            HelpLink = _baseErrorUri + code;
        }
    }
}

