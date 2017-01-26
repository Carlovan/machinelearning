using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace MachineLearning
{
    class KnnGroup
    {
		public string Tag { get; set; }
        public ObservableCollection<DataPoint> DataSet { get; set; } = new ObservableCollection<DataPoint>();
        public ObservableCollection<DataPoint> Points { get; set; } = new ObservableCollection<DataPoint>();
    }
}
