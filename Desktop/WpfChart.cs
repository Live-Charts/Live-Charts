using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using LiveChartsCore;

namespace Desktop
{
    internal abstract class WpfChart : UserControl, IChartView, IChartModel
    {
        protected WpfChart()
        {
            Core = new ChartModel(this);
        }

        public ChartModel Core { get; set; }

        public static readonly DependencyProperty SeriesProperty = DependencyProperty.Register(
            "Series", typeof (SeriesCollection), typeof (WpfChart),
            new PropertyMetadata(default(SeriesCollection), UpdateSeriesCallBack));

        public int ColorIndex { get; set; }

        public SeriesCollection Series
        {
            get { return (SeriesCollection) GetValue(SeriesProperty); }
            set { SetValue(SeriesProperty, value); }
        }

        public void Update(bool restartsAnimation)
        {
            Core.Update(restartsAnimation);
        }

        public void PrepareCanvas(bool restartAnimation)
        {
            if (Series == null) return;

            //if (!SeriesInitialized) InitializeSeries();

            InitializeSeries();
        }

        public void UpdateSeries()
        {
            throw new NotImplementedException();
        }

        private void InitializeSeries()
        {
            foreach (var series in Series)
            {
                var index = ColorIndex++;
                series.Chart = this;
                series.Collection = Series;
                series.Stroke = series.Stroke ?? new SolidColorBrush(Colors[(int)(index - Colors.Count * Math.Truncate(index / (decimal)Colors.Count))]);
                series.Fill = series.Fill ?? new SolidColorBrush(Colors[(int)(index - Colors.Count * Math.Truncate(index / (decimal)Colors.Count))])
                {
                    Opacity = DefaultFillOpacity
                };
                series.RequiresPlot = true;
                series.RequiresAnimation = true;
                var observable = series.Values as INotifyCollectionChanged;
                if (observable != null)
                    observable.CollectionChanged += OnDataSeriesChanged;
            }
        }

        private static void UpdateSeriesCallBack(DependencyObject o, DependencyPropertyChangedEventArgs args)
        {
            var chart = o as IChartModel;
            if (chart == null) return;
            chart.Update(true);
        }
    }

    public class WpfSeries : FrameworkElement, ISeriesModel
    {
        public IChartModel Chart { get; set; }
        public IChartValues Values { get; set; }
        public SeriesCollection Collection { get; set; }
        public ISeriesConfiguration Configuration { get; set; }
    }
}
