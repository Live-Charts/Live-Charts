using System.Windows.Forms;

namespace Winforms.Gauge._180
{
    public partial class Gauge180Example : Form
    {
        public Gauge180Example()
        {
            InitializeComponent();

            gauge1.From = 0;
            gauge1.To = 100;
            gauge1.Value = 65;
            gauge1.LabelFormatter = value => value + " Km/Hr";

        }
    }
}
