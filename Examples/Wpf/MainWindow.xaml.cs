using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using Wpf.Annotations;
using Wpf.CartesianChart;
using Wpf.CartesianChart.BasicLine;
using Wpf.CartesianChart.Basic_Bars;
using Wpf.CartesianChart.Basic_Stacked_Bar;
using Wpf.CartesianChart.Customized_Line_Series;
using Wpf.CartesianChart.CustomTooltipAndLegend;
using Wpf.CartesianChart.DynamicVisibility;
using Wpf.CartesianChart.HeatChart;
using Wpf.CartesianChart.Inverted_Series;
using Wpf.CartesianChart.Irregular_Intervals;
using Wpf.CartesianChart.Labels;
using Wpf.CartesianChart.Linq;
using Wpf.CartesianChart.LogarithmScale;
using Wpf.CartesianChart.ManualZAndP;
using Wpf.CartesianChart.Missing_Line_Points;
using Wpf.CartesianChart.NegativeStackedRow;
using Wpf.CartesianChart.PointShapeLine;
using Wpf.CartesianChart.PointState;
using Wpf.CartesianChart.SharedTooltip;
using Wpf.CartesianChart.StackedArea;
using Wpf.CartesianChart.StackedBar;
using Wpf.CartesianChart.UIElements;
using Wpf.Gauges;
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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
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
                new LinqExample(),
                new SharedTooltipExample(),
                new PointStateExample(),

                new CustomTooltipAndLegendExample(),
                new PointShapeLineExample(),
                new HeatSeriesExample(),
                new ManualZAndPExample(),
                new DynamicVisibilityExample(),
                new LabelsExample(),
                new LabelsHorizontalExample(),
                new BasicStackedRowPercentageExample(),
                new BasicStackedColumnExample(),
                new BasicRowExample(),
                new MultiAxesChart(),
                //new JimmyTheTestsGuy(),
                //new Issue179(),
                new UIElementsExample(),
                new StackedRowExample(),
                new BasicColumn(),
                new BasicLineExample(),
                new StackedAreaExample(),
                //new WelcomeCartesian(),
                new FullyResponsiveExample(),
                new CustomTypesPlotting(),
                new NegativeStackedRowExample(),
                new LineExample(),
                new ConstantChangesChart(),
                new CustomizedLineSeries(),
                new InvertedExample(),
                new BubblesExample(),
                new StackedAreaExample(),
                new FinancialExample(),
                new StackedColumnExample(),
                new SectionsExample(),
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
                new Gauge360()
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
