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
    /// Interaction logic for UIUserNote.xaml
    /// </summary>
    public partial class UIUserNote : Window
    {
        private UserObject scannedUser = null;    

        public UIUserNote()
        {
            InitializeComponent();
        }
        public UIUserNote(UserObject currentUser)
        {
            InitializeComponent();
            scannedUser = currentUser;
            textBoxUserNote.Text = UserController.Instance.GetUserNote(scannedUser.UserMifare);
        }

        private bool firstRun = true;

        private void ButtonConfirmUserNote_Click(object sender, RoutedEventArgs e)
        {
            UserController.Instance.UpdateUserNote(textBoxUserNote.Text, scannedUser.UserMifare);
            if (textBoxUserNote.Text == "")
            {
                MessageBox.Show("Note Slettet");
            }
            else
            {
                MessageBox.Show("Note Gemt");
            }
            this.Close();
        }

        private void TextBoxUserNote_TextChanged(object sender, TextChangedEventArgs e)
        {
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
