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
    /// Interaction logic for UIInputUser.xaml
    /// </summary>

    public partial class UIInputUser : Window
    {
        public UIInputUser()
        {
            InitializeComponent();
        }

        public void TextBoxZbcName_TextChanged(object sender, TextChangedEventArgs e)
        {
            string[] userInfoFromAD = UserController.Instance.CheckIfUserExist(textBoxZbcName.Text);
            LabelFNameResult.Content = userInfoFromAD[0];
            LabelLNameResult.Content = userInfoFromAD[1];

            if (LabelFNameResult.Content.ToString() == "" || textBoxUserMifare.Text == "" || textBoxPhoneNumber.Text == "")
            {
                ButtonCreateOrUpdate.IsEnabled = false;
            }
            else
            {
                ButtonCreateOrUpdate.IsEnabled = true;
            }
        }

        public void TextBoxUserMifare_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (LabelFNameResult.Content.ToString() == "" || textBoxUserMifare.Text == "" || textBoxPhoneNumber.Text == "")
            {
                ButtonCreateOrUpdate.IsEnabled = false;
            }
            else
            {
                ButtonCreateOrUpdate.IsEnabled = true;
            }
        }

        public void TextBoxPhoneNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (LabelFNameResult.Content.ToString() == "" || textBoxUserMifare.Text == "" || textBoxPhoneNumber.Text == "")
            {
                ButtonCreateOrUpdate.IsEnabled = false;
            }
            else
            {
                ButtonCreateOrUpdate.IsEnabled = true;
            }
        }

        private void ButtonCreateOrUpdate_Click(object sender, RoutedEventArgs e)
        {
            UserController.Instance.CreateUserObjectToAddInDB(textBoxUserMifare.Text, LabelFNameResult.Content.ToString(), LabelLNameResult.Content.ToString(), textBoxZbcName.Text, Convert.ToInt32(textBoxPhoneNumber.Text), false, Convert.ToBoolean(checkBoxIsTeacher.Content));
        }
    }
}
