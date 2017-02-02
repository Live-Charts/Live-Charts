using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
using UWP.CartesianChart.Energy_Predictions;
using UWP.CartesianChart.Financial;
using UWP.CartesianChart.FullyResponsive;
using UWP.CartesianChart.Funnel_Chart;
using UWP.CartesianChart.GanttChart;
using UWP.CartesianChart.HeatChart;
using UWP.CartesianChart.InLineSyntax;
using UWP.CartesianChart.Inverted_Series;
using UWP.CartesianChart.Irregular_Intervals;
using UWP.CartesianChart.Labels;
using UWP.CartesianChart.LineExample;
using UWP.CartesianChart.Linq;
using UWP.CartesianChart.LogarithmScale;
using UWP.CartesianChart.ManualZAndP;
using UWP.CartesianChart.MaterialCards;
using UWP.CartesianChart.Missing_Line_Points;
using UWP.CartesianChart.MixingSeries;
using UWP.CartesianChart.MultiAxes;
using UWP.CartesianChart.NegativeStackedRow;
using UWP.CartesianChart.PointShapeLine;
using UWP.CartesianChart.PointState;
using UWP.CartesianChart.ScatterPlot;
using UWP.CartesianChart.Sections;
using UWP.CartesianChart.SectionsDragable;
using UWP.CartesianChart.SharedTooltip;
using UWP.CartesianChart.SolidColorChart;
using UWP.CartesianChart.StackedArea;
using UWP.CartesianChart.StackedBar;
using UWP.CartesianChart.StepLine;
using UWP.CartesianChart.TypesSupport;
using UWP.CartesianChart.UIElements;
using UWP.CartesianChart.ZoomingAndPanning;
using UWP.CartesianChart.zzIssues;
using UWP.Gauges;
using UWP.PieChart;
using UWP.Views;

namespace UWP.Commons
{
    public class SampleDefinitions
    {
        public static SampleDefinition[] Definitions { get; } =
        {
            new SampleDefinition("Home", typeof(MainPage), SampleCategory.Information),
            new SampleDefinition("Material Cards", typeof(MaterialCards), SampleCategory.CartesianChart),
            new SampleDefinition("Energy Prediction", typeof(EnergyPredictionExample), SampleCategory.CartesianChart),
            new SampleDefinition("Solid Color", typeof(SolidColorExample), SampleCategory.CartesianChart),
            new SampleDefinition("Funnel Chart", typeof(FunnelExample), SampleCategory.CartesianChart),
            new SampleDefinition("Basic Column", typeof(BasicColumn), SampleCategory.CartesianChart),
            new SampleDefinition("Basic Row", typeof(BasicRowExample), SampleCategory.CartesianChart),
            new SampleDefinition("Gantt Chart", typeof(GanttExample), SampleCategory.CartesianChart),
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
            new SampleDefinition("Multi Axes", typeof(MultiAxesChart), SampleCategory.CartesianChart),
            new SampleDefinition("Negative Stacked Row", typeof(NegativeStackedRowExample), SampleCategory.CartesianChart),
            new SampleDefinition("Point Shape Line", typeof(PointShapeLineExample), SampleCategory.CartesianChart),
            new SampleDefinition("Point State", typeof(PointStateExample), SampleCategory.CartesianChart),
            new SampleDefinition("Scatter Plot", typeof(ScatterExample), SampleCategory.CartesianChart),
            new SampleDefinition("Sections", typeof(SectionsExample), SampleCategory.CartesianChart),
            new SampleDefinition("Dragable Sections", typeof(DragableSections), SampleCategory.CartesianChart),
            new SampleDefinition("Shared Tooltip", typeof(SharedTooltipExample), SampleCategory.CartesianChart),
            new SampleDefinition("Stacked Area", typeof(StackedAreaExample), SampleCategory.CartesianChart),
            new SampleDefinition("Vertical Stacked Area", typeof(VerticalStackedAreaExample), SampleCategory.CartesianChart),
            new SampleDefinition("Stacked Column", typeof(StackedColumnExample), SampleCategory.CartesianChart),
            new SampleDefinition("Stacked Row", typeof(StackedRowExample), SampleCategory.CartesianChart),
            new SampleDefinition("Step Line", typeof(StepLineExample), SampleCategory.CartesianChart),
            new SampleDefinition("Types Plotting", typeof(TypesPlotting), SampleCategory.CartesianChart),
            new SampleDefinition("UIElements", typeof(UIElementsExample), SampleCategory.CartesianChart),
            new SampleDefinition("Using DateTime", typeof(CartesianChart.Using_DateTime.DateTime), SampleCategory.CartesianChart),
            new SampleDefinition("Zooming And Panning", typeof(ZoomingAndPanning), SampleCategory.CartesianChart),
            new SampleDefinition("Issue #179", typeof(Issue179), SampleCategory.CartesianChart),

            new SampleDefinition("Angular Gauge", typeof(AngularGaugeExmple), SampleCategory.Gauges),
            new SampleDefinition("Gauge 360", typeof(Gauge360), SampleCategory.Gauges),

            new SampleDefinition("Doughnut Chart", typeof(DoughnutChartExample), SampleCategory.PieChart),
            new SampleDefinition("Pie Chart", typeof(PieChartExample), SampleCategory.PieChart),
        };
    }
}
