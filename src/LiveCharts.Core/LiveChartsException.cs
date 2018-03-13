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

#endregion

namespace LiveCharts.Core
{
    /// <summary>
    /// An Exception thrown when there is an error related with LiveCharts.
    /// </summary>
    /// <seealso cref="System.Exception" />
    public sealed class LiveChartsException : Exception
    {
        private static readonly string _baseErrorUri = "http://lvcharts.net/error/";

        /// <summary>
        /// Initializes a new instance of the <see cref="LiveChartsException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="code">the error code</param>
        public LiveChartsException(string message, int code)
            : base(message)
        {
            HelpLink = _baseErrorUri + code;
        }
    }
}

