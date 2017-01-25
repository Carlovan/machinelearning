using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Collections.ObjectModel;

namespace MachineLearning
{
    class KnnHelper
    {
        public DataPoint Point { get; set; }
        public Centroid Group { get; set; }
    }

    class KnnHandler
    {
        public ObservableCollection<Centroid> Centroids { get; set; } = new ObservableCollection<Centroid>();
        readonly int K;

        public KnnHandler(int _k)
        {
            K = _k;
        }

        public Centroid InsertPoint(DataPoint point)
        {
            var tmp = new List<KnnHelper>();
            foreach (Centroid c in Centroids)
            {
                tmp = tmp.Concat(c.Points.Select<DataPoint, KnnHelper>((p) => { return new KnnHelper() { Point = p, Group = c }; })).ToList();
            }
            tmp.Sort((x, y) => x.Point.Distance(point).CompareTo(y.Point.Distance(point)));

            tmp = tmp.TakeWhile((x, i) => i < K).ToList();
            Centroid target = tmp.GroupBy(x => x.Group).OrderBy(x => x.Count()).Last().Key;
            target.Points.Add(point);
            return target;
        }
    }
}
