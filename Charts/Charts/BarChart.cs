using System;
using System.Linq;
using System.Windows;
using Charts.Series;

namespace Charts.Charts
{
    public class BarChart : Chart
    {
        public BarChart()
        {
            PrimaryAxis = new Axis();
            SecondaryAxis = new Axis();
            //SecondaryAxis = new Axis
            //{
            //    Separator = { Enabled = false },
            //    Enabled = false
            //};
            Hoverable = true;
            ShapeHoverBehavior = ShapeHoverBehavior.Shape;
        }

        protected override bool ScaleChanged => GetMax() != Max ||
                                                GetMin() != Min;

        private Point GetMax()
        {
            var p = new Point(
                Series.Cast<BarSerie>().Select(x => x.PrimaryValues.Count).DefaultIfEmpty(0).Max(),
                Series.Cast<BarSerie>().Select(x => x.PrimaryValues.Max()).DefaultIfEmpty(0).Max());
            p.Y = PrimaryAxis.MaxValue ?? p.Y;
            return p;
        }

        private Point GetMin()
        {
            var p = new Point(.01, Series.Cast<BarSerie>().Select(x => x.PrimaryValues.Min()).DefaultIfEmpty(0).Min());
            p.Y = PrimaryAxis.MinValue ?? p.Y;
            return p;
        }

        private Point GetS()
        {
            return new Point(
                CalculateSeparator(Max.X - Min.X, AxisTags.X),
                CalculateSeparator(Max.Y - Min.Y, AxisTags.Y));
        }

        protected override void Scale()
        {
            Max = GetMax();
            Min = GetMin();
            S = GetS();
            S.X = 1; // we force 1

            Max.Y = PrimaryAxis.MaxValue ?? (Math.Truncate(Max.Y / S.Y) + 1) * S.Y;
            Min.Y = PrimaryAxis.MinValue ?? (Math.Truncate(Min.Y / S.Y) - 1) * S.Y;

            var unitW = ToPlotArea(1, AxisTags.X) - PlotArea.X + 5;
            LabelOffset = unitW/2;

            DrawAxis();
        }
    }
}
