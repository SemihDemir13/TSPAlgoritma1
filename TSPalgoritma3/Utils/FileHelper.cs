using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSPalgoritma.Models;

namespace TSPalgoritma.Utils
{
    public static class FileHelper
    {
        public static List<City> LoadCitiesFromFile(string path)
        {
            var lines = File.ReadAllLines(path);
            int count = int.Parse(lines[0]);
            var cities = new List<City>();

            for (int i = 1; i <= count; i++)
            {
                var parts = lines[i].Split(' ', '\t');
                double x = double.Parse(parts[0], CultureInfo.InvariantCulture);
                double y = double.Parse(parts[1], CultureInfo.InvariantCulture);
                cities.Add(new City(i - 1, x, y));
            }

            return cities;
        }
    }
}
