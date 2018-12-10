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
using UdlaanSystem.Properties;

namespace UdlaanSystem
{
    /// <summary>
    /// Interaction logic for UIInputUser.xaml
    /// </summary>

    public partial class UIInputUser : Window
    {
        //public void CardInserted(string hex)
        //{
        //    Application.Current.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, new Action(() => textBoxUserMifare.Text = hex));
        //    System.Windows.Forms.SendKeys.SendWait("{ENTER}");
        //}

        public UIInputUser()
        {
            InitializeComponent();
            textBoxZbcName.Focus();
            if (Settings.Default.LocationRoskilde)
            {
                labelEUUser.Visibility = Visibility.Visible;
                checkBoxEUUser.Visibility = Visibility.Visible;
            }
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

                EnableButton();
            }
            else
            {
                ButtonCreateOrUpdate.Content = "Opdater Bruger";
                textBoxUserMifare.Text = userObject.UserMifare;
                textBoxPhoneNumber.Text = userObject.PhoneNumber.ToString();
                LabelFNameResult.Content = userObject.FName;
                LabelLNameResult.Content = userObject.LName;
                checkBoxIsTeacher.IsChecked = userObject.IsTeacher;
            }
        }

        public void TextBoxUserMifare_TextChanged(object sender, TextChangedEventArgs e)
        {
            EnableButton();
        }

        public void TextBoxPhoneNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            EnableButton();
        }

        public void checkBoxIsTeacher_CheckedChanged(object sender, EventArgs e)
        {
            EnableButton();
        }

        public void checkBoxEU_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxEUUser.IsChecked == true)
            {
                if (!LabelFNameResult.Content.Equals("") && textBoxUserMifare.Text != "")
                {
                    textBoxPhoneNumber.IsEnabled = false;
                    ButtonCreateOrUpdate.IsEnabled = true;
                }
                else
                {
                    textBoxPhoneNumber.IsEnabled = true;
                    ButtonCreateOrUpdate.IsEnabled = false;
                }
            }
            else
            {
                textBoxPhoneNumber.IsEnabled = true;
                ButtonCreateOrUpdate.IsEnabled = false;
            }
        }

        private void EnableButton()
        {
            if (LabelFNameResult.Content.Equals("") || textBoxUserMifare.Text == "" || textBoxPhoneNumber.Text == "")
            {
                if (!LabelFNameResult.Content.Equals("") && textBoxUserMifare.Text != "" && checkBoxEUUser.IsChecked == true)
                {
                    ButtonCreateOrUpdate.IsEnabled = true;
                }
                else
                {
                    ButtonCreateOrUpdate.IsEnabled = false;
                }                
            }            
            else
            {                
                ButtonCreateOrUpdate.IsEnabled = true;
            }
        }

        // Opretter / Opdatere en bruger alt afhængig af om de allerede er oprettet i systemet i forvejen.
        private void ButtonCreateOrUpdate_Click(object sender, RoutedEventArgs e)
        {
            // Hvis lokationen er "Roskilde" så har de muligheden for at gå uden om Tlf Nummer hvis det er EU elever der skal låne
            if (Settings.Default.LocationRoskilde)
            {
                if (checkBoxEUUser.IsChecked == false)
                {
                    if (SmsController.Instance.GenerateVerificationSms(Convert.ToInt32(textBoxPhoneNumber.Text)))
                    {
                        CreateOrUpdateUser();
                    }
                }
                else
                {
                    CreateOrUpdateUser();
                }
            }            
            else
            {
                if (SmsController.Instance.GenerateVerificationSms(Convert.ToInt32(textBoxPhoneNumber.Text)))
                {
                    CreateOrUpdateUser();
                }
            }
            
            this.Close();
        }

        private void CreateOrUpdateUser()
        {
            if (ButtonCreateOrUpdate.Content.ToString() == "Tilføj Bruger")
            {
                try
                {
                    UserController.Instance.CreateUserObjectToAddInDB(textBoxUserMifare.Text, LabelFNameResult.Content.ToString(), LabelLNameResult.Content.ToString(), textBoxZbcName.Text, textBoxPhoneNumber.IsEnabled ? Convert.ToInt32(textBoxPhoneNumber.Text) : 0000, false, Convert.ToBoolean(checkBoxIsTeacher.IsChecked));
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

        private void ButtonUnilogin_Click(object sender, RoutedEventArgs e)
        {
            UIChangeToUniLogin uniLoginChangeBox = new UIChangeToUniLogin();
            uniLoginChangeBox.ShowDialog();
        }
    }
}
