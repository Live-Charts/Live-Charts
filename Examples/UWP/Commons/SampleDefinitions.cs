using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWP.CartesianChart.Basic_Bars;
using UWP.CartesianChart.Basic_Stacked_Bar;
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
            new SampleDefinition("Basic Stacked Row Percentage Example", typeof(BasicStackedRowPercentageExample), SampleCategory.CartesianChart),
        };
    }
}
