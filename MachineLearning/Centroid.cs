using System.Collections.Generic;
using System.Windows;

namespace MachineLearning
{
    class Centroid : UIPoint
    {
        public Centroid(int dimensions = 2) : base(dimensions) { }

        public Centroid(UIPoint other) : base(other) { }

        public Centroid(Point other) : base(other) { }

        public List<DataPoint> Points { get; set; } = new List<DataPoint>();
    }
}
