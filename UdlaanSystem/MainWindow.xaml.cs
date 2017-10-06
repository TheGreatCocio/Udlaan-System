using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
            TextBoxMain.Focus();
        }

        private List<LendObject> scannedItems = new List<LendObject>();
        private List<string> ScannedItemMifares = new List<string>();
        private UserObject userInUse = null;
        private UserObject scannedUser = null;
        private bool isUserScanned = false;
        private bool? isItemsLended = null;


        private void OnMyfareScanned(object sender, KeyEventArgs e)// Runs when a key is pressed.
        {
            if (TextBoxMain.IsFocused) // checks if the maintextbox is focused
            {
                if (e.Key == Key.Return) // checks if its the enter button that has been pressed
                {
                    ItemObject scannedItem = ItemController.Instance.CheckIfMifareIsItem(TextBoxMain.Text);

                    if (scannedItem == null)//On User Scanned
                    {
                        LendedObject lendedObject = LendController.Instance.GetUserData(TextBoxMain.Text);
                        if (lendedObject.UserObject == null)
                        {
                            if (!CheckForInternetConnection())
                            {
                                MessageBox.Show("Der Er Ikke Noget Internet");
                            }
                            else if (isItemsLended == true && lendedObject.UserObject.userMifare != userInUse.userMifare)
                            {
                                MessageBox.Show("FUCK YEAH");
                            }
                            else
                            {
                                MessageBox.Show("Findes Ikke I Databasen!!");
                            }
                        }
                        else
                        {
                            if (isItemsLended == true && lendedObject.UserObject.userMifare != userInUse.userMifare)
                            {
                                MessageBox.Show("Du kan ikke scanne andre brugere end den udstyret tilhøre!");
                            }
                            else
                            {
                                isUserScanned = true;

                                userInUse = lendedObject.UserObject;

                                PrintUserData(lendedObject);

                                CommentCheck(lendedObject);
                            }
                        }

                    }
                    else//On Items Scanned
                    {
                        string userMifare = LendController.Instance.CheckIfLended(scannedItem.itemMifare);
                        TimeSpan timeSpanMonToThur = new TimeSpan(15, 30, 00);
                        TimeSpan timeSpanFri = new TimeSpan(13, 30, 00);
                        LendObject scannedLendObject = null;
                        
                        if (DateTime.Now.DayOfWeek == DayOfWeek.Friday)
                        {
                            scannedLendObject = new LendObject(scannedItem, DateTime.Now, datePickerReturn.SelectedDate.Value.Date + timeSpanFri, null);
                        }
                        else
                        {
                            scannedLendObject = new LendObject(scannedItem, DateTime.Now, datePickerReturn.SelectedDate.Value.Date + timeSpanMonToThur, null);
                        }

                        if (userMifare != "")
                        {
                            //ScannedItem Er Udlånt
                            if (isItemsLended != false)
                            {
                                if (userInUse == null || userMifare == userInUse.userMifare)
                                {
                                    isItemsLended = true;
                                    if (!ScannedItemMifares.Contains(scannedItem.itemMifare))
                                    {
                                        LendedObject lendedObject = LendController.Instance.GetUserData(userMifare);

                                        foreach (LendObject lendObject in lendedObject.LendObjects)
                                        {
                                            if (lendObject.itemObject == scannedLendObject.itemObject)
                                            {
                                                scannedLendObject.returnDate = lendObject.returnDate;
                                                scannedLendObject.lendDate = lendObject.lendDate;
                                            }
                                        }

                                        PrintItemToList(scannedLendObject);

                                        userInUse = lendedObject.UserObject;

                                        PrintUserData(lendedObject);

                                        CommentCheck(lendedObject);
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Du kan kun scanne udstyr er udlånt til samme bruger!");
                                }
                            }
                            else
                            {
                                MessageBox.Show("Du må IKKE scanne udstyr der er udlånt med udstyr der ikke er udlånt!");
                            }
                        }
                        else
                        {
                            //ScannedItem Er IKKE Udlånt
                            if (isItemsLended != true)
                            {
                                isItemsLended = false;
                                try
                                {
                                    if (!ScannedItemMifares.Contains(scannedItem.itemMifare))
                                    {
                                        if (userInUse != null)
                                        {
                                            if (userInUse.hasPC && !userInUse.isTeacher && scannedLendObject.itemObject.type == "Computer")
                                            {
                                                MessageBox.Show("Denne Bruger Har Allerede 1 Computer Og Er Ikke Lærer");
                                            }
                                            else
                                            {
                                                PrintItemToList(scannedLendObject);
                                            }
                                        }
                                        else
                                        {
                                            PrintItemToList(scannedLendObject);
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("Du Har Allerede Scannet Dette Produkt");
                                    }
                                }
                                catch (Exception)
                                {
                                    MessageBox.Show("Noget Gik Galt Fejlkode: 10x5");
                                }
                            }
                            else
                            {
                                MessageBox.Show("Du må IKKE scanne udstyr der ikke er udlånt med udstyr der er udlånt!");
                            }
                        }
                    }
                    TextBoxMain.Text = "";
                    TextBoxMain.Focus();
                    if (isItemsLended == true)
                    {
                        ButtonReturn.IsEnabled = true;
                        ButtonLend.IsEnabled = false;
                    }
                    else
                    {
                        ButtonLend.IsEnabled = true;
                        ButtonReturn.IsEnabled = false;
                    }
                }
            }
        }

        private void PrintItemToList(LendObject lendObject)
        {
            scannedItems.Add(lendObject);
            ScannedItemMifares.Add(lendObject.itemObject.itemMifare);
            this.ListViewItems.Items.Add(new ListViewObject(lendObject.itemObject.itemMifare, lendObject.itemObject.type, lendObject.itemObject.manufacturer, lendObject.itemObject.model, lendObject.itemObject.id, lendObject.itemObject.serialNumber, lendObject.lendDate, lendObject.returnDate, lendObject.returnedDate, null, ""));
        }

        


        public void PrintUserData(LendedObject lendedObject)
        {
            LabelNameResult.Content = lendedObject.UserObject.fName + " " + lendedObject.UserObject.lName;
            LabelNameResult.Visibility = Visibility.Visible;

            LabelZbcNameResult.Content = lendedObject.UserObject.zbcName;
            LabelZbcNameResult.Visibility = Visibility.Visible;

            LabelPhoneResult.Content = lendedObject.UserObject.phoneNumber;
            LabelPhoneResult.Visibility = Visibility.Visible;

            if (lendedObject.UserObject.isTeacher)
            {
                LabelTeacherResult.Content = "Ja";
            }
            else
            {
                LabelTeacherResult.Content = "Nej";
            }

            if (userInUse.isDisabled)
            {
                LabelIsDisabledResult.Content = "Ja";
                LabelIsDisabledResult.Foreground = new SolidColorBrush(Colors.Red);
            }
            else
            {
                LabelIsDisabledResult.Content = "Nej";
                LabelIsDisabledResult.Foreground = new SolidColorBrush(Colors.Green);
            }

            if (isUserScanned)
            {
                LabelIsScannedResult.Content = "Ja";
                LabelIsScannedResult.Foreground = new SolidColorBrush(Colors.Green);
            }
            else
            {
                LabelIsScannedResult.Content = "Nej";
                LabelIsScannedResult.Foreground = new SolidColorBrush(Colors.Red);
            }

            LabelTeacherResult.Visibility = Visibility.Visible;
            LabelIsDisabledResult.Visibility = Visibility.Visible;
            LabelIsScannedResult.Visibility = Visibility.Visible;

            ButtonComment.IsEnabled = true;

            this.ListViewLend.Items.Clear();

            foreach (LendObject lendObject in lendedObject.LendObjects)
            {
                bool? isOverdue = null;

                if (lendObject.returnDate.Date < DateTime.Now.Date && lendObject.returnedDate == null)
                {
                    isOverdue = true;
                }
                else if (lendObject.returnDate.Date > DateTime.Now.Date && lendObject.returnedDate == null)
                {
                    isOverdue = false;
                }
                else if (lendObject.returnDate.Date == DateTime.Now.Date && lendObject.returnedDate == null)
                {
                    if (lendObject.returnDate.TimeOfDay < DateTime.Now.TimeOfDay && lendObject.returnedDate == null)
                    {
                        isOverdue = true;
                    }
                    else if (lendObject.returnDate.TimeOfDay >= DateTime.Now.TimeOfDay && lendObject.returnedDate == null)
                    {
                        isOverdue = false;
                    }
                }
                this.ListViewLend.Items.Add(new ListViewObject(lendObject.itemObject.itemMifare, lendObject.itemObject.type, lendObject.itemObject.manufacturer, lendObject.itemObject.model, lendObject.itemObject.id, lendObject.itemObject.serialNumber, lendObject.lendDate, lendObject.returnDate, lendObject.returnedDate, isOverdue, ""));
            }
            TextBoxMain.Focus();
        }

        private void ButtonItem_Click(object sender, RoutedEventArgs e)
        {
            UIInputItem inputItembox = new UIInputItem();
            inputItembox.ShowDialog();
            TextBoxMain.Focus();
        }

        private void ButtonUser_Click(object sender, RoutedEventArgs e)
        {
            UIInputUser inputUserbox = new UIInputUser();
            inputUserbox.ShowDialog();
            TextBoxMain.Focus();
        }

        private void ButtonLend_Click(object sender, RoutedEventArgs e)
        {
            if (userInUse != null)
            {
                int scannedItemsContainsComputer = 0;

                foreach (LendObject scannedItem in scannedItems)
                {
                    if (scannedItem.itemObject.type == "Computer")
                    {
                        scannedItemsContainsComputer++;
                    }
                }
                if (!userInUse.isTeacher && userInUse.hasPC && scannedItemsContainsComputer > 0)
                {
                    MessageBox.Show("OPS, udstyret blev IKKE udlånt! Brugeren har i forvejen 1 computer og denne bruger er ikke lærer");
                }
                else if(!userInUse.isTeacher && !userInUse.hasPC && scannedItemsContainsComputer > 1)
                {
                    MessageBox.Show("OPS, udstyret blev IKKE udlånt! Der er scannet mere end 1 computer og denne bruger er ikke lærer");
                }
                else
                {
                    if (SmsController.Instance.GenerateVerificationSms(userInUse.phoneNumber))
                    {
                        if (LendController.Instance.GenLendedObject(userInUse, scannedItems))
                        {
                            SmsController.Instance.GenerateLendReceipt(userInUse, scannedItems);
                            MessageBox.Show("Udstyret er nu udlånt og der er sendt en kvitering til personen via SMS");

                            ClearUI();
                        }
                        else
                        {
                            MessageBox.Show("OPS, udstyret blev IKKE udlånt! Hvis dette fortsætter, kontakt IT.");
                        }
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
                if (SmsController.Instance.GenerateVerificationSms(userInUse.phoneNumber))
                {
                    if (LendController.Instance.MoveLendedIntoArchive(scannedItems))
                    {
                        SmsController.Instance.GenerateReturnReceipt(userInUse, scannedItems);
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
            UIStat uiUserNote = new UIStat();
            uiUserNote.ShowDialog();
        }

        //Når Datepickeren bliver loaded bliver dens valgte værdi sat til i morgen.
        private void DatePickerReturn_Loaded(object sender, RoutedEventArgs e)
        {
            datePickerReturn.SelectedDate = DateTime.Now;
        }

        private void ButtonClear_Click(object sender, RoutedEventArgs e)
        {
            ClearUI();
        }

        private void ClearUI()
        {
            scannedItems = new List<LendObject>();
            ScannedItemMifares = new List<string>();
            userInUse = null;

            LabelNameResult.Visibility = Visibility.Hidden;
            LabelZbcNameResult.Visibility = Visibility.Hidden;            
            LabelPhoneResult.Visibility = Visibility.Hidden;
            LabelTeacherResult.Visibility = Visibility.Hidden;
            LabelIsDisabledResult.Visibility = Visibility.Hidden;
            LabelIsScannedResult.Visibility = Visibility.Hidden;

            ButtonComment.IsEnabled = false;

            this.ListViewLend.Items.Clear();
            this.ListViewItems.Items.Clear();

            this.datePickerReturn.SelectedDate = DateTime.Now;

            isUserScanned = false;
            isItemsLended = null;

            TextBoxMain.Focus();

            ButtonReturn.IsEnabled = false;
            ButtonLend.IsEnabled = false;
        }

        private void ButtonComment_Click(object sender, RoutedEventArgs e)
        {
            UIUserNote uiUserNote = new UIUserNote(userInUse);
            uiUserNote.ShowDialog();
            TextBoxMain.Focus();
        }

        private void CommentCheck(LendedObject lendedObject)
        {
            if (lendedObject.UserObject.comment != "")
            {
                MessageBox.Show(userInUse.comment);
            }
        }

        private void ButtonDeleteItems_Click(object sender, RoutedEventArgs e)
        {
            foreach (ListViewObject listViewObject in ListViewItems.SelectedItems)
            {
                foreach (LendObject lendObject in scannedItems.ToList())
                {
                    if (lendObject.itemObject.itemMifare == listViewObject.itemMifare)
                    {
                        scannedItems.Remove(lendObject);
                        ScannedItemMifares.Remove(lendObject.itemObject.itemMifare);
                    }
                }
            }
            RefreshListViewItems();
            TextBoxMain.Focus();
        }

        private void RefreshListViewItems()
        {
            this.ListViewItems.Items.Clear();
            foreach (LendObject lendObject in scannedItems)
            {
                this.ListViewItems.Items.Add(new ListViewObject(lendObject.itemObject.itemMifare, lendObject.itemObject.type, lendObject.itemObject.manufacturer, lendObject.itemObject.model, lendObject.itemObject.id, lendObject.itemObject.serialNumber, lendObject.lendDate, lendObject.returnDate, lendObject.returnedDate, null, ""));
            }
        }

        private void DatePickerReturn_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (datePickerReturn.SelectedDate.Value.DayOfWeek == DayOfWeek.Saturday || datePickerReturn.SelectedDate.Value.DayOfWeek == DayOfWeek.Sunday)
            {
                MessageBox.Show("Du må ikke vælge at afleveringsdatoen skal være i weekenden!");
                datePickerReturn.SelectedDate = DateTime.Now;
            }
            else
            {
                if (ListViewItems.SelectedItems.Count != 0)
                {
                    foreach (ListViewObject listViewObject in ListViewItems.SelectedItems)
                    {
                        foreach (LendObject lendObject in scannedItems.ToList())
                        {
                            if (lendObject.itemObject.itemMifare == listViewObject.itemMifare)
                            {
                                lendObject.returnDate = datePickerReturn.SelectedDate.Value.Date;
                            }
                        }
                    }
                    RefreshListViewItems();
                }
            }
            TextBoxMain.Focus();
        }

        public static bool CheckForInternetConnection()
        {
            try
            {
                using (var client = new WebClient())
                {
                    using (client.OpenRead("http://clients3.google.com/generate_204"))
                    {
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
