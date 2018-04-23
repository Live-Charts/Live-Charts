using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Assets.Models;
using LiveCharts.Core;
using LiveCharts.Core.Coordinates;

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
                            var shape = (Path) args.Shape;
                            shape.Stroke = Brushes.Blue;
                        })
                    .When(
                        player => player.Score <= 0, // when lt or eq to 0
                        (sender, args) =>
                        {
                            var shape = (Path) args.Shape;
                            shape.Stroke = Brushes.Red;
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
