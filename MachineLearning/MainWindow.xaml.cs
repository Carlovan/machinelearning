using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MachineLearning
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnStart2D_Click(object sender, RoutedEventArgs e)
        {
            var tmp = new WindowGraphic2D();
            tmp.ShowActivated = true;
            this.Close();
            tmp.Show();
        }

        private void btnStartKmeansMulti_Click(object sender, RoutedEventArgs e)
        {
            uint countD;
            if (!uint.TryParse(txtKmeansDimensions.Text, out countD))
            {
                MessageBox.Show("Invalid dimensions value");
                return;
            }
            var tmp = new WindowKmeansMulti(countD);
            tmp.ShowActivated = true;
            this.Close();
            tmp.Show();
        }

        private void btnStartKnnMulti_Click(object sender, RoutedEventArgs e)
        {
            uint countD;
            if(!uint.TryParse(txtKnnDimensions.Text, out countD))
            {
                MessageBox.Show("Invalid dimensions value");
                return;
            }
            var tmp = new WindowKnnMulti(countD);
            tmp.ShowActivated = true;
            this.Close();
            tmp.Show();
        }
    }
}
