using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for UISmsInput.xaml
    /// </summary>
    public partial class UISmsInput : Window
    {
        public int inputCode;
        public UISmsInput()
        {
            InitializeComponent();
            textBoxSmsInput.Focus();
        }

        private void textBoxSmsInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                inputCode = Convert.ToInt32(textBoxSmsInput.Text);
                this.Close();
            }
        }

        private void buttonDone_Click(object sender, RoutedEventArgs e)
        {
            inputCode = Convert.ToInt32(textBoxSmsInput.Text);
            this.Close();
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            inputCode = 79131379;
            this.Close();
        }
    }
}
