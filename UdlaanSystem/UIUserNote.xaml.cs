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

namespace UdlaanSystem
{
    /// <summary>
    /// Interaction logic for UIUserNote.xaml
    /// </summary>
    public partial class UIUserNote : Window
    {
        public UIUserNote()
        {
            InitializeComponent();
        }

        private void ButtonConfirmUserNote_Click(object sender, RoutedEventArgs e)
        {

        }

        private void TextBoxUserNote_TextChanged(object sender, TextChangedEventArgs e)
        {
            bool firstRun = true;
            label1.Content = textBoxUserNote.Text.Length + " / 400 tegn";

            if (textBoxUserNote.Text.Length > 400)
            {
                if (firstRun == true)
                {
                    MessageBox.Show("Du kan max gemme 400 tegn!");
                    firstRun = false;
                }
                buttonConfirmUserNote.IsEnabled = false;
            }
            else
            {
                buttonConfirmUserNote.IsEnabled = true;
            }
        }
    }
}
