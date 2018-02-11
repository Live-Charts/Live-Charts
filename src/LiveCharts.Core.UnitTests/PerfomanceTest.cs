using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using LiveCharts.Core.Coordinates;
using LiveCharts.Core.DataSeries;
using LiveCharts.Core.Drawing;
using LiveCharts.Core.UnitTests.Mocked;
using LiveCharts.Core.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LiveCharts.Core.UnitTests
{
    [TestClass]
    public class PerfomanceTest
    {
        private static Random r = new Random();
        private Func<double> g = () => r.Next();

        [TestMethod]
        public void RandomTest()
        {
            var chart = new CartesianChart();

            var series = new LineSeries<double> {2, 4, 8, 16, 32};
            series.LineSmoothness = 1;

            chart.Series = new SeriesCollection
            {
                series
            };

            chart.Updated += sender =>
            {
                var l = series.Points.Select(p =>
                {
                    return new[]
                    {
                        p.ViewModel.Point1,
                        p.ViewModel.Point2,
                        p.ViewModel.Point3,
                    };
                }).ToArray();

                var pts = new List<Point>()
                {
                    new Point(0, 2),
                    new Point(1, 4),
                    new Point(2, 8),
                    new Point(3, 16),
                    new Point(4, 32)
                };

                var x = chart.Dimensions[0][0];
                var y = chart.Dimensions[1][0];
                var smoothness = 1;
                var s = new Size(585,570);

                var tt = chart.Model.ScaleToUi(new Point(0, 2), x, y, s);

                var index = 0;
                var p0 = pts.Count > 0
                    ? pts[0]
                    : new Point(0, 0);
                var p1 = pts.Count > 0
                    ? pts[0]
                    : p0;
                var p2 = pts.Count > 1
                    ? pts[1]
                    : p1;
                var p3 = pts.Count > 2
                    ? pts[2]
                    : p2;

                foreach (var pt in pts)
                {
                    var xc1 = (p0.X + p1.X) / 2.0;
                    var yc1 = (p0.Y + p1.Y) / 2.0;
                    var xc2 = (p1.X + p2.X) / 2.0;
                    var yc2 = (p1.Y + p2.Y) / 2.0;
                    var xc3 = (p2.X + p3.X) / 2.0;
                    var yc3 = (p2.Y + p3.Y) / 2.0;

                    var len1 = Math.Sqrt((p1.X - p0.X) * (p1.X - p0.X) + (p1.Y - p0.Y) * (p1.Y - p0.Y));
                    var len2 = Math.Sqrt((p2.X - p1.X) * (p2.X - p1.X) + (p2.Y - p1.Y) * (p2.Y - p1.Y));
                    var len3 = Math.Sqrt((p3.X - p2.X) * (p3.X - p2.X) + (p3.Y - p2.Y) * (p3.Y - p2.Y));

                    var k1 = len1 / (len1 + len2);
                    var k2 = len2 / (len2 + len3);

                    if (double.IsNaN(k1)) k1 = 0d;
                    if (double.IsNaN(k2)) k2 = 0d;

                    var xm1 = xc1 + (xc2 - xc1) * k1;
                    var ym1 = yc1 + (yc2 - yc1) * k1;
                    var xm2 = xc2 + (xc3 - xc2) * k2;
                    var ym2 = yc2 + (yc3 - yc2) * k2;

                    var c1X = xm1 + (xc2 - xm1) * smoothness + p1.X - xm1;
                    var c1Y = ym1 + (yc2 - ym1) * smoothness + p1.Y - ym1;
                    var c2X = xm2 + (xc2 - xm2) * smoothness + p2.X - xm2;
                    var c2Y = ym2 + (yc2 - ym2) * smoothness + p2.Y - ym2;

                    var r = new List<Point>
                    {
                        index == 0 ? new Point(p1.X, p1.Y) : new Point(c1X, c1Y),
                        new Point(c2X, c2Y),
                        new Point(p2.X, p2.Y)
                    };

                    var ll = l[index];

                    var a = ll[0];
                    var a1 = r[0];
                    var ae = ll[0] == r[0];
                    var b = ll[1] == r[1];
                    var c = ll[2] == r[2];

                    p0 = new Point(p1.X, p1.Y);
                    p1 = new Point(p2.X, p2.Y);
                    p2 = new Point(p3.X, p3.Y);
                    p3 = pts.Count > index + 3
                        ? chart.Model.ScaleToUi(pts[index + 3], x, y)
                        : p2;

                    index++;
                }
            };

            Thread.Sleep((int) TimeSpan.FromHours(1).TotalMilliseconds);
        }

        [TestMethod]
        public void DelegateVsMethodTest()
        {
            const int iterations = 100000000;
            const int tests = 1;

            var sum = 0d;

            for (int j = 0; j < tests; j++)
            {
                var sw = new Stopwatch();
                sw.Start();
                for (int i = 0; i < iterations; i++)
                {
                    var c = GetRandom();
                }
                sw.Stop();
                var sw2 = new Stopwatch();
                sw2.Start();
                for (int i = 0; i < iterations; i++)
                {
                    var b = g();
                }
                sw2.Stop();
                sum += sw.ElapsedMilliseconds - sw2.ElapsedMilliseconds;
            }

            using (var writter = new StreamWriter($"DelegateVsMethodTest {iterations/1000000} million times.txt"))
            {
                writter.WriteLine(sum/tests);
                // 100 million calls, the difference is around 120-200m
                // is it worth?
                // using Action/Func allows us to have a better user experience
            }
        }

        [TestMethod]
        public void CopyTo()
        {
            const int itemsCount = 100000000;

            var r = new Random();
            var source = new int[itemsCount];
            var sw = new Stopwatch();
            for (var i = 0; i < itemsCount; i++)
            {
                source[i] = r.Next();
            }
            sw.Start();
            var destiny = new int[itemsCount];
            source.CopyTo(destiny, 0);
            sw.Stop();

            using (var writter = new StreamWriter("copyTo.txt"))
            {
                writter.WriteLine($"{sw.ElapsedMilliseconds} ms");
            }
        }

        [TestMethod]
        public void CastingImpactTest()
        {
            const int iterations = 100000000;
            const int tests = 50;

            var t1 = 0d;
            for (int i = 0; i < tests; i++)
            {
                var sw = new Stopwatch();
                sw.Start();
                foreach (var myStruct in GetStruct(iterations))
                {
                    var a = myStruct.A;
                }
                sw.Stop();
                var sw1 = new Stopwatch();
                sw1.Start();
                foreach (var obj in GetStackObject(iterations))
                {
                    var a = ((MyStruct)obj).A;
                }
                sw1.Stop();
                t1 += sw1.ElapsedMilliseconds - sw.ElapsedMilliseconds;
            }

            using (var w = new StreamWriter($"boxing impact {iterations/1000000} million times.txt"))
            {
                w.WriteLine(t1*1000000/iterations);
            }

            var t2 = 0d;
            for (int i = 0; i < tests; i++)
            {
                var sw2 = new Stopwatch();
                sw2.Start();
                foreach (var myClass in GetClass(iterations))
                {
                    var a = myClass.A;
                }
                sw2.Stop();
                var sw3 = new Stopwatch();
                sw3.Start();
                foreach (var obj in GetHeapObject(iterations))
                {
                    var a = ((MyClass)obj).A;
                }
                sw3.Stop();
                t2 += sw3.ElapsedMilliseconds - sw2.ElapsedMilliseconds;
            }

            using (var w = new StreamWriter($"casting impact {iterations/1000000} million times.txt"))
            {
                w.WriteLine(t2*1000000/iterations);
            }
        }

        private static IEnumerable<object> GetStackObject(int iterations)
        {
            for (int i = 0; i < iterations; i++)
            {
                yield return new MyStruct();
            }
        }

        private static IEnumerable<MyStruct> GetStruct(int iterations)
        {
            for (int i = 0; i < iterations; i++)
            {
                yield return new MyStruct();
            }
        }

        private IEnumerable<object> GetHeapObject(int iterations)
        {
            for (var index = 0; index < iterations; index++)
            {
                yield return new MyClass();
            }
        }

        private IEnumerable<MyClass> GetClass(int iterations)
        {
            for (int i = 0; i < iterations; i++)
            {
                yield return new MyClass();
            }
        }

        private struct MyStruct
        {
            public double A { get; set; }
        }

        private class MyClass
        {
            public double A { get; set; }
        }

        private double GetRandom()
        {
            return r.Next();
        }
    }

    
}
