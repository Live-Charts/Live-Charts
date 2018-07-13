# Teaching the library to plot a custom type

This is probably the most confusing part of the library, it is not necessary to use the library because you can only call `.LearnPrimitiveAndDefaultTypes()` (explained at [settings section](),docs/getting started/settings} and use the types that this documentation and samples set use, but might help you to achieve better code quality and performance.

LiveCharts has no idea how to plot anything, you have to tell the library how to map an object to a point in a chart, as explained at `docs/getting started/settings` when we called `LearnPrimitiveAndDefaultTypes()` we taught the library how to plot all the primitive and the default types defined in the library.

Imagine your application has a collection of the `Student` class:

```c#
public class Student
{
    public string Name { get; set; }
    public double Age { get; set; }
}
```

We want to create a bar chart that compares the age of our students, then how do I teach the library to map the `Student` class to a point in our chart?

We will use the `LearnType()` method to do the mapping:

```c#
// approach 1
charting.LearnType&lt;Student&gt;(MapStudent);
// where MapStudent is a method in your application as follows:
public LiveCharts.Core.Coordinates.Point MapStudent(Student student, int index)
{
    return new LiveCharts.Core.Coordinates.Point(index, (float) student.Age);
}

// approach 2
// The code above could be replaced with a lambda expression as follows:
charting.LearnType&lt;Student&gt;(
                    (student, index) => new LiveCharts.Core.Coordinates.Point(index, (float) student.Age));
```

See 'approach 1' in the block above, when we call `LearnType&lt;T&gt;()` we are letting the library store a mapper globally for `T` in this case the `Student` class, the method `LearnType&lt;T&gt;(*mapper*)` asks for the *mapper* parameter, in 'approach 1' we passed the MapStudent function, which receives a student and the index of the student in the series, and returns a `Point` where the X coordinate is the index of the student in the series, and the Y coordinate will be the `Age` property of the student (notice the `Point` constructor requires [*x*,*y*] coordinates).

Approach 2 is a short way to achieve the same using lambda expressions, it depends on your prefered method.

And that's all, now the library by default every time you are trying to draw a collection of the `Student` class, it will use the mapper we defined.

## Multiple mappers for the same type

In the sample above we stored globally a mapper, so every time the `Student` class requests a plot, we will map the index of the student as our *x* coordinate and the `Age` property as or *y*, but imagine we also require to create a plot for the `Score` property:

```c#
public class Student
{
    public string Name { get; set; }
    public double Age { get; set; }
    public double Score { get; set; }
}
```

As we explained by default a student is represented in a chart by the following point (*index*, *age*), to override this setting we need to set the `Series.Mapper` property:

```c#
var studentsByScoreSeries = new LineSeries&lt;Student&gt;();
studentsByScoreSeries.Mapper = new ModelToCoordinateMapper<Student, Point>(
                (student, index) => new Point(index, (float) student.Score));
```

Now for the `studentsByScoreSeries` instance a student will be represented with a point which the X coordinate will be the index of the student in the series, and the Y coordinate will be the `Score` property.

### Do all the charts in this framework use the Coordinates.Point struct?

No, the previous samples will work only for series that require a `LiveCharts.Core.Coordinates.Point` struct, but in a case of a pie series where we obviously have no *X* and *Y* points, or a financial series when we require our points to have *Open*, *High*, *Low* and *Close* properties then we have to set a mapper for the corresponding *Coordinate*.

**ToDo:** Add a table with the series and the *Coordinate* they use.