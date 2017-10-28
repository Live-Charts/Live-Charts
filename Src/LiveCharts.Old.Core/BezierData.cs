namespace LiveChartsCore
{
    public class BezierData
    {
        public BezierData()
        {
        }

        public BezierData(LvcPoint point)
        {
            Point1 = point;
            Point2 = point;
            Point3 = point;
        }

        public LvcPoint Point1 { get; set; }
        public LvcPoint Point2 { get; set; }
        public LvcPoint Point3 { get; set; }
        public LvcPoint StartPoint { get; set; }
    }
}