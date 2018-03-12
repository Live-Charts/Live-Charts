using System;

namespace LiveCharts.Core.Data
{
    /// <summary>
    /// A helper class.
    /// </summary>
    public static class Formatters
    {
        /// <summary>
        /// Converts a double number to a short string, based on the metric convention. 
        /// </summary>
        /// <param name="number">The number.</param>
        /// <returns></returns>
        public static string AsMetricNumber(double number)
        {
            const double pico = 0.000000000001;
            const double nano = 0.000000001;
            const double micro = 0.000001;
            const double mili = 0.001;
            const double kilo = 1000;
            const double mega = 1000000;
            const double giga = 1000000000;
            const double tera = 1000000000000;

            var log = Math.Abs(number) < pico ? 0 : Math.Log10(Math.Abs(number));
            string Func(double @const) => Math.Round(number / @const, 2).ToString("N2");

            if (log >= 12) return $"{Func(tera)} T";
            if (log >= 9) return $"{Func(giga)} G";
            if (log >= 6) return $"{Func(mega)} M";
            if (log >= 3) return $"{Func(kilo)} k";
            if (log >= -3) return Func(1);
            if (log >= -6) return $"{Func(mili)} m";
            if (log >= -9) return $"{Func(micro)} µ";
            if (log >= -12) return $"{Func(nano)} n";
            return $"{Func(pico)} p";
        }
    }
}
