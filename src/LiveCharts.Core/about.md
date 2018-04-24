# About

This file contains the conventions followed in the code, please stick to these 
conventions to keep the code coherent.

## Core

It is a dot net core project, the main task of the core is to calculate every 
shape in the UI.

### LiveCharts.Core.Charts

Contains the models of the supported charts, the model contains the logic of 
how things are calcualted in the UI.

A chart model always has a chart view, the chart model calculates every shape 
based on the chart view properties.

#### LiveCharts.Core.Charts.IResource

A chart has resources, every time an update happens all the resources that 
were not used by the current update will be deleted from both, memory and the UI.

A resource is always a element that lives in LiveCharts.Core, a resource could
have a view in the UI.

### LiveCharts.Core.Collections

A set of specialized collections for the library.

### LiveCharts.Core.Coordinates

There are many different types of coordinates in the library, this namespace
contains them all, every coordinate has different requirements, for example
to generate a line series, we just need the PointCoordinate (X, Y based) 
mean while when we need a stacked bar series, the coordinate will be of type
stackedPointCoordinate, this coordinate stacks a value based on a given key.

The ICoordinate interface forces every coordinate to expose its content as a 
vector, this way we can access the X coordinate (for example in a cartesian chart)
using the *coordinate[0]* syntax, the Y coordinate would be *coordinate[1]*, 
this syntax is useful in many cases, specially when we need to invert charts.

Notice *coordinate[0]* returns an array (*float[]*, from now on planeArray) 
this means that for the X
plane we could have different values, for example in the case of a ranged point
in the X plane we would access the min value of the range as *coordinate[0][0]*
while the max value of the range for the X axis would be *coordinate[0][1]*,
we normally only use the first value (*coordinate[0][0]*) in the planeArray, but
we can store as much values as we need in the X plane.

### LiveCharts.Core.DataSeries

A series calculates every point shape, then requests the view to draw them.

### LiveCharts.Core.Defaults

The default types that the library already knows how to plot.

### LiveCharts.Core.Dimensions

A dimension indicates a direction where something can be measured,
in the case of a cartesian chart we have the X axis and the Y axis and some times
depending on the series we also have the Weight plane.

A chart in livecharts could have as many dimensions as it requires.

### LiveCharts.Core.Drawing

Contains shapes, brushes, animations, styles related material.

### LiveChart.Core.Interaction

Contains what normally a view consumes from the core of the library: view interfaces, 
view controls, points, and some events.

### LiveCharts.Core.Themes

The default themes in the library.

### LiveCharts.Core.Updating

Updates related material.

## View

A view project provides the Core of the library the logic of how shapes are drawn in 
the UI.

