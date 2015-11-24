using System.Windows.Input;

namespace ChartsTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
	    public MainWindow()
        {
            InitializeComponent();
	        ExamplesMapper.Initialize(this);
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
