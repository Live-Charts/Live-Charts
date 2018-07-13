﻿#region License
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

namespace LiveCharts.Core.Interaction.Series
{
    /// <summary>
    /// A class containing series information.
    /// </summary>
    public struct SeriesMetadata
    {
        /// <summary>
        /// Gets the type of the model.
        /// </summary>
        /// <value>
        /// The type of the model.
        /// </value>
        public Type ModelType { get; internal set; }

        /// <summary>
        /// Gets a value indicating whether this instance is referenced type.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is referenced type; otherwise, <c>false</c>.
        /// </value>
        public bool IsValueType { get; internal set; }

        /// <summary>
        /// Gets a value indicating whether this instance is observable.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is observable; otherwise, <c>false</c>.
        /// </value>
        public bool IsObservable { get; internal set; }
    }
}
