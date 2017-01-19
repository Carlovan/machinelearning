using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MachineLearning
{
    static class Utils
    {
        static public T MinItem<T, U>(this IEnumerable<T> ie, Func<T, U> key) where U : IComparable
        {
            T minVal = default(T);
            U minKey = default(U);
            foreach (T v in ie)
            {
                if (minKey.CompareTo(default(U)) == 0 || key(v).CompareTo(minKey) < 0)
                {
                    minKey = key(v);
                    minVal = v;
                }
            }
            return minVal;
        }

        static public void DrawPoint(this Canvas canvas, DataPoint pos)
        {
            Ellipse ell = new Ellipse() { Width = 10, Height = 10 };
            ell.Fill = new SolidColorBrush(new Color() { R = 44, G = 44, B = 44, A = 255 });
            Canvas.SetLeft(ell, pos.Dimensions[0] - ell.Width / 2);
            Canvas.SetTop(ell, pos.Dimensions[1] - ell.Height / 2);
            canvas.Children.Add(ell);
        }

        static public void DrawPoint(this Canvas canvas, Centroid pos)
        {
            Rectangle rect = new Rectangle() { Width = 10, Height = 10 };
            rect.Fill = new SolidColorBrush(new Color() { R = 100, G = 44, B = 44, A = 255 });
            Canvas.SetLeft(rect, pos.Dimensions[0] - rect.Width / 2);
            Canvas.SetTop(rect, pos.Dimensions[1] - rect.Height / 2);
            canvas.Children.Add(rect);
        }
    }
}
