using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSPalgoritma.Models
{
    public class City
    {
        public int Id { get; set; }
        public double X { get; set; }
        public double Y { get; set; }

        public City(int id, double x, double y)
        {
            Id = id;
            X = x;
            Y = y;
        }

        public double DistanceTo(City other)
        {
            double dx = X - other.X;
            double dy = Y - other.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }
    }
}
