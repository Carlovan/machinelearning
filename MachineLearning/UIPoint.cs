using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using System.Windows;

namespace MachineLearning
{
    class UIPoint
    {
        public ObservableCollection<double> Dimensions { get; set; }

        public UIPoint(int dimensions=2)
        {
            Dimensions = new ObservableCollection<double>();
            for(int i = 0; i < dimensions; i++)
            {
                Dimensions.Add(0);
            }
        }

        public UIPoint(UIPoint other)
        {
            Dimensions = new ObservableCollection<double>(other.Dimensions);
        }

        public UIPoint(Point other)
        {
            Dimensions = new ObservableCollection<double>(new List<double>() { other.X, other.Y });
        }

        public double Distance(UIPoint other)
        {
            if(other.Dimensions.Count != Dimensions.Count)
            {
                throw new ArgumentException("You cannot calculate distance between points with a different number of dimensions!");
            }
            return Math.Sqrt(Enumerable.Zip(other.Dimensions, Dimensions, (x1, x2) => Math.Pow(x1 - x2, 2)).Sum());
        }
    }
}
