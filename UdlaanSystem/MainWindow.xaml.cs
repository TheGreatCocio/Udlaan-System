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
        private bool isUserScanned = false;

        private void OnMyfareScanned(object sender, KeyEventArgs e)// Runs when a key is pressed.
        {
            if (TextBoxMain.IsFocused) // checks if the maintextbox is focused
            {
                if (e.Key == Key.Return) // checks if its the enter button that has been pressed
                {
                    ItemObject scannedItem = ItemController.Instance.CheckIfMifareIsItem(TextBoxMain.Text);

                    if (scannedItem == null)
                    {
                        LendedObject lendedObject = LendController.Instance.GetUserData(TextBoxMain.Text);
                        if (lendedObject.UserObject == null)
                        {
                            MessageBox.Show("Findes Ikke I Databasen!!");
                        }
                        else
                        {
                            isUserScanned = true;
                            scannedUser = lendedObject.UserObject;
                            PrintUserData(lendedObject);
                            if (lendedObject.UserObject.comment != "")
                            {
                                MessageBox.Show(lendedObject.UserObject.comment);
                            }
                        }

                    }
                    else
                    {
                        string userMifare = LendController.Instance.CheckIfLended(scannedItem.itemMifare);
                        LendObject scannedLendObject = new LendObject(scannedItem, DateTime.Now, datePickerReturn.SelectedDate.Value.Date, null);

                        if (userMifare != "")
                        {
                            LendedObject lendedObject = LendController.Instance.GetUserData(userMifare);
                            PrintItemToList(scannedLendObject);

                            scannedUser = lendedObject.UserObject;
                            PrintUserData(lendedObject);
                            if (lendedObject.UserObject.comment != "")
                            {
                                MessageBox.Show(lendedObject.UserObject.comment);
                            }
                        }
                        else
                        {
                            try
                            {
                                PrintItemToList(scannedLendObject);
                            }
                            catch (Exception)
                            {
                                MessageBox.Show("Vælg venligst en dato");
                            }
                        }
                    }
                    TextBoxMain.Text = "";
                    TextBoxMain.Focus();
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
            LabelNameResult.Visibility = Visibility.Visible;

            LabelZbcNameResult.Content = lendedObject.UserObject.zbcName;
            LabelZbcNameResult.Visibility = Visibility.Visible;

            LabelMifareResult.Content = lendedObject.UserObject.userMifare;
            LabelMifareResult.Visibility = Visibility.Visible;

            LabelPhoneResult.Content = lendedObject.UserObject.phoneNumber;
            LabelPhoneResult.Visibility = Visibility.Visible;

            LabelTeacherResult.Content = lendedObject.UserObject.isTeacher;
            LabelTeacherResult.Visibility = Visibility.Visible;

            LabelIsDisabledResult.Content = lendedObject.UserObject.isDisabled;
            LabelIsDisabledResult.Visibility = Visibility.Visible;


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
            if (scannedUser != null)
            {
                if (SmsController.Instance.GenerateVerificationSms(scannedUser.phoneNumber))
                {
                    if (LendController.Instance.GenLendedObject(scannedUser, scannedItems))
                    {
                        SmsController.Instance.GenerateLendReceipt(scannedUser, scannedItems);
                        MessageBox.Show("Udstyret er nu udlånt og der er sendt en kvitering til personen via SMS");

                        ClearUI();
                }
                    else
                    {
                        MessageBox.Show("OPS, udstyret blev IKKE udlånt! Hvis dette fortsætter, kontakt IT.");
                    }
                }
            }
            else
            {
                MessageBox.Show("Du Skal Scanne En Bruger");
            }
        }

        private void ButtonReturn_Click(object sender, RoutedEventArgs e)
        {
            if (isUserScanned == true)
            {
                if (SmsController.Instance.GenerateVerificationSms(scannedUser.phoneNumber))
                {
                    if (LendController.Instance.MoveLendedIntoArchive(scannedItems))
                    {
                        SmsController.Instance.GenerateReturnReceipt(scannedUser, scannedItems);
                        MessageBox.Show("Udstyret er nu afleveret og der er sendt en kvitering til personen via SMS");

                        ClearUI();
                    }
                    else
                    {
                        MessageBox.Show("OPS, udstyret blev IKKE afleveret! Hvis dette fortsætter, kontakt IT.");
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

        private void ButtonClear_Click(object sender, RoutedEventArgs e)
        {
            ClearUI();
        }

        private void ClearUI()
        {
            scannedItems = new List<LendObject>();
            scannedUser = null;

            LabelNameResult.Visibility = Visibility.Hidden;
            LabelZbcNameResult.Visibility = Visibility.Hidden;
            LabelMifareResult.Visibility = Visibility.Hidden;
            LabelPhoneResult.Visibility = Visibility.Hidden;
            LabelTeacherResult.Visibility = Visibility.Hidden;
            LabelIsDisabledResult.Visibility = Visibility.Hidden;

            this.ListViewLend.Items.Clear();
            this.ListViewItems.Items.Clear();

            this.datePickerReturn.SelectedDate = DateTime.Now;

            isUserScanned = false;
        }

        private void ButtonComment_Click(object sender, RoutedEventArgs e)
        {
            UIUserNote uiUserNote = new UIUserNote();
            uiUserNote.ShowDialog();
        }
    }
}
