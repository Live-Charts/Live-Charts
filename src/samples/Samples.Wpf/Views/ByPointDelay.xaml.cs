using System.Windows.Controls;

namespace Samples.Wpf.Views
{
    /// <summary>
    /// Interaction logic for ByPointDelay.xaml
    /// </summary>
    public partial class ByPointDelay : UserControl
    {
        public ByPointDelay()
        {
            InitializeComponent();

            var context = new Assets.ViewModels.ByPointDelay();
            context.PropertyChanged += (sender, args) => { Chart.ForceUpdate(true); };
            DataContext = context;
        }
    }
}
