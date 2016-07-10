using System.Windows.Forms;

namespace Winforms.Gauge._360
{
    public partial class Gauge360Example : Form
    {
        public Gauge360Example()
        {
            InitializeComponent();

            gauge1.From = 0;
            gauge1.To = 1;
            gauge1.Value = .65;
            gauge1.Uses360Mode = true;
            gauge1.LabelFormatter = value => value.ToString("P");
        }
    }
}
