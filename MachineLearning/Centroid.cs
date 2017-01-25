using System.Collections.Generic;
using System.Windows;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace MachineLearning
{
    [DataContract(Name = "Centroid")]
    class Centroid : UIPoint
    {
        public Centroid(int dimensions = 2) : base(dimensions) { }

        public Centroid(UIPoint other) : base(other) { }

        public Centroid(Point other) : base(other) { }

        [DataMember(Name = "Points")]
        public List<DataPoint> Points { get; set; } = new List<DataPoint>();
    }
}
