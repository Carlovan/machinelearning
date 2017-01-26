using System;
using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Data;
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
        Binding DETAILS_RUNNING = new Binding() { ElementName = "dtgCentroids", Path = new PropertyPath("SelectedItem.Points") };
        Binding DETAILS_INSERTING = new Binding("Points");

        KMeansHandler handler;
        DispatcherTimer timer = new DispatcherTimer();

        public WindowGraphic2D()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            handler = new KMeansHandler();
            this.DataContext = handler;

            timer.Interval = new TimeSpan((long)sldAnimSpeed.Value);
            timer.Tick += Timer_Tick;
            sldAnimSpeed.ValueChanged += sldAnimSpeed_ValueChanged;
        }

        private void sldAnimSpeed_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            timer.Interval = new TimeSpan((long)e.NewValue);
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

        private void btnRun_Click(object sender, RoutedEventArgs e)
        {
            if (timer.IsEnabled)
            {
                StopAnimation();
            }
            else if(handler.Points.Count > 0)
            {
                StartAnimation();
            }
            else
            {
                MessageBox.Show("You must add more points", "ERROR");
            }
        }

        private void StartAnimation()
        {
            EnableButtons(false);
            dtgPoints.SetBinding(DataGrid.ItemsSourceProperty, DETAILS_RUNNING);
            handler.InitializeCentroids((uint)GetCentroidsCount());
            dtgCentroids.Items.Refresh();
            btnStart.Content = "Stop";
            handler.DrawOnCanvas(cnvDrawArea, true);
            timer.Start();
        }

        private void StopAnimation()
        {
            btnStart.Content = "Run";
            timer.Stop();
            EnableButtons(true);
        }

        private void Timer_Tick(object sender, System.EventArgs e)
        {
            if (!ExecuteStep())
                StopAnimation();
        }

        private void EnableButtons(bool enabled)
        {
            btnClear.IsEnabled = enabled;
            btnClearCentroids.IsEnabled = enabled;
            sldNumCentroids.IsEnabled = enabled;
        }

        private bool ExecuteStep()
        {
            bool val;
            handler.AssignPoints();
            val = handler.MoveCentroids();
            handler.DrawOnCanvas(cnvDrawArea, true);
            return val;
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            handler.Points.Clear();
            handler.Centroids.Clear();
            SetInserting();
            handler.DrawOnCanvas(cnvDrawArea, false);
        }

        private int GetCentroidsCount()
        {
            return (int)sldNumCentroids.Value;
        }

        private void btnClearCentroids_Click(object sender, RoutedEventArgs e)
        {
            handler.Centroids.Clear();
            SetInserting();
            handler.DrawOnCanvas(cnvDrawArea, false);
        }

        private void dtgPoints_AddingNewItem(object sender, AddingNewItemEventArgs e)
        {
            e.NewItem = new DataPoint();
        }

        private void SetInserting()
        {
            dtgPoints.SetBinding(DataGrid.ItemsSourceProperty, DETAILS_INSERTING);
        }

        private void dtgPoints_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            handler.DrawOnCanvas(cnvDrawArea, false);
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "File XML|*.xml|Tutti i file|*.*";
            sfd.DefaultExt = ".xml";
            if ((bool)sfd.ShowDialog())
            {
                Utils.SerializeToFile(sfd.FileName, handler);
            }
        }

        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "File XML|*.xml|Tutti i file|*.*";
            ofd.DefaultExt = ".xml";
            if ((bool)ofd.ShowDialog())
            {
                handler = Utils.DeserializeFromFile<KMeansHandler>(ofd.FileName);
                this.DataContext = handler;
                dtgPoints.SetBinding(DataGrid.ItemsSourceProperty, DETAILS_RUNNING);
                handler.DrawOnCanvas(cnvDrawArea, true);
            }
        }
    }
}
