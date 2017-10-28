using System.Collections.Generic;

namespace Wpf.Home
{
    public class SampleGroupVm
    {
        public string Name { get; set; }
        public IEnumerable<SampleVm> Items { get; set; }
    }
}
