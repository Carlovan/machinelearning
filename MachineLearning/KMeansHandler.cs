using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System;

namespace MachineLearning
{
    class KMeansHandler
    {
        public List<DataPoint> Points { get; set; } = new List<DataPoint>();
        public List<Centroid> Centroids { get; set; } = new List<Centroid>();

        public void InitializeCentroids(uint count)
        {
            Random rand = new Random();
            Centroids.Clear();
            for (uint i = 0; i < count; i++)
            {
                UIPoint tmp = Points[rand.Next(Points.Count)];
                Centroids.Add(new Centroid(tmp));
            }
        }

        public void AssignPoints()
        {
            // Clear centroids
            foreach (Centroid centroid in Centroids)
            {
                centroid.Points.Clear();
            }

            // Assign every point to a centroid
            foreach (DataPoint p in Points)
            {
                Centroids.MinItem((c) => c.Distance(p)).Points.Add(p);
            }
        }

        public void MoveCentroids()
        {
            foreach (Centroid c in Centroids)
            {
                if (c.Points.Count > 0)
                {
                    for(int i = 0; i < c.Dimensions.Count; i++)
                    {
                        c.Dimensions[i] = c.Points.Average(p => p.Dimensions[i]);
                    }
                }
            }
        }

        public void DrawOnCanvas(Canvas canvas, bool running)
        {
            canvas.Children.Clear();
            foreach (DataPoint point in Points)
            {
                canvas.DrawPoint(point);
            }
            if (running)
            {
                foreach (Centroid centroid in Centroids)
                {
                    canvas.DrawPoint(centroid);
                    foreach(DataPoint p in centroid.Points)
                    {
                        Line l = new Line() { X1 = p.Dimensions[0], Y1 = p.Dimensions[1], X2 = centroid.Dimensions[0], Y2 = centroid.Dimensions[1] };
                        l.Stroke = new SolidColorBrush(new Color() { A = 255, R = 0, G = 0, B = 0 });
                        l.StrokeThickness = 1;
                        canvas.Children.Add(l);
                    }
                }
            }
        }
    }
}
