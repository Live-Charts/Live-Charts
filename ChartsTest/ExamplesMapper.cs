using System.Collections.Generic;
using System.Windows.Controls;
using ChartsTest.Line_Examples;

namespace ChartsTest
{
    //IMPORTANT
    //This class is only to display examples in MainWindow
    //If you want to see the code behind and XAML of an example you should go to that path
    //for example in solution exmplorer go to "Line Examples" folder and open the example you need.
    public static class ExamplesMapper
    {
        public static void Initialize(MainWindow window)
        {
            LineAndAreaAexamples = new List<UserControl>
            {
                new BasicLine(),
                new CustomLine()
            };
            window.LineControl.Content = LineAndAreaAexamples[0];
        }
        public static List<UserControl> LineAndAreaAexamples { get; set; }

        public static void Next(this ContentControl control, List<UserControl> list)
        {
            var c = control.Content as UserControl;
            if (c == null) return;
            var i = list.IndexOf(c);
            i++;
            if (i > list.Count - 1) i = 0;
            control.Content = list[i];
        }

        public static void Previous(this ContentControl control, List<UserControl> list)
        {
            var c = control.Content as UserControl;
            if (c == null) return;
            var i = list.IndexOf(c);
            i--;
            if (i < 0) i = list.Count-1;
            control.Content = list[i];
        }
    }
}
