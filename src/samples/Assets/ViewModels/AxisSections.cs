using System.Collections.Generic;
using System.Drawing;
using LiveCharts.Core.DataSeries;
using LiveCharts.Core.Dimensions;
using LiveCharts.Core.Drawing;
using LiveCharts.Core.Interaction.Styles;

namespace Assets.ViewModels
{
    public class ModelDependentStyle
    {

    }

    public class AxisSections
    {
        public AxisSections()
        {
            SeriesCollection = new List<ISeries>
            {
                new LineSeries<double>
                {
                    Values = new List<double>
                    {
                        4,
                        7,
                        2,
                        9,
                        5,
                        1,
                        2,
                        9
                    }
                }
            };

            // create a new section
            var xSection = new Section();
            // in this case we will set the value of the section to 3
            // this means it be drawn at 3
            xSection.Value = 3;
            // we want the section to go from 3 to 5, so we need to set
            // the length of the section to 2
            xSection.Length = 2;
            // finally set the style of the section
            xSection.LabelContent = "Nice Zone";
            xSection.Fill = new SolidColorBrush(Color.FromArgb(60, 244, 67, 54));
            xSection.Stroke = new SolidColorBrush(Color.FromArgb(244, 67, 54));
            xSection.StrokeThickness = 3;
            xSection.StrokeDashArray = new float[] {3, 3};
            xSection.LabelHorizontalAlignment = HorizontalAlignment.Right;
            xSection.LabelVerticalAlignment = VerticalAlignment.Bottom;

            // now we build the x Axis

            var sectionsCollection = new List<Section>();
            sectionsCollection.Add(xSection);

            var xAxis = new Axis();
            xAxis.Sections = sectionsCollection;

            var xAxisCollection = new List<Plane>();
            xAxisCollection.Add(xAxis);

            X = xAxisCollection;

            // now we will do the same for the Y axis
            // just using a shorter syntax.

            Y = new List<Plane>
            {
                new Axis
                {
                    Sections = new List<Section>
                    {
                        new Section
                        {
                            LabelContent = "Nice Zone",
                            Value = 3,
                            Length = 2,
                            Fill = new SolidColorBrush(Color.FromArgb(60, 244, 67, 54)),
                            Stroke = new SolidColorBrush(Color.FromArgb(244, 67, 54)),
                            StrokeThickness = 3,
                            StrokeDashArray = new float[] {3, 3},
                            LabelHorizontalAlignment = HorizontalAlignment.Right,
                            LabelVerticalAlignment = VerticalAlignment.Bottom
                        }
                    }
                }
            };
        }

        public IEnumerable<ISeries> SeriesCollection { get; set; }
        public IList<Plane> X { get; set; }
        public IList<Plane> Y { get; set; }
    }
}
