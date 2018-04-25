using System;
using System.Collections;

namespace LiveCharts.Core.Charts
{
    internal class EnumerableResource
    {
        /// <summary>
        /// Gets or sets the collection.
        /// </summary>
        /// <value>
        /// The collection.
        /// </value>
        public IEnumerable Collection { get; set; }

        /// <summary>
        /// Gets or sets the update identifier.
        /// </summary>
        /// <value>
        /// The update identifier.
        /// </value>
        public object UpdateId { get; set; }

        public event Action Disposed;

        public void Dispose()
        {
            Disposed?.Invoke();
            Collection = null;
            UpdateId = null;
        }
    }
}