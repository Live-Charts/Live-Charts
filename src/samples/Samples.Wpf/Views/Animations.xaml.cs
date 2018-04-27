namespace Samples.Wpf.Views
{
    /// <summary>
    /// Interaction logic for Animations.xaml
    /// </summary>
    public partial class Animations
    {
        public Animations()
        {
            InitializeComponent();
            var dataContext = new Assets.ViewModels.Animations();
            dataContext.PropertyChanged += (sender, args) => { Chart.Update(true); };
            DataContext = dataContext;
        }
    }
}
