using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.ComponentModel;
using System.Collections.Specialized;

namespace MachineLearning
{
    /// <summary>
    /// Interaction logic for WindowGraphic2D.xaml
    /// </summary>
    public partial class WindowGraphic2D : Window
    {
        const int STEP_DELAY = 2000;

        KMeansHandler handler;
        DispatcherTimer timer = new DispatcherTimer();

        public WindowGraphic2D()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            handler = new KMeansHandler();
            dtgCentroids.DataContext = handler;
        }

        private void cnvDrawArea_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!timer.IsEnabled)
            {
                Point pos = e.GetPosition((Canvas)sender);
                DataPoint tmp = new DataPoint(pos);
                tmp.Dimensions.CollectionChanged += (object s, NotifyCollectionChangedEventArgs events) => { handler.DrawOnCanvas(cnvDrawArea, false); };
                handler.Points.Add(tmp);
                handler.DrawOnCanvas((Canvas)sender, false);
                dtgCentroids.Items.Refresh();
            }
        }

        private void cnvDrawArea_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            /*if (handler != null)
            {
                foreach (DataPoint point in handler.Points)
                {
                    point.X = point.X * e.NewSize.Width / e.PreviousSize.Width;
                    point.Y = point.Y * e.NewSize.Height / e.PreviousSize.Height;
                }
                foreach (Centroid point in handler.Centroids)
                {
                    point.X = point.X * e.NewSize.Width / e.PreviousSize.Width;
                    point.Y = point.Y * e.NewSize.Height / e.PreviousSize.Height;
                }
                handler.DrawOnCanvas(cnvDrawArea, timer.IsEnabled);
            }*/
        }

        private void btnRun_Click(object sender, RoutedEventArgs e)
        {
            if (timer.IsEnabled)
            {
                ((Button)sender).Content = "Run";
                timer.Stop();
                EnableButtons(true);
            }
            else
            {
                EnableButtons(false);
                handler.InitializeCentroids(5);
                dtgCentroids.Items.Refresh();
                ((Button)sender).Content = "Stop";
                handler.DrawOnCanvas(cnvDrawArea, true);
                timer.Tick += Timer_Tick;
                timer.Interval = new System.TimeSpan(STEP_DELAY * 10000);
                timer.Start();
            }
        }

        private void Timer_Tick(object sender, System.EventArgs e)
        {
            ExecuteStep();
        }

        private void EnableButtons(bool enabled)
        {
            //btnClear.IsEnabled = enabled;
        }

        private void ExecuteStep()
        {
            handler.AssignPoints();
            handler.MoveCentroids();
            handler.DrawOnCanvas(cnvDrawArea, true);
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            handler.Points.Clear();
            handler.Centroids.Clear();
            handler.DrawOnCanvas(cnvDrawArea, false);
        }

        private void btnStep_Click(object sender, RoutedEventArgs e)
        {
            ExecuteStep();
        }
    }
}
