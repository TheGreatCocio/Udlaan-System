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
    /// Interaction logic for UIChangeToUniLogin.xaml
    /// </summary>
    public partial class UIChangeToUniLogin : Window
    {
        public UIChangeToUniLogin()
        {
            InitializeComponent();
        }

        public void TextBoxUniLogin_TextChanged(object sender, TextChangedEventArgs e)
        {            
            LabelFNameResult.Content = "";
            LabelLNameResult.Content = "";

            string[] userInfo = UserController.Instance.CheckIfUserExistInAD(textBoxUniLogin.Text);

            if (userInfo[0] != "")
            {
                LabelFNameResult.Content = userInfo[0];
                LabelLNameResult.Content = userInfo[1];
                LabelFound.Visibility = Visibility.Visible;
                ButtonSave.IsEnabled = true;
            }
            else
            {
                LabelFNameResult.Content = "";
                LabelLNameResult.Content = "";
                LabelFound.Visibility = Visibility.Hidden;
                ButtonSave.IsEnabled = false;
            }
        }

        public void TextBoxZbcName_TextChanged(object sender, TextChangedEventArgs e)
        {
            UserObject userObject = null;
            userObject = UserController.Instance.CheckIfUserExist(textBoxUniLogin.Text);            

            if (userObject != null)
            {
                LabelFoundZbc.Visibility = Visibility.Visible;
                LabelFoundZbc.Foreground.Equals("LawnGreen");
                LabelFoundZbc.Content.Equals("Fundet!");
            }
            else
            {
                LabelFoundZbc.Visibility = Visibility.Visible;
                LabelFoundZbc.Foreground = new SolidColorBrush(Color.FromArgb(255, 255, 58, 58));
                LabelFoundZbc.Content = "Ikke Fundet";
            }
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {                        
            try
            {
                if (LabelFound.IsVisible && LabelFoundZbc.IsVisible)
                {
                    UserController.Instance.UpdateUserZbcNameToUniLogin(textBoxZbcName.Text, textBoxUniLogin.Text);
                    MessageBox.Show("Brugeren er nu opdateret");
                }
                else
                {
                    MessageBox.Show("Brugeren er ikke fundet, enten via zbc navn eller uni login");                
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Brugeren blev ikke opdateret");
                throw;
            }
            
            this.Close();
        }
    }
}
