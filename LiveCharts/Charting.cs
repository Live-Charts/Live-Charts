//The MIT License(MIT)

//Copyright(c) 2016 Alberto Rodriguez & LiveCharts Contributors

//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.

using System;
using System.Collections.Generic;
using LiveCharts.Configurations;
using LiveCharts.Defaults;
using LiveCharts.Helpers;

namespace LiveCharts
{
    /// <summary>
    /// Global LiveCharts configuration
    /// </summary>
    public class Charting
    {
        private static readonly Dictionary<Type, ConfigWrapper> Configurations;

        #region Constructors

        //Lets define some default configurations, all charts should be able to detect any of these types and plot
        //them correctly

        static Charting()
        {
            Configurations = new Dictionary<Type, ConfigWrapper>();

            // Live.Charts can plot any type, yes even any class defined by yourself.
            // you just need to tell the charts how to plot it.
            // for example the XY Mapper exposes an X and Y method,
            // for example .X( "mapper" ) asks for the function to get the value of a point in X, same with Y
            // live charts will inject a value and an index
            // the value will be of type <T> and the index is only the value of the point in the array.

            //the SeriesMappers class is just a reminder of the configurations that we have, it actually just
            //returns a new instance of the related configuration
            //a configuration is only a holder for the mappers.

            //lets configure <int>

            For<int>(Mappers.Xy<int>()
                .X((value, index) => index) //use the index of the item in the array as X
                .Y(value => value), SeriesOrientation.Horizontal); //use the value (of type int integer this case) as Y
            For<int>(Mappers.Xy<int>()
                .X(value => value) //use the value (int) as X
                .Y((value, index) => index), SeriesOrientation.Vertical); //use the index of the item in the array as Y

            //OK now lets configure a class I defined, the ObservablePoint class, it only has 2 properties, X and Y

            For<ObservablePoint>(Mappers.Xy<ObservablePoint>() //in this case value is of type <ObservablePoint>
                .X(value => value.X) //use the X property as X
                .Y(value => value.Y)); //use the Y property as Y

            //easy, now live charts know how to plot an ObservablePoint class and integers.

            //OK, now lets use another mapper, in this case we are going to configure a class for a PolarChart
            //polar chart requires a Radius and an Angle instead of X and Y
            //so we will just pull the Mappers.Polar configuration
            //and specify which property to use as Radius and which one as Angle
            //in this case the PolarPoint class that I defined only contains these 2 properties.
            //so it is really easy, now value is of type Polar point, because we said so when: SeriesMappers.Polar<PolarPoint>()

            For<PolarPoint>(Mappers.Polar<PolarPoint>()
                .Radius(value => value.Radius) //use the radius property as radius for the plotting
                .Angle(value => value.Angle)); //use the angle property as angle for the plotting

            //now a more complex situation
            //the OhclPoint is ready to plot financial points,
            //the financial series requires a Open, High, Low, and Close values at least
            //that is just by definition of a CandleStick series
            //so I created a OhclPoint class, and used the Financial mapper:
            //and again the OhclPoint class only contains those 4 properties
            //we just mapped them correctly and LiveCharts will know how to handle this class.

            //the DateTime for now its tricky....
            //I will explain better later if i do not find a better solution...

            For<OhlcPoint>(Mappers.Financial<OhlcPoint>()
                .X((value, index) => index)
                .Open(value => value.Open)
                .High(value => value.High)
                .Low(value => value.Low)
                .Close(value => value.Close), SeriesOrientation.Horizontal);

            For<double>(Mappers.Xy<double>()
                .X((value, index) => index)
                .Y(value => value), SeriesOrientation.Horizontal);
            For<double>(Mappers.Xy<double>()
                .X(value => value)
                .Y((value, index) => index), SeriesOrientation.Vertical);

            For<decimal>(Mappers.Xy<decimal>()
                .X((value, index) => index)
                .Y(value => (double) value), SeriesOrientation.Horizontal);
            For<decimal>(Mappers.Xy<decimal>()
                .X(value => (double) value)
                .Y((value, index) => index), SeriesOrientation.Vertical);

            For<short>(Mappers.Xy<short>()
                .X((value, index) => index)
                .Y(value => value), SeriesOrientation.Horizontal);
            For<short>(Mappers.Xy<short>()
                .X(value => value)
                .Y((value, index) => index), SeriesOrientation.Vertical);

            For<float>(Mappers.Xy<float>()
                .X((value, index) => index)
                .Y(value => value), SeriesOrientation.Horizontal);
            For<float>(Mappers.Xy<float>()
                .X(value => value)
                .Y((value, index) => index), SeriesOrientation.Vertical);

            For<long>(Mappers.Xy<long>()
                .X((value, index) => index)
                .Y(value => value), SeriesOrientation.Horizontal);
            For<long>(Mappers.Xy<long>()
                .X(value => value)
                .Y((value, index) => index), SeriesOrientation.Vertical);

            For<ScatterPoint>(Mappers.Weighted<ScatterPoint>()
                .X(value => value.X)
                .Y(value => value.Y)
                .Weight(value => value.Weight));

            For<HeatPoint>(Mappers.Weighted<HeatPoint>()
                .X(value => value.X)
                .Y(value => value.Y)
                .Weight(value => value.Weight));

            For<ObservableValue>(Mappers.Xy<ObservableValue>()
                .X((value, index) => index)
                .Y(value => value.Value),
                SeriesOrientation.Horizontal);
            For<ObservableValue>(Mappers.Xy<ObservableValue>()
                .X(value => value.Value)
                .Y((value, index) => index),
                SeriesOrientation.Vertical);

            For<DateTimePoint>(Mappers.Xy<DateTimePoint>()
                .X(value => value.DateTime.Ticks)
                .Y(value => value.Value), SeriesOrientation.Horizontal);
            For<DateTimePoint>(Mappers.Xy<DateTimePoint>()
                .X(value => value.Value)
                .Y(value => value.DateTime.Ticks), SeriesOrientation.Vertical);

            For<GanttPoint>(Mappers.Gantt<GanttPoint>()
                .X((v, i) => -i)
                .YStart(v => v.StartPoint)
                .Y(v => v.EndPoint), SeriesOrientation.Horizontal);
            For<GanttPoint>(Mappers.Gantt<GanttPoint>()
                .Y((v, i) => -i)
                .XStart(v => v.StartPoint)
                .X(v => v.EndPoint), SeriesOrientation.Vertical);

        }

        /// <summary>
        /// Saves a type mapper globally.
        /// </summary>
        /// <typeparam name="T">Type to configure</typeparam>
        /// <param name="config">mapper</param>
        /// <param name="orientation">mapper orientation</param>
        public static void For<T>(object config, SeriesOrientation orientation = SeriesOrientation.All)
        {
            ConfigWrapper wrapper;
            var t = typeof (T);

            if (!Configurations.TryGetValue(t, out wrapper))
            {
                wrapper = new ConfigWrapper();
                Configurations[t] = wrapper;
            }

            if (orientation == SeriesOrientation.All)
            {
                wrapper.VerticalConfig = config;
                wrapper.HorizontalConfig = config;

                return;
            }

            if (orientation == SeriesOrientation.Horizontal) wrapper.HorizontalConfig = config;
            else wrapper.VerticalConfig = config;
        }

        #endregion

        /// <summary>
        /// Gets the configuration of a given type and orientation
        /// </summary>
        /// <typeparam name="T">type to look for</typeparam>
        /// <param name="orientation">orientation to look for</param>
        /// <returns></returns>
        public object GetConfig<T>(SeriesOrientation orientation = SeriesOrientation.Horizontal)
        {
            ConfigWrapper wrapper;

            if (!Configurations.TryGetValue(typeof(T), out wrapper))
                throw new LiveChartsException("LiveCharts does not know how to plot " + typeof(T).Name + ", " +
                                              "you can either, use an already configured type " +
                                              "or configure this type you are trying to use, " +
                                              "For more info see " +
                                              "http://lvcharts.net/App/examples/v1/wpf/Types%20and%20Configuration");

            return orientation == SeriesOrientation.Horizontal || orientation == SeriesOrientation.All
                ? wrapper.HorizontalConfig
                : wrapper.VerticalConfig;
        }
    }

    internal class ConfigWrapper
    {
        public object HorizontalConfig { get; set; }
        public object VerticalConfig { get; set; }
    }
}
