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

namespace LiveCharts.Defaults
{
    public class DefaultConfigurations
    {
        private static readonly Dictionary<Type, SeriesConfigurationWrapper> Configurations;

        #region Contructors

        //Lets define some default configurations, all charts should be able to detect any of this types and plot
        //them correclty

        static DefaultConfigurations()
        {
            Configurations = new Dictionary<Type, SeriesConfigurationWrapper>();

            Configurations[typeof (object)] = new SeriesConfigurationWrapper
            {
                IndexedX = new SeriesConfiguration<object>().X((v, i) => i).Y(v => (double) v),
                IndexedY = new SeriesConfiguration<object>().X(v => (double) v).Y((v, i) => i)
            };

            Configurations[typeof (int)] = new SeriesConfigurationWrapper
            {
                IndexedX = new SeriesConfiguration<int>().X((v, i) => i).Y(v => v),
                IndexedY = new SeriesConfiguration<int>().X(v => v).Y((v, i) => i)
            };

            Configurations[typeof(double)] = new SeriesConfigurationWrapper
            {
                IndexedX = new SeriesConfiguration<double>().X((v, i) => i).Y(v => v),
                IndexedY = new SeriesConfiguration<double>().X(v => v).Y((v, i) => i)
            };

            Configurations[typeof(decimal)] = new SeriesConfigurationWrapper
            {
                IndexedX = new SeriesConfiguration<decimal>().X((v, i) => i).Y(v => (double) v),
                IndexedY = new SeriesConfiguration<decimal>().X(v => (double) v).Y((v, i) => i)
            };

            Configurations[typeof(short)] = new SeriesConfigurationWrapper
            {
                IndexedX = new SeriesConfiguration<short>().X((v, i) => i).Y(v => v),
                IndexedY = new SeriesConfiguration<short>().X(v => v).Y((v, i) => i)
            };

            Configurations[typeof(float)] = new SeriesConfigurationWrapper
            {
                IndexedX = new SeriesConfiguration<float>().X((v, i) => i).Y(v => v),
                IndexedY = new SeriesConfiguration<float>().X(v => v).Y((v, i) => i)
            };

            Configurations[typeof(long)] = new SeriesConfigurationWrapper
            {
                IndexedX = new SeriesConfiguration<long>().X((v, i) => i).Y(v => v),
                IndexedY = new SeriesConfiguration<long>().X(v => v).Y((v, i) => i)
            };

            Configurations[typeof (BubblePoint)] = new SeriesConfigurationWrapper
            {
                IndexedX = new SeriesConfiguration<BubblePoint>()
                    .X(bubble => bubble.X)
                    .Y(bubble => bubble.Y)
                    .Weight(bubble => bubble.Weight)
            };

            Configurations[typeof (ObservablePoint)] = new SeriesConfigurationWrapper
            {
                IndexedX = new SeriesConfiguration<ObservablePoint>().X(p => p.X).Y(p => p.Y)
            };
        }

        #endregion

        public ISeriesConfiguration this[Type type, 
            SeriesConfigurationType configurationType = SeriesConfigurationType.IndexedX]
        {
            get
            {
                SeriesConfigurationWrapper wrapper;

                if (!Configurations.TryGetValue(type, out wrapper)) wrapper = new SeriesConfigurationWrapper();

                return wrapper.GetConfig(configurationType);
            }
        }
    }


    internal class SeriesConfigurationWrapper
    {
        internal ISeriesConfiguration IndexedX { get; set; }
        internal ISeriesConfiguration IndexedY { get; set; }

        internal ISeriesConfiguration GetConfig(SeriesConfigurationType type)
        {
            return type == SeriesConfigurationType.IndexedX
                ? IndexedX
                : IndexedY;
        }
    }
}
