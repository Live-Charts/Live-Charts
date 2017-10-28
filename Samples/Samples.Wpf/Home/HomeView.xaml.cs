using System;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using MahApps.Metro.Controls;

namespace Wpf.Home
{
    public partial class HomeView : MetroWindow
    {
        public HomeView()
        {
            InitializeComponent();
        }

        private void RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

        private void UIElement_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            var sample = (SampleVm)((Border)sender).DataContext;
            var hvm = (HomeViewModel) DataContext;
            hvm.Content = (UserControl) Activator.CreateInstance(sample.Content);
            hvm.IsMenuOpen = false;
        }
    }
}
