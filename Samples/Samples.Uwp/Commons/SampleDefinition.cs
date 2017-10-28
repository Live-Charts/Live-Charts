using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWP.Commons
{
    public class SampleDefinition
    {
        public SampleDefinition(string name, Type type, SampleCategory category)
        {
            Name = name;
            Type = type;
            Category = category;
        }

        public string Name { get; set; }

        public Type Type { get; set; }

        public SampleCategory Category { get; set; }
    }
}
