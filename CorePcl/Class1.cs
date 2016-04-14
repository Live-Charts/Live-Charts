using System;
using System.Diagnostics;

namespace LvCore
{
    public abstract class Chart
    {
        protected virtual void MeasureCanvas()
        {
            throw new Exception("Canvas not Set!");
        }
    }
}
