using System;
using System.Globalization;
using Windows.Foundation;
using Windows.UI.Xaml.Media;

namespace LiveCharts.Uwp.Components
{
    /// <summary>
    /// A String-To-PathGeometry Parser from https://stringtopathgeometry.codeplex.com/
    /// </summary>
    internal class GeometryParser
    {
        #region Const & Private Variables
        const bool AllowSign = true;
        const bool AllowComma = true;
        const bool IsFilled = true;
        const bool IsClosed = true;

        IFormatProvider _formatProvider;

        PathFigure _figure;     // Figure object, which will accept parsed segments
        string _pathString;        // Input string to be parsed
        int _pathLength;
        int _curIndex;          // Location to read next character from
        bool _figureStarted;     // StartFigure is effective 

        Point _lastStart;         // Last figure starting point
        Point _lastPoint;         // Last point 
        Point _secondLastPoint;   // The point before last point

        char _token;             // Non whitespace character returned by ReadToken
        #endregion

        #region Public Functionality
        /// <summary>
        /// Main conversion routine - converts string path data definition to PathGeometry object
        /// </summary>
        /// <param name="path">String with path data definition</param>
        /// <returns>PathGeometry object created from string definition</returns>
        public PathGeometry Convert(string path)
        {
            if (path == null)
                throw new ArgumentException("Path string cannot be null!", nameof(path));

            if (path.Length == 0)
                throw new ArgumentException("Path string cannot be empty!", nameof(path));

            return Parse(path);
        }

        /// <summary>
        /// Main back conversion routine - converts PathGeometry object to its string equivalent
        /// </summary>
        /// <param name="geometry">Path Geometry object</param>
        /// <returns>String equivalent to PathGeometry contents</returns>
        public string ConvertBack(PathGeometry geometry)
        {
            if (geometry == null)
                throw new ArgumentException("Path Geometry cannot be null!", nameof(geometry));

            return ParseBack(geometry);
        }
        #endregion

        #region Private Functionality
        /// <summary>
        /// Main parser routine, which loops over each char in received string, and performs actions according to command/parameter being passed
        /// </summary>
        /// <param name="path">String with path data definition</param>
        /// <returns>PathGeometry object created from string definition</returns>
        private PathGeometry Parse(string path)
        {
            PathGeometry pathGeometry = null;

            _formatProvider = CultureInfo.InvariantCulture;
            _pathString = path;
            _pathLength = path.Length;
            _curIndex = 0;

            _secondLastPoint = new Point(0, 0);
            _lastPoint = new Point(0, 0);
            _lastStart = new Point(0, 0);

            _figureStarted = false;

            var first = true;

            var lastCmd = ' ';

            while (ReadToken()) // Empty path is allowed in XAML
            {
                char cmd = _token;

                if (first)
                {
                    if ((cmd != 'M') && (cmd != 'm') && (cmd != 'f') && (cmd != 'F'))  // Path starts with M|m 
                    {
                        ThrowBadToken();
                    }

                    first = false;
                }

                switch (cmd)
                {
                    case 'f':
                    case 'F':
                        pathGeometry = new PathGeometry();
                        var num = ReadNumber(!AllowComma);
                        // ReSharper disable once CompareOfFloatsByEqualityOperator
                        pathGeometry.FillRule = num == 0 ? FillRule.EvenOdd : FillRule.Nonzero;
                        break;

                    case 'm':
                    case 'M':
                        // XAML allows multiple points after M/m
                        _lastPoint = ReadPoint(cmd, !AllowComma);

                        _figure = new PathFigure
                        {
                            StartPoint = _lastPoint,
                            IsFilled = IsFilled,
                            IsClosed = !IsClosed
                        };
                        //context.BeginFigure(_lastPoint, IsFilled, !IsClosed);
                        _figureStarted = true;
                        _lastStart = _lastPoint;
                        lastCmd = 'M';

                        while (IsNumber(AllowComma))
                        {
                            _lastPoint = ReadPoint(cmd, !AllowComma);

                            var lineSegment = new LineSegment {Point = _lastPoint};
                            _figure.Segments.Add(lineSegment);
                            //context.LineTo(_lastPoint, IsStroked, !IsSmoothJoin);
                            lastCmd = 'L';
                        }
                        break;

                    case 'l':
                    case 'L':
                    case 'h':
                    case 'H':
                    case 'v':
                    case 'V':
                        EnsureFigure();

                        do
                        {
                            switch (cmd)
                            {
                                case 'l': _lastPoint = ReadPoint(cmd, !AllowComma); break;
                                case 'L': _lastPoint = ReadPoint(cmd, !AllowComma); break;
                                case 'h': _lastPoint.X += ReadNumber(!AllowComma); break;
                                case 'H': _lastPoint.X = ReadNumber(!AllowComma); break;
                                case 'v': _lastPoint.Y += ReadNumber(!AllowComma); break;
                                case 'V': _lastPoint.Y = ReadNumber(!AllowComma); break;
                            }

                            var lineSegment = new LineSegment {Point = _lastPoint};
                            _figure.Segments.Add(lineSegment);
                            //context.LineTo(_lastPoint, IsStroked, !IsSmoothJoin);
                        }
                        while (IsNumber(AllowComma));

                        lastCmd = 'L';
                        break;

                    case 'c':
                    case 'C': // cubic Bezier 
                    case 's':
                    case 'S': // smooth cublic Bezier
                        EnsureFigure();

                        do
                        {
                            Point p;

                            if ((cmd == 's') || (cmd == 'S'))
                            {
                                p = lastCmd == 'C' ? Reflect() : _lastPoint;

                                _secondLastPoint = ReadPoint(cmd, !AllowComma);
                            }
                            else
                            {
                                p = ReadPoint(cmd, !AllowComma);

                                _secondLastPoint = ReadPoint(cmd, AllowComma);
                            }

                            _lastPoint = ReadPoint(cmd, AllowComma);

                            var bizierSegment = new BezierSegment
                            {
                                Point1 = p,
                                Point2 = _secondLastPoint,
                                Point3 = _lastPoint
                            };
                            _figure.Segments.Add(bizierSegment);
                            //context.BezierTo(p, _secondLastPoint, _lastPoint, IsStroked, !IsSmoothJoin);

                            lastCmd = 'C';
                        }
                        while (IsNumber(AllowComma));

                        break;

                    case 'q':
                    case 'Q': // quadratic Bezier 
                    case 't':
                    case 'T': // smooth quadratic Bezier
                        EnsureFigure();

                        do
                        {
                            if ((cmd == 't') || (cmd == 'T'))
                            {
                                _secondLastPoint = lastCmd == 'Q' ? Reflect() : _lastPoint;

                                _lastPoint = ReadPoint(cmd, !AllowComma);
                            }
                            else
                            {
                                _secondLastPoint = ReadPoint(cmd, !AllowComma);
                                _lastPoint = ReadPoint(cmd, AllowComma);
                            }

                            var quadraticBezierSegment = new QuadraticBezierSegment
                            {
                                Point1 = _secondLastPoint,
                                Point2 = _lastPoint
                            };
                            _figure.Segments.Add(quadraticBezierSegment);
                            //context.QuadraticBezierTo(_secondLastPoint, _lastPoint, IsStroked, !IsSmoothJoin);

                            lastCmd = 'Q';
                        }
                        while (IsNumber(AllowComma));

                        break;

                    case 'a':
                    case 'A':
                        EnsureFigure();

                        do
                        {
                            // A 3,4 5, 0, 0, 6,7
                            var w = ReadNumber(!AllowComma);
                            var h = ReadNumber(AllowComma);
                            var rotation = ReadNumber(AllowComma);
                            var large = ReadBool();
                            var sweep = ReadBool();

                            _lastPoint = ReadPoint(cmd, AllowComma);

                            var arcSegment = new ArcSegment
                            {
                                Point = _lastPoint,
                                Size = new Size(w, h),
                                RotationAngle = rotation,
                                IsLargeArc = large,
                                SweepDirection = sweep ? SweepDirection.Clockwise : SweepDirection.Counterclockwise
                            };
                            _figure.Segments.Add(arcSegment);
                            //context.ArcTo(
                            //    _lastPoint,
                            //    new Size(w, h),
                            //    rotation,
                            //    large,
                            //    sweep ? SweepDirection.Clockwise : SweepDirection.Counterclockwise,
                            //    IsStroked,
                            //    !IsSmoothJoin
                            //    );
                        }
                        while (IsNumber(AllowComma));

                        lastCmd = 'A';
                        break;

                    case 'z':
                    case 'Z':
                        EnsureFigure();
                        _figure.IsClosed = IsClosed;
                        //context.SetClosedState(IsClosed);

                        _figureStarted = false;
                        lastCmd = 'Z';

                        _lastPoint = _lastStart; // Set reference point to be first point of current figure
                        break;

                    default:
                        ThrowBadToken();
                        break;
                }

                if (null != _figure)
                {
                    if (_figure.IsClosed)
                    {
                        if (null == pathGeometry)
                            pathGeometry = new PathGeometry();

                        pathGeometry.Figures.Add(_figure);

                        _figure = null;
                        first = true;
                    }
                }


            }

            if (null != _figure)
            {
                if (null == pathGeometry)
                    pathGeometry = new PathGeometry();

                if (!pathGeometry.Figures.Contains(_figure))
                    pathGeometry.Figures.Add(_figure);

            }
            return pathGeometry;
        }

        void SkipDigits(bool signAllowed)
        {
            // Allow for a sign 
            if (signAllowed && More() && ((_pathString[_curIndex] == '-') || _pathString[_curIndex] == '+'))
            {
                _curIndex++;
            }

            while (More() && (_pathString[_curIndex] >= '0') && (_pathString[_curIndex] <= '9'))
            {
                _curIndex++;
            }
        }

        bool ReadBool()
        {
            SkipWhiteSpace(AllowComma);

            if (More())
            {
                _token = _pathString[_curIndex++];

                if (_token == '0')
                {
                    return false;
                }
                else if (_token == '1')
                {
                    return true;
                }
            }

            ThrowBadToken();

            return false;
        }

        private Point Reflect()
        {
            return new Point(2 * _lastPoint.X - _secondLastPoint.X,
                             2 * _lastPoint.Y - _secondLastPoint.Y);
        }

        private void EnsureFigure()
        {
            if (!_figureStarted)
            {
                _figure = new PathFigure {StartPoint = _lastStart};

                //_context.BeginFigure(_lastStart, IsFilled, !IsClosed);
                _figureStarted = true;
            }
        }

        double ReadNumber(bool allowComma)
        {
            if (!IsNumber(allowComma))
            {
                ThrowBadToken();
            }

            bool simple = true;
            int start = _curIndex;

            //
            // Allow for a sign
            //
            // There are numbers that cannot be preceded with a sign, for instance, -NaN, but it's 
            // fine to ignore that at this point, since the CLR parser will catch this later.
            // 
            if (More() && ((_pathString[_curIndex] == '-') || _pathString[_curIndex] == '+'))
            {
                _curIndex++;
            }

            // Check for Infinity (or -Infinity).
            if (More() && (_pathString[_curIndex] == 'I'))
            {
                // 
                // Don't bother reading the characters, as the CLR parser will 
                // do this for us later.
                // 
                _curIndex = Math.Min(_curIndex + 8, _pathLength); // "Infinity" has 8 characters
                simple = false;
            }
            // Check for NaN 
            else if (More() && (_pathString[_curIndex] == 'N'))
            {
                // 
                // Don't bother reading the characters, as the CLR parser will
                // do this for us later. 
                //
                _curIndex = Math.Min(_curIndex + 3, _pathLength); // "NaN" has 3 characters
                simple = false;
            }
            else
            {
                SkipDigits(!AllowSign);

                // Optional period, followed by more digits 
                if (More() && (_pathString[_curIndex] == '.'))
                {
                    simple = false;
                    _curIndex++;
                    SkipDigits(!AllowSign);
                }

                // Exponent
                if (More() && ((_pathString[_curIndex] == 'E') || (_pathString[_curIndex] == 'e')))
                {
                    simple = false;
                    _curIndex++;
                    SkipDigits(AllowSign);
                }
            }

            if (simple && (_curIndex <= (start + 8))) // 32-bit integer
            {
                int sign = 1;

                if (_pathString[start] == '+')
                {
                    start++;
                }
                else if (_pathString[start] == '-')
                {
                    start++;
                    sign = -1;
                }

                int value = 0;

                while (start < _curIndex)
                {
                    value = value * 10 + (_pathString[start] - '0');
                    start++;
                }

                return value * sign;
            }
            else
            {
                string subString = _pathString.Substring(start, _curIndex - start);

                try
                {
                    return System.Convert.ToDouble(subString, _formatProvider);
                }
                catch (FormatException except)
                {
                    throw new FormatException(string.Format("Unexpected character in path '{0}' at position {1}", _pathString, _curIndex - 1), except);
                }
            }
        }

        private bool IsNumber(bool allowComma)
        {
            bool commaMet = SkipWhiteSpace(allowComma);

            if (More())
            {
                _token = _pathString[_curIndex];

                // Valid start of a number
                if ((_token == '.') || (_token == '-') || (_token == '+') || ((_token >= '0') && (_token <= '9'))
                    || (_token == 'I')  // Infinity
                    || (_token == 'N')) // NaN 
                {
                    return true;
                }
            }

            if (commaMet) // Only allowed between numbers
            {
                ThrowBadToken();
            }

            return false;
        }

        private Point ReadPoint(char cmd, bool allowcomma)
        {
            double x = ReadNumber(allowcomma);
            double y = ReadNumber(AllowComma);

            if (cmd >= 'a') // 'A' < 'a'. lower case for relative
            {
                x += _lastPoint.X;
                y += _lastPoint.Y;
            }

            return new Point(x, y);
        }

        private bool ReadToken()
        {
            SkipWhiteSpace(!AllowComma);

            // Check for end of string 
            if (More())
            {
                _token = _pathString[_curIndex++];

                return true;
            }
            else
            {
                return false;
            }
        }

        bool More()
        {
            return _curIndex < _pathLength;
        }

        // Skip white space, one comma if allowed
        private bool SkipWhiteSpace(bool allowComma)
        {
            bool commaMet = false;

            while (More())
            {
                char ch = _pathString[_curIndex];

                switch (ch)
                {
                    case ' ':
                    case '\n':
                    case '\r':
                    case '\t': // SVG whitespace 
                        break;

                    case ',':
                        if (allowComma)
                        {
                            commaMet = true;
                            allowComma = false; // one comma only
                        }
                        else
                        {
                            ThrowBadToken();
                        }
                        break;

                    default:
                        // Avoid calling IsWhiteSpace for ch in (' ' .. 'z']
                        if (((ch > ' ') && (ch <= 'z')) || !Char.IsWhiteSpace(ch))
                        {
                            return commaMet;
                        }
                        break;
                }

                _curIndex++;
            }

            return commaMet;
        }

        private void ThrowBadToken()
        {
            throw new FormatException(string.Format("Unexpected character in path '{0}' at position {1}", _pathString, _curIndex - 1));
        }

        internal static char GetNumericListSeparator(IFormatProvider provider)
        {
            var numericSeparator = ',';

            // Get the NumberFormatInfo out of the provider, if possible
            // If the IFormatProvider doesn't not contain a NumberFormatInfo, then 
            // this method returns the current culture's NumberFormatInfo. 
            var numberFormat = NumberFormatInfo.GetInstance(provider);

            // Is the decimal separator is the same as the list separator?
            // If so, we use the ";". 
            if ((numberFormat.NumberDecimalSeparator.Length > 0) && (numericSeparator == numberFormat.NumberDecimalSeparator[0]))
            {
                numericSeparator = ';';
            }

            return numericSeparator;
        }

        private string ParseBack(PathGeometry geometry)
        {
            var sb = new System.Text.StringBuilder();
            IFormatProvider provider = new CultureInfo("en-us");

            sb.Append("F" + (geometry.FillRule == FillRule.EvenOdd ? "0" : "1") + " ");

            foreach (PathFigure figure in geometry.Figures)
            {
                sb.Append("M " + ((IFormattable)figure.StartPoint).ToString(null, provider) + " ");

                foreach (PathSegment segment in figure.Segments)
                {
                    char separator = GetNumericListSeparator(provider);

                    if (segment is LineSegment)
                    {
                        LineSegment lineSegment = segment as LineSegment;

                        sb.Append("L " + ((IFormattable)lineSegment.Point).ToString(null, provider) + " ");
                    }
                    else if (segment is BezierSegment)
                    {
                        BezierSegment bezierSegment = segment as BezierSegment;

                        sb.Append(string.Format(provider,
                             "C{1:" + null + "}{0}{2:" + null + "}{0}{3:" + null + "} ",
                             separator,
                             bezierSegment.Point1,
                             bezierSegment.Point2,
                             bezierSegment.Point3
                             ));
                    }
                    else if (segment is QuadraticBezierSegment)
                    {
                        QuadraticBezierSegment quadraticBezierSegment = segment as QuadraticBezierSegment;

                        sb.Append(String.Format(provider,
                             "Q{1:" + null + "}{0}{2:" + null + "} ",
                             separator,
                             quadraticBezierSegment.Point1,
                             quadraticBezierSegment.Point2));
                    }
                    else if (segment is ArcSegment)
                    {
                        ArcSegment arcSegment = segment as ArcSegment;

                        sb.Append(String.Format(provider,
                             "A{1:" + null + "}{0}{2:" + null + "}{0}{3}{0}{4}{0}{5:" + null + "} ",
                             separator,
                             arcSegment.Size,
                             arcSegment.RotationAngle,
                             arcSegment.IsLargeArc ? "1" : "0",
                             arcSegment.SweepDirection == SweepDirection.Clockwise ? "1" : "0",
                             arcSegment.Point));
                    }
                }

                if (figure.IsClosed)
                    sb.Append("Z");
            }

            return sb.ToString();
        }
        #endregion
    }
}
