using System;
using System.Drawing;
using LiveCharts.Core.Collections;
using LiveCharts.Core.DataSeries;
using LiveCharts.Core.Drawing.Svg;

namespace Assets.ViewModels
{
    public class BasicLineChart
    {
        public BasicLineChart()
        {
            SeriesCollection = new ChartingCollection<Series>();

            // create a new line series
            var lineSeries = new LineSeries<double>();
            lineSeries.Add(4);
            lineSeries.Add(7);
            lineSeries.Add(8);
            lineSeries.Add(9);

            // now lets customize it

            // set a geometry and its size
            lineSeries.GeometrySize = 30;
            lineSeries.Geometry = Geometry.Diamond;

            // a custom fill and stroke, if we don't these properties
            // LiveCharts will set them for us according to our theme.
            lineSeries.Stroke = Color.Purple;
            lineSeries.Fill = Color.FromArgb(10, 80, 00, 80);

            // the beziers in the drawn shape are calculated based on
            // the line smoothness property, the value goes from 
            // 0 (straight lines) to 1 (super curved lines)
            // use any value between 0 and 1 to play with the smoothness.
            lineSeries.LineSmoothness = 0;

            // do not display a label for every point
            lineSeries.DataLabels = false;

            //lineSeries.

            SeriesCollection.Add(lineSeries);
        }

        public ChartingCollection<Series> SeriesCollection { get; set; }

        public string[] Labels { get; set; }

        public Func<double, string> YFormatter { get; set; }
    }
}
