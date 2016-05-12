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

namespace LiveCharts.Defaults
{
    public class SeriesConfigurations
    {
        private static readonly Dictionary<Type, ConfigWrapper> Configurations;

        #region Contructors

        //Lets define some default configurations, all charts should be able to detect any of this types and plot
        //them correclty

        static SeriesConfigurations()
        {
            Configurations = new Dictionary<Type, ConfigWrapper>();

            For<int>(SeriesMappers.Xy<int>().X((v, i) => i).Y(v => v), SeriesOrientation.Horizontal);
            For<int>(SeriesMappers.Xy<int>().X(v => v).Y((v, i) => i), SeriesOrientation.Vertical);

            For<double>(SeriesMappers.Xy<double>().X((v, i) => i).Y(v => v), SeriesOrientation.Horizontal);
            For<double>(SeriesMappers.Xy<double>().X(v => v).Y((v, i) => i), SeriesOrientation.Vertical);

            For<decimal>(SeriesMappers.Xy<decimal>().X((v, i) => i).Y(v => (double) v), SeriesOrientation.Horizontal);
            For<decimal>(SeriesMappers.Xy<decimal>().X(v => (double) v).Y((v, i) => i), SeriesOrientation.Vertical);

            For<short>(SeriesMappers.Xy<short>().X((v, i) => i).Y(v => v), SeriesOrientation.Horizontal);
            For<short>(SeriesMappers.Xy<short>().X(v => v).Y((v, i) => i), SeriesOrientation.Vertical);

            For<float>(SeriesMappers.Xy<float>().X((v, i) => i).Y(v => v), SeriesOrientation.Horizontal);
            For<float>(SeriesMappers.Xy<float>().X(v => v).Y((v, i) => i), SeriesOrientation.Vertical);

            For<long>(SeriesMappers.Xy<long>().X((v, i) => i).Y(v => v), SeriesOrientation.Horizontal);
            For<long>(SeriesMappers.Xy<long>().X(v => v).Y((v, i) => i), SeriesOrientation.Vertical);

            For<BubblePoint>(SeriesMappers.Bubble<BubblePoint>()
                .X(v => v.X)
                .Y(v => v.Y)
                .Weight(v => v.Weight), SeriesOrientation.Horizontal);
            For<BubblePoint>(SeriesMappers.Bubble<BubblePoint>()
                .X(v => v.X)
                .Y(v => v.Y)
                .Weight(v => v.Weight), SeriesOrientation.Vertical);

            For<ObservablePoint>(SeriesMappers.Xy<ObservablePoint>().X(v => v.X).Y(v => v.Y),
                SeriesOrientation.Horizontal);
            For<ObservablePoint>(SeriesMappers.Xy<ObservablePoint>().X(v => v.X).Y(v => v.Y), 
                SeriesOrientation.Vertical);

            For<ObservableValue>(SeriesMappers.Xy<ObservableValue>().X((v,i) => i).Y(v => v.Value), 
                SeriesOrientation.Horizontal);
            For<ObservableValue>(SeriesMappers.Xy<ObservableValue>().X(v => v.Value).Y((v, i) => i),
                SeriesOrientation.Vertical);

            For<DatePoint>(SeriesMappers.Xy<DatePoint>().X(x => x.DateTime.Ticks).Y(x => x.Value),
                SeriesOrientation.Horizontal);
            For<DatePoint>(SeriesMappers.Xy<DatePoint>().X(x => x.Value).Y(x => x.DateTime.Ticks),
                SeriesOrientation.Vertical);

            For<OhlcPoint>(SeriesMappers.Financial<OhlcPoint>()
                .X(v => v.DateTime)
                .Open(v => v.Open)
                .High(v => v.High)
                .Low(v => v.Low)
                .Close(v => v.Close), SeriesOrientation.Horizontal);
        }

        static void For<T>(object config, SeriesOrientation orientation)
        {
            ConfigWrapper wrapper;
            var t = typeof (T);

            if (!Configurations.TryGetValue(t, out wrapper))
            {
                wrapper = new ConfigWrapper();
                Configurations[t] = wrapper;
            }

            if (orientation == SeriesOrientation.Horizontal) wrapper.HorizontalConfig = config;
            else wrapper.VerticalConfig = config;
        }

        #endregion

        public object GetConfig<T>(SeriesOrientation orientation = SeriesOrientation.Horizontal)
        {
            var wrapper = Configurations[typeof (T)];

            return orientation == SeriesOrientation.Horizontal
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
