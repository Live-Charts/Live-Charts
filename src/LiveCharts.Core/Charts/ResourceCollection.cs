using System;
using System.Collections;
using System.Linq;

namespace LiveCharts.Charts
{
    internal class EnumerableResource
    {
        /// <summary>
        /// Gets or sets the collection.
        /// </summary>
        /// <value>
        /// The collection.
        /// </value>
        public IEnumerable Collection { get; set; } = Enumerable.Empty<object>();

        /// <summary>
        /// Gets or sets the update identifier.
        /// </summary>
        /// <value>
        /// The update identifier.
        /// </value>
        public object UpdateId { get; set; } = new object();

        public event Action Disposed;

        public void Dispose()
        {
            Disposed?.Invoke();
        }
    }
}