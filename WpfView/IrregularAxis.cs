using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows;
using LiveCharts.Charts;
using LiveCharts.Definitions.Charts;

namespace LiveCharts.Wpf
{
    /// <summary>
    /// 
    /// </summary>
    public class IrregularAxis : Axis, IIrregularAxisView
    {
        /// <summary>
        /// get the core element
        /// </summary>
        /// <param name="chart">The chart.</param>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public override AxisCore AsCoreElement(ChartCore chart, AxisOrientation source)
        {
            if (Model == null) Model = new IrregularAxisCore(this);

            Model.ShowLabels = ShowLabels;
            Model.Chart = chart;
            Model.IsMerged = IsMerged;
            Model.Labels = Labels;
            Model.LabelFormatter = LabelFormatter;
            Model.MaxValue = MaxValue;
            Model.MinValue = MinValue;
            Model.Title = Title;
            Model.Position = Position;
            Model.Separator = Separator.AsCoreElement(Model, source);
            Model.DisableAnimations = DisableAnimations;
            Model.Sections = Sections.Select(x => x.AsCoreElement(Model, source)).ToList();

            return Model;
        }



        /// <summary>
        /// The separator location provider property
        /// </summary>
        public static readonly DependencyProperty SeparatorProviderProperty = DependencyProperty.Register(
                "SeparatorProvider",
                typeof(SeparatorProvider),
                typeof(IrregularAxis),
                new PropertyMetadata(default(SeparatorProvider), UpdateChart()));
        /// <summary>
        /// Gets or sets separator location provider.
        /// </summary>
        /// <value>
        /// The Separator location provider function.
        /// </value>
        public SeparatorProvider SeparatorProvider
        {
            get { return (SeparatorProvider)GetValue(SeparatorProviderProperty); }
            set { SetValue(SeparatorProviderProperty, value); }
        }
    }

}
