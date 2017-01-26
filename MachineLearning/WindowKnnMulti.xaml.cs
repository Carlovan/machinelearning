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
using System.Linq;

namespace MachineLearning
{
    /// <summary>
    /// Interaction logic for WindowKnnMulti.xaml
    /// </summary>
    public partial class WindowKnnMulti : Window
    {
        KnnHandler handler;

        readonly uint dimensionsCount;

        public WindowKnnMulti(uint dimensions)
        {
            InitializeComponent();
            dimensionsCount = dimensions;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            handler = new KnnHandler(5);
            this.DataContext = handler;

            for (int i = 0; i < dimensionsCount; i++)
            {
                var col1 = new DataGridTextColumn() { Header = $"D{i + 1}", Binding = new Binding($"Dimensions[{i}]"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) };
                var col2 = new DataGridTextColumn() { Header = $"D{i + 1}", Binding = new Binding($"Dimensions[{i}]"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) };
                dtgDataset.Columns.Add(col1);
                dtgPoints.Columns.Add(col2);
            }
        }

        private void dtgPoints_InitializingNewItem(object sender, AddingNewItemEventArgs e)
        {
            e.NewItem = new DataPoint((int)dimensionsCount);
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            handler.Groups.Clear();
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
                handler = Utils.DeserializeFromFile<KnnHandler>(ofd.FileName);
                this.DataContext = handler;
                dtgPoints.CanUserAddRows = false;
            }
        }

        private void btnInsert_Click(object sender, RoutedEventArgs e)
        {
            if (handler.Groups.Select(x => x.DataSet.Count()).Sum() == 0)
            {
                MessageBox.Show("You must insert some points first!");
            }
            else
            {
                var tmp = new WindowInsertKnn(dimensionsCount);
                if ((bool)tmp.ShowDialog())
                {
                    dtgCentroids.SelectedItem = handler.InsertPoint(tmp.Result);
                    dtgPoints.SelectedItem = tmp.Result;
                    dtgPoints.Items.Refresh();
                }
            }
        }
    }
}
