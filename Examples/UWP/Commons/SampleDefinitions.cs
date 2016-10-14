using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWP.CartesianChart.Basic_Bars;
using UWP.CartesianChart.Basic_Stacked_Bar;
using UWP.CartesianChart.BasicLine;
using UWP.CartesianChart.Bubbles;
using UWP.CartesianChart.ConstantChanges;
using UWP.CartesianChart.Customized_Series;
using UWP.CartesianChart.CustomTooltipAndLegend;
using UWP.CartesianChart.DynamicVisibility;
using UWP.CartesianChart.Financial;
using UWP.CartesianChart.FullyResponsive;
using UWP.CartesianChart.HeatChart;
using UWP.CartesianChart.InLineSyntax;
using UWP.CartesianChart.Inverted_Series;
using UWP.CartesianChart.Irregular_Intervals;
using UWP.CartesianChart.Labels;
using UWP.CartesianChart.LineExample;
using UWP.CartesianChart.Linq;
using UWP.CartesianChart.LogarithmScale;
using UWP.CartesianChart.ManualZAndP;
using UWP.CartesianChart.Missing_Line_Points;
using UWP.CartesianChart.MixingSeries;
using UWP.Views;

namespace UWP.Commons
{
    public class SampleDefinitions
    {
        public static SampleDefinition[] Definitions { get; } =
        {
            new SampleDefinition("Home", typeof(MainPage), SampleCategory.Information),
            new SampleDefinition("Basic Column", typeof(BasicColumn), SampleCategory.CartesianChart),
            new SampleDefinition("Basic Row", typeof(BasicRowExample), SampleCategory.CartesianChart),
            new SampleDefinition("Basic Stacked Column", typeof(BasicStackedColumnExample), SampleCategory.CartesianChart),
            new SampleDefinition("Basic Stacked Row Percentage", typeof(BasicStackedRowPercentageExample), SampleCategory.CartesianChart),
            new SampleDefinition("Basic Line", typeof(BasicLineExample), SampleCategory.CartesianChart),
            new SampleDefinition("Bubbles", typeof(BubblesExample), SampleCategory.CartesianChart),
            new SampleDefinition("Constant Changes", typeof(ConstantChangesChart), SampleCategory.CartesianChart),
            new SampleDefinition("Customized Series", typeof(CustomizedLineSeries), SampleCategory.CartesianChart),
            new SampleDefinition("Custom Tooltip and Legend", typeof(CustomTooltipAndLegendExample), SampleCategory.CartesianChart),
            new SampleDefinition("Dynamic Visibility", typeof(DynamicVisibilityExample), SampleCategory.CartesianChart),
            new SampleDefinition("Financial Candle Stick", typeof(CandleStickExample), SampleCategory.CartesianChart),
            new SampleDefinition("Financial Ohcl", typeof(OhclExample), SampleCategory.CartesianChart),
            new SampleDefinition("Fully Responsive", typeof(FullyResponsiveExample), SampleCategory.CartesianChart),
            new SampleDefinition("Heat Chart", typeof(HeatSeriesExample), SampleCategory.CartesianChart),
            new SampleDefinition("InLine Syntax", typeof(InLineSyntaxTest), SampleCategory.CartesianChart),
            new SampleDefinition("Inverted Series", typeof(InvertedExample), SampleCategory.CartesianChart),
            new SampleDefinition("Irregular Intervals", typeof(IrregularIntervalsExample), SampleCategory.CartesianChart),
            new SampleDefinition("Labels", typeof(LabelsExample), SampleCategory.CartesianChart),
            new SampleDefinition("Labels Horizontal", typeof(LabelsHorizontalExample), SampleCategory.CartesianChart),
            new SampleDefinition("Line Example", typeof(LineExample), SampleCategory.CartesianChart),
            new SampleDefinition("Linq", typeof(LinqExample), SampleCategory.CartesianChart),
            new SampleDefinition("Logarithm Scale", typeof(LogarithmScaleExample), SampleCategory.CartesianChart),
            new SampleDefinition("Manual Z And P", typeof(ManualZAndPExample), SampleCategory.CartesianChart),
            new SampleDefinition("Missing Line Points", typeof(MissingPointsExample), SampleCategory.CartesianChart),
            new SampleDefinition("Mixing Series", typeof(MixingSeries), SampleCategory.CartesianChart),
        };
    }
}
