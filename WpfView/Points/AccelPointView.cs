
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using LiveCharts.Charts;
using LiveCharts.Definitions.Points;
using LiveCharts.Dtos;

namespace LiveCharts.Wpf.Points
{
    enum ViewShrinkState
    {
        Individual,     // must draw as view it's own
        Shrinker,       // must draw as shrinker 
        Shrinked,       // must not to draw
    }


    internal class AccelPointView : IChartPointView
    {
        public bool IsNew { get; set; }
        public CoreRectangle ValidArea { get; internal set; }


        public virtual void DrawOrMove(ChartPoint previousDrawn, ChartPoint current, int index, ChartCore chart)
        {
        }

        public virtual void RemoveFromView(ChartCore chart)
        {
        }

        public virtual void OnHover(ChartPoint point)
        {
        }

        public virtual void OnHoverLeave(ChartPoint point)
        {
        }
    }
}
