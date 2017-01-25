using System.Windows;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace MachineLearning
{
    [DataContract(Name = "DataPoint")]
    class DataPoint : UIPoint
    {
        public DataPoint() : base(2) { }

        public DataPoint(int dimensions) : base(dimensions) { }

        public DataPoint(UIPoint other) : base(other) { }

        public DataPoint(Point other) : base(other) {}

        public override string ToString()
        {
            return string.Format("DataPoint ({0})", string.Join(", ", Dimensions));
        }
    }
}
