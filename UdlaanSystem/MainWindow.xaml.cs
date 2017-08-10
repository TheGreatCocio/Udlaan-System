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

namespace UdlaanSystem
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

        private void onMyfareScanned(object sender, KeyEventArgs e)// Runs when a key is pressed.
        {
            if (TextBoxMain.IsFocused) // checks if the maintextbox is focused
            {
                if (e.Key == Key.Return) // checks if its the enter button that has been pressed
                {
                    ItemObject item = ItemController.Instance.CheckIfMifareIsItem(TextBoxMain.Text);

                    if (item == null)
                    {
                        LendedObject lendedObject = LendController.Instance.GetUserData(TextBoxMain.Text);
                        if (lendedObject.UserObject == null)
                        {
                            MessageBox.Show("Findes Ikke I Databasen!!");
                        }
                        else
                        {
                            PrintUserData(lendedObject);
                        }

                    }
                    else
                    {
                        string userMifare = LendController.Instance.CheckIfLended(item.itemMifare);

                        if (userMifare == "")
                        {
                            PrintItemToList(item);
                        }
                        else
                        {
                            LendedObject lendedObject = LendController.Instance.GetUserData(userMifare);
                            PrintItemToList(item);
                        }
                    }
                    /*ItemObject item = 

                        List<ItemObject> items = new List<ItemObject>();
                        items.Add(new ItemObject() { itemMifare = item.itemMifare, type = item.type, manufacturer = item.manufacturer, model = item.model, ID = 0, serialNumber = "asdf" });
                        items.Add(new User() { Name = "Jane Doe", Age = 39 });
                        items.Add(new User() { Name = "Sammy Doe", Age = 13 });


                        //ListViewItems.ItemsSource

                        */
                }
            }
        }

        private void PrintItemToList(ItemObject item)
        {
            this.ListViewItems.Items.Add(item);
        }


        public void PrintUserData(LendedObject lendedObject)
        {
            LabelNameResult.Content = lendedObject.UserObject.fName + " " + lendedObject.UserObject.lName;
            LabelNameResult.Visibility = Visibility;

            LabelZbcNameResult.Content = lendedObject.UserObject.zbcName;
            LabelZbcNameResult.Visibility = Visibility;

            LabelMifareResult.Content = lendedObject.UserObject.userMifare;
            LabelMifareResult.Visibility = Visibility;

            LabelPhoneResult.Content = lendedObject.UserObject.phoneNumber;
            LabelPhoneResult.Visibility = Visibility;

            LabelTeacherResult.Content = lendedObject.UserObject.isTeacher;
            LabelTeacherResult.Visibility = Visibility;

            LabelIsDisabledResult.Content = lendedObject.UserObject.isDisabled;
            LabelIsDisabledResult.Visibility = Visibility;

            foreach (LendObject lendObject in lendedObject.LendObjects)
            {
                this.ListViewLend.Items.Add(lendObject.itemObject);
                this.ListViewLend.Items.Add(lendObject.returnDate);
            }

            
        }

        private void ButtonItem_Click(object sender, RoutedEventArgs e)
        {
            UIInputItem inputItembox = new UIInputItem();
            inputItembox.ShowDialog();
        }

        /*private string ButtonUser_Click(object sender, RoutedEventArgs e)
        {
            UIInputUser inputUserBox = new UIInputUser();
        }*/
    }
}
