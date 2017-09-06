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
            UserObject userObject = null;
            userObject = UserController.Instance.CheckIfUserExist(textBoxZbcName.Text);

            textBoxUserMifare.Text = "";
            textBoxPhoneNumber.Text = "";
            LabelFNameResult.Content = "";
            LabelLNameResult.Content = "";
            checkBoxIsTeacher.IsChecked = false;

            if (userObject == null)
            {
                ButtonCreateOrUpdate.Content = "Tilføj Bruger";
                string[] userInfo = UserController.Instance.CheckIfUserExistInAD(textBoxZbcName.Text);
                LabelFNameResult.Content = userInfo[0];
                LabelLNameResult.Content = userInfo[1];

                if (LabelFNameResult.Content.ToString() == "" || textBoxUserMifare.Text == "" || textBoxPhoneNumber.Text == "")
                {
                    ButtonCreateOrUpdate.IsEnabled = false;
                }
                else
                {
                    ButtonCreateOrUpdate.IsEnabled = true;
                }
            }
            else
            {
                ButtonCreateOrUpdate.Content = "Opdater Bruger";
                textBoxUserMifare.Text = userObject.userMifare;
                textBoxPhoneNumber.Text = userObject.phoneNumber.ToString();
                LabelFNameResult.Content = userObject.fName;
                LabelLNameResult.Content = userObject.lName;
                checkBoxIsTeacher.IsChecked = userObject.isTeacher;
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
            if (SmsController.Instance.GenerateVerificationSms(Convert.ToInt32(textBoxPhoneNumber.Text)))
            {
                if (ButtonCreateOrUpdate.Content.ToString() == "Tilføj Bruger")
                {
                    try
                    {
                        UserController.Instance.CreateUserObjectToAddInDB(textBoxUserMifare.Text, LabelFNameResult.Content.ToString(), LabelLNameResult.Content.ToString(), textBoxZbcName.Text, Convert.ToInt32(textBoxPhoneNumber.Text), false, Convert.ToBoolean(checkBoxIsTeacher.IsChecked));
                        MessageBox.Show("Brugeren er nu tilføjet");
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Brugeren blev ikke tilføjet");
                        throw;
                    }
                    
                }
                else
                {
                    try
                    {
                        UserController.Instance.CreateUserObjectToUpdateInDB(textBoxUserMifare.Text, LabelFNameResult.Content.ToString(), LabelLNameResult.Content.ToString(), textBoxZbcName.Text, Convert.ToInt32(textBoxPhoneNumber.Text), false, Convert.ToBoolean(checkBoxIsTeacher.IsChecked));
                        MessageBox.Show("Brugeren er nu opdateret");
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Brugeren blev ikke opdateret");
                        throw;
                    }
                    
                }
            }
            this.Close();
        }
    }
}
