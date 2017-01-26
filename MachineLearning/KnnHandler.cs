using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Runtime.CompilerServices;

namespace MachineLearning
{
    class KnnHelper
    {
        public DataPoint Point { get; set; }
        public KnnGroup Group { get; set; }
    }

    [DataContract(Name = "KnnHandler")]
    class KnnHandler
    {
        [DataMember(Name = "Groups")]
        public ObservableCollection<KnnGroup> Groups { get; set; } = new ObservableCollection<KnnGroup>();

        public KnnGroup InsertPoint(DataPoint point, uint K)
        {
            var tmp = new List<KnnHelper>();
            foreach (KnnGroup c in Groups)
            {
                tmp = tmp.Concat(c.DataSet.Select<DataPoint, KnnHelper>((p) => { return new KnnHelper() { Point = p, Group = c }; })).ToList();
            }
            tmp.Sort((x, y) => x.Point.Distance(point).CompareTo(y.Point.Distance(point)));

            tmp = tmp.TakeWhile((x, i) => i < K).ToList();
            KnnGroup target = tmp.GroupBy(x => x.Group).OrderBy(x => x.Count()).Last().Key;
            target.Points.Add(point);
            return target;
        }
    }
}
