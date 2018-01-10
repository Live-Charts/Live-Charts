using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LiveCharts.Core.UnitTests
{
    [TestClass]
    public class PerfomanceTest
    {
        private static Random r = new Random();
        private Func<double> g = () => r.Next();

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
