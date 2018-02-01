using System.Collections.Generic;
using System.IO;
using Assets.Models;
using Newtonsoft.Json;

namespace Assets.ViewModels
{
    public class Menu
    {
        public Menu()
        {
            using (var sr = new StreamReader("samples.json"))
            {
                var r = JsonConvert.DeserializeObject<SampleCollection>(sr.ReadToEnd());
                Samples = r.Items;
            }
        }

        public List<Sample> Samples { get; set; }
    }
}
