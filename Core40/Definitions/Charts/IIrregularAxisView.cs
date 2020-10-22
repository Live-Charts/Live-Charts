using System;
using System.Collections.Generic;

namespace LiveCharts.Definitions.Charts
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="st">range start</param>
    /// <param name="ed">range end</param>
    /// <param name="Magnitude">small tick width</param>
    /// <param name="S">tick width</param>
    /// <returns></returns>
    public delegate IEnumerable<double> SeparatorProvider(double st, double ed, double Magnitude, double S);


    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="LiveCharts.Definitions.Charts.IAxisView" />
    public interface IIrregularAxisView : IAxisView
    {
        /// <summary>
        /// Gets or sets separator location provider.
        /// </summary>
        /// <value>
        /// The Separator location provider function.
        /// </value>
        SeparatorProvider SeparatorProvider { get; set; }
    }

}
