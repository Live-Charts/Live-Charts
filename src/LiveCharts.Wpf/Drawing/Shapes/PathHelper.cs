using System.Windows.Media;
using System.Windows.Shapes;

namespace LiveCharts.Wpf.Drawing
{
    public class PathHelper
    {
        public PathHelper()
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
    }
}
