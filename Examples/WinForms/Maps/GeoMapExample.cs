using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Winforms.Maps
{
    public partial class GeoMapExample : Form
    {
        public GeoMapExample()
        {
            InitializeComponent();

            var r = new Random();

            var values = new Dictionary<string, double>();

            values["MX"] = r.Next(0, 100);
            values["CA"] = r.Next(0, 100);
            values["US"] = r.Next(0, 100);
            values["IN"] = r.Next(0, 100);
            values["CN"] = r.Next(0, 100);
            values["JP"] = r.Next(0, 100);
            values["BR"] = r.Next(0, 100);
            values["DE"] = r.Next(0, 100);
            values["FR"] = r.Next(0, 100);
            values["GB"] = r.Next(0, 100);

            var lang = new Dictionary<string, string>();
            lang["MX"] = "México"; // change the language if necessary

            geoMap1.HeatMap = values;
            geoMap1.LanguagePack = lang;
            geoMap1.Source = "Maps/World.xml";
        }
    }
}
