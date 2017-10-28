using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using UWP.Commons;

namespace UWP
{
    public sealed partial class AppShell : Page
    {
        public AppShell()
        {
            this.InitializeComponent();
            SampleViewSource.Source = SampleDefinitions.Definitions.GroupBy(x => x.Category);
        }

        private void SampleList_ItemClick(Object sender, ItemClickEventArgs e)
        {
            var definition = e.ClickedItem as SampleDefinition;
            if (definition != null)
            {
                this.ContentFrame.Navigate(definition.Type);
                SamplesSplitView.IsPaneOpen = false;
            }
        }

        private void Button_Click(Object sender, RoutedEventArgs e)
        {
            SamplesSplitView.IsPaneOpen = !SamplesSplitView.IsPaneOpen;
        }
    }
}
