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
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MachineLearning
{
    /// <summary>
    /// Interaction logic for WindowKmeansMulti.xaml
    /// </summary>
    public partial class WindowKmeansMulti : Window
    {
        Binding DETAILS_RUNNING = new Binding() { ElementName = "dtgCentroids", Path = new PropertyPath("SelectedItem.Points") };
        Binding DETAILS_INSERTING = new Binding("Points");

        KMeansHandler handler;

        readonly uint dimensionsCount;

        public WindowKmeansMulti(uint dimensions)
        {
            InitializeComponent();
            dimensionsCount = dimensions;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            handler = new KMeansHandler();
            this.DataContext = handler;

            for (int i = 0; i < dimensionsCount; i++)
            {
                var col1 = new DataGridTextColumn() { Header = $"D{i + 1}", Binding = new Binding($"Dimensions[{i}]"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) };
                var col2 = new DataGridTextColumn() { Header = $"D{i + 1}", Binding = new Binding($"Dimensions[{i}]"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) };
                dtgCentroids.Columns.Add(col1);
                dtgPoints.Columns.Add(col2);
            }
        }

        private void dtgPoints_InitializingNewItem(object sender, AddingNewItemEventArgs e)
        {
            e.NewItem = new DataPoint((int)dimensionsCount);
        }

        private void btnRun_Click(object sender, RoutedEventArgs e)
        {
            if (handler.Points.Count > 0)
            {
                StartAlgorithm();
                while (!ExecuteStep()) ;
                StopAlgorithm();
            }
            else
            {
                MessageBox.Show("You must add more points", "ERROR");
            }
        }

        private void StartAlgorithm()
        {
            EnableButtons(false);
            dtgPoints.SetBinding(DataGrid.ItemsSourceProperty, DETAILS_RUNNING);
            handler.InitializeCentroids((uint)GetCentroidsCount());
            dtgCentroids.Items.Refresh();
            dtgPoints.CanUserAddRows = false;
        }

        private void StopAlgorithm()
        {
            EnableButtons(true);
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
            return val;
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            handler.Points.Clear();
            handler.Centroids.Clear();
            SetInserting();

        }

        private int GetCentroidsCount()
        {
            return (int)sldNumCentroids.Value;
        }

        private void btnClearCentroids_Click(object sender, RoutedEventArgs e)
        {
            handler.Centroids.Clear();
            SetInserting();
        }

        private void SetInserting()
        {
            dtgPoints.CanUserAddRows = true;
            dtgPoints.SetBinding(DataGrid.ItemsSourceProperty, DETAILS_INSERTING);
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "File XML|*.xml|Tutti i file|*.*";
            sfd.DefaultExt = ".xml";
            if ((bool)sfd.ShowDialog())
            {
                Utils.SerializeKmeans(sfd.FileName, handler);
            }
        }

        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "File XML|*.xml|Tutti i file|*.*";
            ofd.DefaultExt = ".xml";
            if ((bool)ofd.ShowDialog())
            {
                handler = Utils.DeserializeKmeans(ofd.FileName);
                this.DataContext = handler;
                dtgPoints.CanUserAddRows = false;
                dtgPoints.SetBinding(DataGrid.ItemsSourceProperty, DETAILS_RUNNING);
            }
        }
    }
}

