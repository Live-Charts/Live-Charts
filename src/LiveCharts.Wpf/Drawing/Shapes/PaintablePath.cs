using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Shapes;
using LiveCharts.Animations;
using LiveCharts.Drawing.Brushes;
using LiveCharts.Drawing.Shapes;

namespace LiveCharts.Wpf.Drawing
{
    public class PaintablePath : IPaintable
    {
        public PaintablePath()
        {
            Figure = new PathFigure
            {
                Segments = new PathSegmentCollection()
            };

            Path = new Path
            {
                Data = new PathGeometry
                {
                    Figures = new PathFigureCollection(1)
                    {
                        Figure
                    }
                }
            };
        }

        public Path Path { get; }

        public PathFigure Figure { get; }

        IAnimationBuilder IAnimatable.Animate(Transition timeline)
        {
            throw new System.NotImplementedException();
        }

        IEnumerable<IPaintable> IUIContent.GetPaintables()
        {
            yield return this;
        }

        void IPaintable.Paint(IBrush stroke, IBrush fill)
        {
            throw new System.NotImplementedException();
        }
    }
}
