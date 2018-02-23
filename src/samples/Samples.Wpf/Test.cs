using System.Windows.Controls;
using System.Windows.Media;

namespace Samples.Wpf
{
    class Test : ContentPresenter
    {
        public Test()
        {
            Content = new Canvas
            {
                Background = Brushes.Red
            };
        }
    }
}
