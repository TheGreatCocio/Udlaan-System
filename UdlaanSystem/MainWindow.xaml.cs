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

        private List<LendObject> scannedItems = new List<LendObject>();
        private UserObject scannedUser = null;

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
                            scannedUser = lendedObject.UserObject;
                            PrintUserData(lendedObject);
                        }

                    }
                    else
                    {
                        string userMifare = LendController.Instance.CheckIfLended(item.itemMifare);

                        if (userMifare != "")
                        {
                            LendedObject lendedObject = LendController.Instance.GetUserData(userMifare);

                            foreach (LendObject lendObject in lendedObject.LendObjects)
                            {
                                if (lendObject.itemObject.itemMifare == item.itemMifare)
                                {
                                    PrintItemToList(lendObject);
                                }
                            }
                            scannedUser = lendedObject.UserObject;
                            PrintUserData(lendedObject);
                        }
                        else
                        {
                            try
                            {
                                LendObject lendObject = new LendObject(item, DateTime.Now, datePickerReturn.SelectedDate.Value.Date, null);
                                PrintItemToList(lendObject);
                            }
                            catch (Exception)
                            {
                                MessageBox.Show("Vælg venligst en dato");
                            }
                        }
                    }
                }
            }
        }

        private void PrintItemToList(LendObject lendObject)
        {
            scannedItems.Add(lendObject);
            this.ListViewItems.Items.Add(new ListViewObject(lendObject.itemObject.itemMifare, lendObject.itemObject.type, lendObject.itemObject.manufacturer, lendObject.itemObject.model, lendObject.itemObject.id, lendObject.itemObject.serialNumber, lendObject.lendDate, lendObject.returnDate, lendObject.returnedDate, null));
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


            this.ListViewLend.Items.Clear();

            foreach (LendObject lendObject in lendedObject.LendObjects)
            {
                bool? isOverdue = null;

                if (lendObject.returnDate <= DateTime.Now && lendObject.returnedDate == null)
                {
                    isOverdue = true;
                }
                else if (lendObject.returnDate > DateTime.Now && lendObject.returnedDate == null)
                {
                    isOverdue = false;
                }
                this.ListViewLend.Items.Add(new ListViewObject(lendObject.itemObject.itemMifare, lendObject.itemObject.type, lendObject.itemObject.manufacturer, lendObject.itemObject.model, lendObject.itemObject.id, lendObject.itemObject.serialNumber, lendObject.lendDate, lendObject.returnDate, lendObject.returnedDate, isOverdue));
            }
        }

        private void ButtonItem_Click(object sender, RoutedEventArgs e)
        {
            UIInputItem inputItembox = new UIInputItem();
            inputItembox.ShowDialog();
        }

        private void ButtonUser_Click(object sender, RoutedEventArgs e)
        {
            UIInputUser inputUserbox = new UIInputUser();
            inputUserbox.ShowDialog();
        }

        private void ButtonLend_Click(object sender, RoutedEventArgs e)
        {
            if (SmsController.Instance.GenerateVerificationSms(scannedUser.phoneNumber))
            {


                if (LendController.Instance.GenLendedObject(scannedUser, scannedItems))
                {
                    SmsController.Instance.GenerateLendReceipt(scannedUser, scannedItems);
                    MessageBox.Show("Udstyret er nu udlånt og der er sendt en kvitering til personen via SMS");
                }
                else
                {
                    MessageBox.Show("FAILED TO INSERT LENDS TO DATABASE");
                }
            }
        }

        private void ButtonReturn_Click(object sender, RoutedEventArgs e)
        {
            if (scannedUser != null)
            {
                if (SmsController.Instance.GenerateVerificationSms(scannedUser.phoneNumber))
                {
                    if (LendController.Instance.MoveLendedIntoArchive(scannedItems))
                    {
                        SmsController.Instance.GenerateReturnReceipt(scannedUser, scannedItems);
                        MessageBox.Show("Udstyret er nu afleveret og der er sendt en kvitering til personen via SMS");
                    }
                    else
                    {
                        MessageBox.Show("FAILED TO MOVE LENDS TO ARCHIVE");
                    }
                }
            }
            else
            {
                MessageBox.Show("Brugeren SKAL scannes for at kunne aflevere sit udstyr!");
            }
            
        }

        private void ButtonStat_Click(object sender, RoutedEventArgs e)
        {
            //bool test = SmsController.Instance.GenerateVerificationSms(30621451);
        }

        //Når Datepickeren bliver loaded bliver dens valgte værdi sat til i morgen.
        private void DatePickerReturn_Loaded(object sender, RoutedEventArgs e)
        {
            datePickerReturn.SelectedDate = DateTime.Now.AddDays(1);
        }
    }
}
