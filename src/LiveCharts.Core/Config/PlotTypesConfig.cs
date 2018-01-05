using LiveCharts.Core.Data.Builders;
using LiveCharts.Core.Defaults;

namespace LiveCharts.Core.Config
{
    /// <summary>
    /// Primitive types configuration.
    /// </summary>
    public static class PlotTypesConfig
    {
        /// <summary>
        /// Returns a builder to teach the library how to plot any given type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="config">The configuration.</param>
        /// <returns></returns>
        public static ChartingTypeBuilder<T> PlotType<T>(this LiveChartsConfig config)
        {
            return config.ConfigureType<T>();
        }

        /// <summary>
        /// Removes all configured plot types.
        /// </summary>
        /// <param name="config">The configuration.</param>
        /// <returns></returns>
        public static LiveChartsConfig RemoveAllConfiguredPlotTypes(this LiveChartsConfig config)
        {
            LiveChartsConfig.Builders.Clear();
            return config;
        }

        /// <summary>
        /// Configures LiveCharts to plot C# primitive types.
        /// </summary>
        /// <param name="config">The configuration.</param>
        /// <returns></returns>
        public static LiveChartsConfig AddPrimitivesPlotTypes(this LiveChartsConfig config)
        {
            config.ConfigureType<short>()
                .AsCartesian(options =>
                {
                    options.XGetter = (value, index) => index;
                    options.YGetter = (value, index) => value;
                })
                .AsPie(builder =>
                {
                    builder.ValueGetter = (value, index) => value;
                });

            config.ConfigureType<ushort>()
                .AsCartesian(options =>
                {
                    options.XGetter = (value, index) => index;
                    options.YGetter = (value, index) => value;
                })
                .AsPie(builder =>
                {
                    builder.ValueGetter = (value, index) => value;
                });

            config.ConfigureType<int>()
                .AsCartesian(options =>
                {
                    options.XGetter = (value, index) => index;
                    options.YGetter = (value, index) => value;
                })
                .AsPie(options =>
                {
                    options.ValueGetter = (value, index) => value;
                });

            config.ConfigureType<uint>()
                .AsCartesian(options =>
                {
                    options.XGetter = (value, index) => index;
                    options.YGetter = (value, index) => value;
                })
                .AsPie(options =>
                {
                    options.ValueGetter = (value, index) => value;
                });

            config.ConfigureType<long>()
                .AsCartesian(options =>
                {
                    options.XGetter = (value, index) => index;
                    options.YGetter = (value, index) => value;
                })
                .AsPie(options =>
                {
                    options.ValueGetter = (value, index) => value;
                });

            config.ConfigureType<ulong>()
                .AsCartesian(options =>
                {
                    options.XGetter = (value, index) => index;
                    options.YGetter = (value, index) => value;
                })
                .AsPie(options =>
                {
                    options.ValueGetter = (value, index) => value;
                });

            config.ConfigureType<double>()
                .AsCartesian(options =>
                {
                    options.XGetter = (value, index) => index;
                    options.YGetter = (value, index) => value;
                })
                .AsPie(options =>
                {
                    options.ValueGetter = (value, index) => value;
                });

            config.ConfigureType<float>()
                .AsCartesian(options =>
                {
                    options.XGetter = (value, index) => index;
                    options.YGetter = (value, index) => value;
                })
                .AsPie(options =>
                {
                    options.ValueGetter = (value, index) => value;
                });

            return config;
        }

        /// <summary>
        /// Configures LiveCharts to plot the already the default objects at LiveCharts.Core.Defaults namespace.
        /// </summary>
        /// <param name="config">The configuration.</param>
        /// <returns></returns>
        public static LiveChartsConfig AddDefaultPlotObjects(this LiveChartsConfig config)
        {
            config.ConfigureType<decimal>()
                .AsCartesian(options =>
                {
                    options.XGetter = (value, index) => index;
                    options.YGetter = (value, index) => (double)value;
                })
                .AsPie(options =>
                {
                    options.ValueGetter = (value, index) => (double)value;
                });

            config.ConfigureType<Observable>()
                .AsCartesian(options =>
                {
                    options.XGetter = (value, index) => index;
                    options.YGetter = (observable, index) => observable.Value;
                })
                .AsPie(options =>
                {
                    options.ValueGetter = (observable, index) => observable.Value;
                });

            config.ConfigureType<ObservablePoint>()
                .AsCartesian(options =>
                {
                    options.XGetter = (observable, index) => observable.X;
                    options.YGetter = (observable, index) => observable.Y;
                });

            config.ConfigureType<PolarPoint>()
                .AsPolar(options =>
                {
                    options.RadiusGetter = (observable, index) => observable.Radius;
                    options.AngleGetter = (observable, index) => observable.Angle;
                });

            config.ConfigureType<FinancialPoint>()
                .AsFinancial(options =>
                {
                    options.OpenGetter = (observable, index) => observable.Open;
                    options.HighGetter = (observable, index) => observable.High;
                    options.LowGetter = (observable, index) => observable.Low;
                    options.CloseGetter = (observable, index) => observable.Close;
                });

            config.ConfigureType<WeightedPoint>()
                .AsWeighted(buidler =>
                {
                    buidler.XGetter = (observalbe, index) => observalbe.X;
                    buidler.YGetter = (observalbe, index) => observalbe.Y;
                    buidler.WeightGetter = (observalbe, index) => observalbe.Weight;
                });

            return config;
        }
    }
}