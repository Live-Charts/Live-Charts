//The MIT License(MIT)

//copyright(c) 2016 Alberto Rodriguez

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

namespace LiveCharts
{
    public class Charting
    {
        private static readonly Dictionary<Type, ConfigWrapper> Configurations;

        #region Contructors

        //Lets define some default configurations, all charts should be able to detect any of these types and plot
        //them correclty

        static Charting()
        {
            Configurations = new Dictionary<Type, ConfigWrapper>();

            // Live.Charts can plot any type, yes even any class defined by yourself.
            // you just need to tell the charts how to plot it.
            // for example the Xy Mapper expouses an X and Y method,
            // for example .X( "mapper" ) asks for the function to get the value of a point in X, same with Y
            // live charts will inject a value and an index
            // the value will be of type <T> and the index is only the value of the point in the array.

            //the SeriesMappers class is just a reminder of the configurators that we have, it actually just
            //returns a new instance of the related configurator
            //a configurator is only a holder for the mappers.

            //lets configure <int>

            For<int>(Mappers.Xy<int>()
                .X((value, index) => index) //use the index of the item in the array as X
                .Y(value => value), SeriesOrientation.Horizontal); //use the value (of type int int this case) as Y
            For<int>(Mappers.Xy<int>()
                .X(value => value) //use the value (int) as X
                .Y((value, index) => index), SeriesOrientation.Vertical); //use the index of the item in the array as Y

            //ok now lets configure a class I defined, the ObservablePoint class, it only has 2 properties, X and Y

            For<ObservablePoint>(Mappers.Xy<ObservablePoint>() //in this case value is of type <ObservablePoint>
                .X(value => value.X) //use the X property as X
                .Y(value => value.Y)); //use the Y property as Y

            //easy, now live charts know how to plot an ObservablePoint class and integers.

            //ok, now lets use another mapper, in this case we are going to configre a class for a PolarChart
            //polar chart requieres a Radius and an Angle instead of X and Y
            //so we will just pull the Mappers.Polar configurator
            //and specify which property to use as Radius and which one as Angle
            //in this case the PolarPoint class that I defined only contains these 2 properties.
            //so it is realy easy, now value is of type Polar point, becaus we said so when: SeriesMappers.Polar<PolarPoint>()

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

            For<BubblePoint>(Mappers.Bubble<BubblePoint>()
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

        }

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

        public object GetConfig<T>(SeriesOrientation orientation = SeriesOrientation.Horizontal)
        {
            var wrapper = Configurations[typeof (T)];

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
