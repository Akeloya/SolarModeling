using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SolarModeling
{
    class Point
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Mass { get; set; }
        public double Radius { get; set; }
        public double Vx { get; set; }
        public double Vy { get; set; }
        public Ellipse Ellipse { get; }
        public Point(double x, double y, double mass, double radius)
        {
            X = x;
            Y = y;
            Mass = mass;
            Radius = radius;
            Vx = 0;
            Vy = 0;
            Ellipse = new Ellipse();
            Ellipse.Width = 2;
            Ellipse.Height = 2;
            Ellipse.Fill = Brushes.Black;
        }
    }
}
