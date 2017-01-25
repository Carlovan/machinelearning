using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.IO;
using System.Xml;

namespace MachineLearning
{
    [DataContract(Name = "KMeans")]
    class KMeansHandler
    {
        public ObservableCollection<DataPoint> Points { get; set; } = new ObservableCollection<DataPoint>();

        [DataMember(Name = "Centroids")]
        public ObservableCollection<Centroid> Centroids { get; set; } = new ObservableCollection<Centroid>();

        public void SerializeToFile(string filename)
        {
            using (FileStream saveStream = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None))
            using (XmlWriter writer = XmlWriter.Create(saveStream, new XmlWriterSettings() { Indent = true }))
            {
                DataContractSerializer dcser = new DataContractSerializer(typeof(KMeansHandler));
                dcser.WriteObject(writer, this);
            }
        }

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

        public bool MoveCentroids()
        {
            bool somethingMoved = false;
            foreach (Centroid c in Centroids)
            {
                if (c.Points.Count > 0)
                {
                    for(int i = 0; i < c.Dimensions.Count; i++)
                    {
                        double tmp = c.Points.Average(p => p.Dimensions[i]);
                        if (Math.Abs(tmp - c.Dimensions[i]) >= 1)
                            somethingMoved = true;
                        c.Dimensions[i] = tmp;
                    }
                }
                else
                {
                    Random rand = new Random();
                    DataPoint rPoint = Points[rand.Next(Points.Count)];
                    c.Dimensions = new ObservableCollection<double>(rPoint.Dimensions);
                    somethingMoved = true;
                }
            }
            return somethingMoved;
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
