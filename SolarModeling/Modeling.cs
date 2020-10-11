using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace SolarModeling
{
    class Modeling
    {
        private readonly double G = 0.000000000667;
        private double _modelRadius;
        private List<Point> _points = new List<Point>();
        public double MassCoeff { get; set; }
        private bool _terminate = false;
        private Canvas _canv;
        private double dt;
        public void SetTimeScale(double newDt)
        {
            dt = newDt;
        }
        public void Start(int number,double massCoeff, double modelRadius,Canvas canv)
        {
            _points.Clear();
            MassCoeff = massCoeff;
            _modelRadius = modelRadius;
            var random = new Random();
            for(var i = 0; i < number; i++)
            {
                var randRadius = random.NextDouble() * _modelRadius;
                var randAngle = random.NextDouble() * Math.PI * 2;
                var randMass = random.NextDouble() * MassCoeff;
                var randRadiusObj = random.NextDouble();
                var p = new Point(randRadius * Math.Cos(randAngle), randRadius * Math.Sin(randAngle),randMass, randRadiusObj);
                _points.Add(p);
            }
            _canv = canv;
        }

        public void Work()
        {
            _terminate = false;
            DispatcherOperation operation = null;
            var lastDrawedTime = DateTime.Now;
            _canv.Dispatcher.Invoke((Action)(() =>
            {
                _canv.Children.Clear();
                for (var i = 0; i < _points.Count; i++)
                {
                    _canv.Children.Add(_points[i].Ellipse);
                }
            }));
            while (!_terminate)
            {
                var viewportHeight = _canv.ActualHeight;
                var viewportWidth = _canv.ActualWidth;
                var centerX = viewportWidth / 2;
                var centerY = viewportHeight / 2;

                var scaleX = viewportWidth / _modelRadius / 2;
                var scaleY = viewportHeight / _modelRadius / 2;
                var viewportScale = Math.Min(scaleX, scaleY);
                for (var k = 0; k < _points.Count; k++)
                {
                    var point1 = _points[k];
                    var Fx = 0.0;
                    var Fy = 0.0;
                    for (var i = 0; i < _points.Count; i++)
                    {
                        if (k == i)
                            continue;
                        //считаем для i и i+1
                        var point2 = _points[i];
                        var r = Math.Sqrt(Math.Pow(point1.X - point2.X, 2) + Math.Pow(point1.Y - point2.Y, 2));
                        //сила, с которой точка 1 тянется к точке 2
                        var F = G * (point1.Mass * point2.Mass) / Math.Pow(r, 2);
                        //коэффициенты 1/-1 по осям для разложения вектора силы
                        var imgX = (point2.X - point1.X) / Math.Abs(point1.X - point2.X);
                        var imgY = (point2.Y - point1.Y) / Math.Abs(point1.Y - point2.Y);
                        //проекции вектора силы на оси X (acos) и Y (asin)
                        Fx += imgX * F * Math.Abs(point1.X - point2.X) / r;
                        Fy += imgY * F * Math.Abs(point1.Y - point2.Y) / r;
                    }
                    var aX = Fx;// / point1.Mass;
                    var aY = Fy;// / point1.Mass;
                    point1.X += aX * Math.Pow(dt, 2) / 2;
                    point1.Y += aY * Math.Pow(dt, 2) / 2;
                    _canv.Dispatcher.Invoke((Action)(() => {
                        Canvas.SetLeft(point1.Ellipse, centerX + point1.X * viewportScale);
                        Canvas.SetTop(point1.Ellipse, centerY + point1.Y * viewportScale);
                    }));
                }
                /*
                if (operation == null || operation?.Status == DispatcherOperationStatus.Completed)
                {
                    if ((DateTime.Now - lastDrawedTime).TotalSeconds > 1)
                    {
                        operation = _canv.Dispatcher.InvokeAsync((Action)(() =>
                        {
                            var viewportHeight = _canv.ActualHeight;
                            var viewportWidth = _canv.ActualWidth;
                            var centerX = viewportWidth / 2;
                            var centerY = viewportHeight / 2;

                            var scaleX = viewportWidth / _modelRadius/2;
                            var scaleY = viewportHeight / _modelRadius/2;
                            var viewportScale = Math.Min(scaleX, scaleY);
                            for (var i = 0; i < _points.Count; i++)
                            {
                                var point = _points[i];

                                
                                Canvas.SetLeft(point.Ellipse, centerX + point.X* viewportScale);
                                Canvas.SetTop(point.Ellipse, centerY + point.Y* viewportScale);
                            }
                            lastDrawedTime = DateTime.Now;
                        }));
                        
                    }
                }*/
            }
        }

        public void Terminate()
        {
            _terminate = true;
        }
    }
}
