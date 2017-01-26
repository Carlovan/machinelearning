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
using System.Windows.Shapes;

namespace MachineLearning
{
    /// <summary>
    /// Logica di interazione per WindowInsertKnn.xaml
    /// </summary>
    public partial class WindowInsertKnn : Window
    {
        List<TextBox> txtboxes = new List<TextBox>();

        readonly uint dimensionCount;
        public DataPoint Result { get; set; }

        public WindowInsertKnn(uint dimensions)
        {
            InitializeComponent();
            dimensionCount = dimensions;
            Result = new DataPoint((int)dimensionCount);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < dimensionCount; i++)
            {
                TextBox txtTmp = new TextBox();
                txtTmp.Width = 100;
                txtTmp.Height = 20;
                txtboxes.Add(txtTmp);

                Label lblTmp = new Label() { Content = $"D{i+1}", HorizontalAlignment = HorizontalAlignment.Left};

                StackPanel skpTmp = new StackPanel() { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Center };
                skpTmp.Children.Add(lblTmp);
                skpTmp.Children.Add(txtTmp);

                skpContainer.Children.Add(skpTmp);
            }
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            for(int i = 0; i < txtboxes.Count; i++)
            {
                int n;
                if(!int.TryParse(txtboxes[i].Text, out n))
                {
                    MessageBox.Show("Invalid values");
                    return;
                }
                Result.Dimensions[i] = n;
            }
            this.DialogResult = true;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
