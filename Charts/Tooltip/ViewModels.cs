namespace LiveCharts.Tooltip
{
    public class IndexedTooltipViewModel
    {
        public string Label { get; set; }

        public IndexedTooltipData[] Data { get; set; }
    }

    public class IndexedTooltipData
    {
        public Series Series { get; set; }
        public string Value { get; set; }
    }

    public class ScatterTooltipViewModel
    {
        public ScatterSeries Series { get; set; }
        public string PrimaryAxisTitle { get; set; }
        public string PrimaryValue { get; set; }
        public string SecondaryAxisTitle { get; set; }
        public string SecondaryValue { get; set; }
    }
}
