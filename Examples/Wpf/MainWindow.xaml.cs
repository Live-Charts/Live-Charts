using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Input;
using Wpf.Annotations;
using Wpf.CartesianChart;
using Wpf.CartesianChart.BasicLine;
using Wpf.CartesianChart.Basic_Bars;
using Wpf.CartesianChart.Basic_Stacked_Bar;
using Wpf.CartesianChart.Customized_Line_Series;
using Wpf.CartesianChart.CustomTooltipAndLegend;
using Wpf.CartesianChart.DynamicVisibility;
using Wpf.CartesianChart.Energy_Predictions;
using Wpf.CartesianChart.Financial;
using Wpf.CartesianChart.Funnel_Chart;
using Wpf.CartesianChart.GanttChart;
using Wpf.CartesianChart.HeatChart;
using Wpf.CartesianChart.Inverted_Series;
using Wpf.CartesianChart.Irregular_Intervals;
using Wpf.CartesianChart.Labels;
using Wpf.CartesianChart.Linq;
using Wpf.CartesianChart.LogarithmScale;
using Wpf.CartesianChart.ManualZAndP;
using Wpf.CartesianChart.MaterialCards;
using Wpf.CartesianChart.Missing_Line_Points;
using Wpf.CartesianChart.MixingSeries;
using Wpf.CartesianChart.NegativeStackedRow;
using Wpf.CartesianChart.PointShapeLine;
using Wpf.CartesianChart.PointState;
using Wpf.CartesianChart.ScatterPlot;
using Wpf.CartesianChart.SectionsDragable;
using Wpf.CartesianChart.StackedArea;
using Wpf.CartesianChart.StackedBar;
using Wpf.CartesianChart.StepLine;
using Wpf.CartesianChart.UIElements;
using Wpf.Gauges;
using Wpf.Maps;
using Wpf.PieChart;
using BubblesExample = Wpf.CartesianChart.Bubbles.BubblesExample;
using ConstantChangesChart = Wpf.CartesianChart.ConstantChanges.ConstantChangesChart;
using DateTime = Wpf.CartesianChart.Using_DateTime.DateTime;
using FullyResponsiveExample = Wpf.CartesianChart.FullyResponsive.FullyResponsiveExample;
using LabelsExample = Wpf.CartesianChart.Labels.LabelsExample;
using SectionsExample = Wpf.CartesianChart.Sections.SectionsExample;
using StackedAreaExample = Wpf.CartesianChart.StackedArea.StackedAreaExample;

namespace Wpf
{
    public partial class MainWindow : INotifyPropertyChanged
    {
        private UserControl _cartesianView;
        private UserControl _pieView;
        private UserControl _gaugeView;

        public MainWindow()
        {
            InitializeComponent();

            #region Examples

            CartesianExamples = new List<UserControl>
            {
                //new JimmyTheTestsGuy(),
                new MaterialCards(),
                new EnergyPredictionExample(),
                new FunnelExample(),
                new ConstantChangesChart(),
                new FullyResponsiveExample(),
                new LinqExample(),
                new PointShapeLineExample(),
                new HeatSeriesExample(),
                new ManualZAndPExample(),
                new DynamicVisibilityExample(),
                new LabelsExample(),
                new LabelsHorizontalExample(),
                new BasicStackedRowPercentageExample(),
                new BasicStackedColumnExample(),
                new PointStateExample(),
                new BasicRowExample(),
                new GanttExample(),
                new MultiAxesChart(),
                new GeoMapExample(),
                new UiElementsExample(),
                new StackedRowExample(),
                new CustomTooltipAndLegendExample(),
                new BasicColumn(),
                new BasicLineExample(),
                new StackedAreaExample(),
                new CustomTypesPlotting(),
                new NegativeStackedRowExample(),
                new LineExample(),
                new StepLineExample(),
                new CustomizedLineSeries(),
                new InvertedExample(),
                new ScatterExample(),
                new BubblesExample(),
                new StackedAreaExample(),
                new OhclExample(),
                new CandleStickExample(),
                new StackedColumnExample(),
                new SectionsExample(),
                new DragableSections(),
                new MissingPointsExample(),
                new IrregularIntervalsExample(),
                new DateTime(),
                new LogarithmScaleExample(),
                new VerticalStackedAreaExample(),
                new ZoomingAndPanning(),
                new MixingTypes(),
                new InLineSyntaxTest()
            };

            PieExamples = new List<UserControl>
            {
                new PieChartExample(),
                new DoughnutChartExample()
            };

            GaugeExamples = new List<UserControl>
            {
                new Gauge180(),
                new Gauge360(),
                new AngularGaugeExmple()
            };

            #endregion

            Func<List<UserControl>, UserControl> getView = x => x != null && x.Count > 0 ? x[0] : null;

            CartesianView = getView(CartesianExamples);
            PieView = getView(PieExamples);
            GaugeView = getView(GaugeExamples);

            DataContext = this;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public UserControl CartesianView
        {
            get { return _cartesianView; }
            set
            {
                _cartesianView = value;
                OnPropertyChanged("CartesianView");
            }
        }
        public UserControl PieView
        {
            get { return _pieView; }
            set
            {
                _pieView = value;
                OnPropertyChanged("PieView");
            }
        }
        public UserControl GaugeView
        {
            get { return _gaugeView; }
            set
            {
                _gaugeView = value;
                OnPropertyChanged("GaugeView");
            }
        }

        public List<UserControl> CartesianExamples { get; set; }
        public List<UserControl> PieExamples { get; set; }
        public List<UserControl> GaugeExamples { get; set; }

        private void NextCartesianOnClick(object sender, MouseButtonEventArgs e)
        {
            if (CartesianView == null) return;
            var current = CartesianExamples.IndexOf(CartesianView);
            current++;
            CartesianView = CartesianExamples.Count > current ? CartesianExamples[current] : CartesianExamples[0];
        }

        private void PreviousCartesianOnClick(object sender, MouseButtonEventArgs e)
        {
            if (CartesianView == null) return;
            var current = CartesianExamples.IndexOf(CartesianView);
            current--;
            CartesianView = current >= 0 ? CartesianExamples[current] : CartesianExamples[CartesianExamples.Count-1];
        }

        private void NextPieOnClick(object sender, MouseButtonEventArgs e)
        {
            if (PieView == null) return;
            var current = PieExamples.IndexOf(CartesianView);
            current++;
            PieView = PieExamples.Count > current ? PieExamples[current] : PieExamples[0];
        }

        private void PreviousPieOnClick(object sender, MouseButtonEventArgs e)
        {
            if (PieView == null) return;
            var current = PieExamples.IndexOf(CartesianView);
            current--;
            PieView = current >= 0 ? PieExamples[current] : PieExamples[PieExamples.Count - 1];
        }

        private void NextGaugeOnClick(object sender, MouseButtonEventArgs e)
        {
            if (GaugeView == null) return;
            var current = GaugeExamples.IndexOf(GaugeView);
            current++;
            GaugeView = GaugeExamples.Count > current ? GaugeExamples[current] : GaugeExamples[0];
        }

        private void PreviousGaugeOnClick(object sender, MouseButtonEventArgs e)
        {
            if (GaugeView == null) return;
            var current = GaugeExamples.IndexOf(GaugeView);
            current--;
            GaugeView = current >= 0 ? GaugeExamples[current] : GaugeExamples[GaugeExamples.Count - 1];
        }


        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            if (PropertyChanged != null) PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
