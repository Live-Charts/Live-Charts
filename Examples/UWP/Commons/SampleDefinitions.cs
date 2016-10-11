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
        };
    }
}
