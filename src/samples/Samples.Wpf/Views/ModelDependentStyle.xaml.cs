using System.Windows.Controls;
using Assets.Models;
using LiveCharts.Core;
using LiveCharts.Core.Coordinates;
using LiveCharts.Wpf;
using LiveCharts.Wpf.Geared;

namespace Samples.Wpf.Views
{
    /// <summary>
    /// Interaction logic for ModelDependentStyle.xaml
    /// </summary>
    public partial class ModelDependentStyle : UserControl
    {
        public ModelDependentStyle()
        {
            InitializeComponent();

            // in this case we will build a chart using the Player class
            // we need to teach LiveCharts how to map a player to a chart coordinate.

            Charting.Configure(charting =>
            {
                // we will map player to a point in a chart
                // where the X coordinate will be the index of the player in our data collection
                // and the Y coordinate is the player.Score property

                // A mapper from player to a point coordinate
                var playerMapper = charting.LearnType<Player, PointCoordinate>(
                    (player, index) => new PointCoordinate(index, (float)player.Score));

                // the LearnType method returns a new instance of ModelToCoordinateMapper class
                // we will now use the When(trigger, handler) method to modify the visual element
                // of the point when a condition is true.

                playerMapper
                    .When(
                        player => player.Score > 0, // when the score is greater than 0
                        (sender, args) =>
                        {
                            // when score > 0 we are changing the visual stroke property to blue.

                            // in this case we are using the LiveCharts brushes API to use the same instance in both cases.
                            var blueBrush = LiveCharts.Core.Drawing.Brushes.Blue;

                            // notice the geared version uses a different shape!
                            if (charting.UiProvider.Name == "LiveCharts.Wpf.Geared")
                            {
                                var shape = (LiveCharts.Wpf.Geared.Drawing.Shapes.Path) args.Shape;
                                shape.Stroke = blueBrush.AsGearedBrush();
                            }
                            else
                            {
                                var shape = (System.Windows.Shapes.Path) args.Shape;
                                shape.Stroke = blueBrush.AsWpf();
                            }
                        })
                    .When(
                        player => player.Score <= 0, // when lt or eq to 0
                        (sender, args) =>
                        {
                            var redBrush = LiveCharts.Core.Drawing.Brushes.Red;

                            if (Charting.Settings.UiProvider.Name == "LiveCharts.Wpf.Geared")
                            {
                                var shape = (LiveCharts.Wpf.Geared.Drawing.Shapes.Path) args.Shape;
                                shape.Stroke = redBrush.AsGearedBrush();
                            }
                            else
                            {
                                var shape = (System.Windows.Shapes.Path) args.Shape;
                                shape.Stroke = redBrush.AsWpf();
                            }
                        });

                // notice in the case we cast arg.Shape to System.Windows.Shapes.Path
                // because the line series in the wpf package uses this shape
                // every series use a different shape, a way to know the used shape is to
                // browse the source code at LiveCharts.Wpf.Views there you will find the view
                // for every series.
            });
        }
    }
}
