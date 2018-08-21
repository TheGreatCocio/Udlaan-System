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
using UdlaanSystem.Managers;

namespace UdlaanSystem
{
    /// <summary>
    /// Interaction logic for UIShowID.xaml
    /// </summary>
    public partial class UIShowID : Window
    {
        public UIShowID(int ID)
        {
            InitializeComponent();

            LabelIDLarge.Content = ID;
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnOK_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.Close();
            }
        }
    }
}
