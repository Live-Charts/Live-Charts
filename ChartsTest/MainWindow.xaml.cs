using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using LiveCharts.Annotations;

namespace ChartsTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public double[] TestPrimaryValues { get; set; }
        public ObservableCollection<string> TestProperty { get; set; }

        public MainWindow()
        {
            InitializeComponent();
	        ExamplesMapper.Initialize(this);

	        TestPrimaryValues = new[] {3d, 2, 4, 6};
            TestProperty = new ObservableCollection<string> { "bye!" };

            DataContext = this;

        }

        #region NavigationButtons
        private void LineNext(object sender, MouseButtonEventArgs e)
        {
            LineControl.Next(ExamplesMapper.LineAndAreaAexamples);
        }
        private void LinePrevious(object sender, MouseButtonEventArgs e)
        {
            LineControl.Previous(ExamplesMapper.LineAndAreaAexamples);
        }
        private void BarPrevious(object sender, MouseButtonEventArgs e)
        {
            BarControl.Previous(ExamplesMapper.BarExamples);
        }
        private void BarNext(object sender, MouseButtonEventArgs e)
        {
            BarControl.Next(ExamplesMapper.BarExamples);
        }
        private void StackedBarPrevious(object sender, MouseButtonEventArgs e)
        {
            StackedBarControl.Previous(ExamplesMapper.StackedBarExamples);
        }
        private void StackedBarNext(object sender, MouseButtonEventArgs e)
        {
            StackedBarControl.Next(ExamplesMapper.StackedBarExamples);
        }
        private void PiePrevious(object sender, MouseButtonEventArgs e)
        {
            PieControl.Previous(ExamplesMapper.PieExamples);
        }
        private void PieNext(object sender, MouseButtonEventArgs e)
        {
            PieControl.Next(ExamplesMapper.PieExamples);
        }
        private void ScatterPrevious(object sender, MouseButtonEventArgs e)
        {
            ScatterControl.Previous(ExamplesMapper.ScatterExamples);
        }
        private void ScatterNext(object sender, MouseButtonEventArgs e)
        {
            ScatterControl.Next(ExamplesMapper.ScatterExamples);
        }
        #endregion
      
    }
}
