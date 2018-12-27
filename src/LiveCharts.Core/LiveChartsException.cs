#region License
// The MIT License (MIT)
// 
// Copyright (c) 2016 Alberto Rodríguez Orozco & LiveCharts contributors
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy 
// of this software and associated documentation files (the "Software"), to deal 
// in the Software without restriction, including without limitation the rights to 
// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies 
// of the Software, and to permit persons to whom the Software is furnished to 
// do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all 
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, 
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES 
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND 
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT 
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, 
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING 
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR 
// OTHER DEALINGS IN THE SOFTWARE.
#endregion
#region

using System;
using System.IO;
using System.Linq;
using System.Reflection;

#endregion

namespace LiveCharts.Core
{
    /// <summary>
    /// An Exception thrown when there is an error related with LiveCharts.
    /// </summary>
    /// <seealso cref="Exception" />
    public sealed class LiveChartsException : Exception
    {
        private static readonly string _baseErrorUri = "http://lvcharts.net/error/";

        /// <summary>
        /// Initializes a new instance of the <see cref="LiveChartsException"/> class.
        /// </summary>
        /// <param name="code">the error code</param>
        /// <param name="parameters">The parameters to inject.</param>
        public LiveChartsException(int code, params object[]? parameters)
            :base(GetMessage(code, parameters))
        { 
            HelpLink = _baseErrorUri + code;
        }

        private static string GetMessage(int code, object[]? parameters)
        {
            string message;

            var assembly = Assembly.GetExecutingAssembly();
            const string resourceName = "LiveCharts.Core.Assets.exceptions.txt";

            try
            {
                using (var stream = assembly.GetManifestResourceStream(resourceName))
                using (var reader = new StreamReader(stream ?? throw new InvalidOperationException()))
                {
                    string content = reader.ReadToEnd();

                    string[] byLine = content.Split(
                        new[] { Environment.NewLine },
                        StringSplitOptions.None);

                    var error = byLine
                        .Select(x =>
                        {
                            if (string.IsNullOrWhiteSpace(x)) return new {code = 0, message = ""};
                            string[] byField = x.Split(',');
                            return new { code = int.Parse(byField[0]), message = byField[1] };
                        })
                        .FirstOrDefault(x => x.code == code);

                    message = error == null
                        ? $"An unknown exception was thrown, exception code {code}."
                        : string.Format(error.message, parameters);
                }
            }
            catch
            {
                message = $"Oops! something went wrong when trying to build a LiveCharts exception, exception code {code}.";
            }

            return message;
        }
    }
}

