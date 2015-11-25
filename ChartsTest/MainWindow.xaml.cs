using System.Windows.Input;

namespace ChartsTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public double[] TestPrimaryValues { get; set; }
	    public MainWindow()
        {
            InitializeComponent();
	        ExamplesMapper.Initialize(this);

	        TestPrimaryValues = new[] {3d, 2, 4, 6};

	        DataContext = TestPrimaryValues;
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
        #endregion
    }
}
