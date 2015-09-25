using System;
using System.Globalization;

namespace Charts
{
    public static class LabelFormatters
    {
        public static Func<double, string> Currency => x =>
        {
            var a = Math.Abs(x);
            if (a < 1000)  return x.ToString("$#,#.00", CultureInfo.InvariantCulture);
            if (a < 1000000) return x.ToString("$#,##0,K", CultureInfo.InvariantCulture);
            return x.ToString("$#,##0,,M", CultureInfo.InvariantCulture);
        };
        public static Func<double, string> Number => x =>
        {
            var a = Math.Abs(x);
            if (a < 1000) return x.ToString("#,#", CultureInfo.InvariantCulture);
            if (a < 1000000) return x.ToString("#,##0,K", CultureInfo.InvariantCulture);
            return x.ToString("#,##0,,M", CultureInfo.InvariantCulture);
        };
        public static Func<double, string> Precent => x => x.ToString("P");
    }
}
