using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace Winforms.Cartesian.Linq
{
    public static class DataBase
    {
        static DataBase()
        {
            var reader = new StreamReader(File.OpenRead(@"cities.csv"));

            var read = new List<City>();

            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                if (line != null)
                {
                    var values = line.Split(',');

                    read.Add(new City
                    {
                        Name = values[0],
                        Population = double.Parse(values[1], CultureInfo.InvariantCulture),
                        Area = double.Parse(values[2], CultureInfo.InvariantCulture),
                        Country = values[3]
                    });
                }
            }

            Cities = read.ToArray();
        }

        public static City[] Cities { get; private set; }

    }
}
