using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using UdlaanSystem.Managers;
using UdlaanSystem.Views;
using UdlaanSystem.Views;

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
            StartScanner();
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
                    ItemObject scannedItem = ItemController.Instance.CheckIfMifareIsItem(TextBoxMain.Text); //Tjekker om den kan finde et item udfra det scannede Mifare. Hvis den ikke returner et item er scannedItem = null og mifare er enter en user eller findes ikke

                    if (scannedItem == null)//On User Scanned
                    {
                        LendedObject lendedObject = LendController.Instance.GetUserData(TextBoxMain.Text); //Henter Det lended Object der tilhøre det usermifare der er blevet scanned, inklusive alle hans date, hans nuværende lån og hans arkiv
                        if (lendedObject.UserObject == null)//Hvis userobject er null findes han ikke og skal derfor hentes fra den gamle database
                        {
                            if (!CheckForInternetConnection())//Tjekker om der er internet
                            {
                                MessageBox.Show("Der Er Ikke Noget Internet");
                            }
                            else if (isItemsLended == true && lendedObject.UserObject.UserMifare != userInUse.UserMifare) //Tjekker at du ikke scanner 2 forskellige brugeres udstyr i samma omgang
                            {
                                MessageBox.Show("Du kan ikke scanne udstyr der er udlånt til forskellige brugere!");
                            }
                            else
                            {
                                /*
                                 * Denne del af koden er UDELUKKENDE til mmigration så udstyret bliver afleveret i den gamle DB!!!
                                 */
                                if (MigrationController.Instance.CheckIfItemIsLendedInOldDB(TextBoxMain.Text))//Hvis mifaret ikke tilhøre en bruger skal vi tjekke om det er udlånt i den gamle DB og aflevere det
                                {
                                    MigrationController.Instance.ReturnItemInOldDB(TextBoxMain.Text);
                                    MessageBox.Show("Udstyret er afleveret i den gamle databse, men er ikke scannet ind i den nye!" + Environment.NewLine + "Scan det venligst ind når i har tid!");
                                }
                                else
                                {
                                    MessageBox.Show("Findes ikke i databasen!");
                                }
                                /*
                                 * Her Til
                                 */
                            }
                        }
                        else
                        {
                            if (isItemsLended == true && lendedObject.UserObject.UserMifare != userInUse.UserMifare) //sikre sig at man ikke scanner udstyr der tilhøre én bruger og derefter scanner en anden bruger i samme omgang
                            {
                                MessageBox.Show("Du kan ikke scanne andre brugere end den udstyret tilhøre!");
                            }
                            else
                            {
                                isUserScanned = true;//Vi sætter isUserScanned for at sikre os at han ikke kan aflevere uden selv at være tilstede

                                userInUse = lendedObject.UserObject; //Vi sætter userInUse for at sikre os at man ikke scanner forskellige brugere sammen

                                PrintUserData(lendedObject); //Printer alle hans data og hans nuværende lån/arkiv

                                CommentCheck(lendedObject);//tjekker om der er nogle noter skrevet om personen
                            }
                        }

                    }
                    else //On Items Scanned
                    {
                        string userMifare = LendController.Instance.CheckIfLended(scannedItem.ItemMifare);//Tjekker om det item der er scanned er udlånt til en person, hvis der er udlånt skal vi hante alle hans data g hans lån/arkiv
                        TimeSpan timeSpanMonToThur = Settings.Default.TimeForReturnMonToThur; //Afleveringsdato for man-tors
                        TimeSpan timeSpanFri = Settings.Default.TimeForReturnFriday;//Afleveringsdato for fradag
                        LendObject scannedLendObject = null;

                        if (DateTime.Now.DayOfWeek == DayOfWeek.Friday)//Hvis der er valgt at afleveringsdatoen er fredag
                        {
                            scannedLendObject = new LendObject(scannedItem, DateTime.Now, datePickerReturn.SelectedDate.Value.Date + timeSpanFri, null);
                        }
                        else
                        {
                            scannedLendObject = new LendObject(scannedItem, DateTime.Now, datePickerReturn.SelectedDate.Value.Date + timeSpanMonToThur, null);
                        }

                        if (userMifare != "")//Hvis userMifare indeholder noget, er itemmet udlånt
                        {
                            //ScannedItem Er Udlånt
                            if (isItemsLended != false)//Tjekker at vi ikke scanne udlånt udstyr med ikk-udlånt udstyr
                            {
                                if (userInUse == null || userMifare == userInUse.UserMifare) //Tjekker at du ikke scanne udstyr der er udlånt til andre end den bruger der er i gang
                                {
                                    isItemsLended = true;
                                    if (!ScannedItemMifares.Contains(scannedItem.ItemMifare)) //tjekker at vi ikke scanner det samme mifare mere end én gang
                                    {
                                        LendedObject lendedObject = LendController.Instance.GetUserData(userMifare);//henter brugeren som itemmet er udlånt til's date, lån/arkiv

                                        foreach (LendObject lendObject in lendedObject.LendObjects)//giver det scanned udtyr de rigtige datoer, istedet for de datoer som der er valgt
                                        {
                                            if (lendObject.ItemObject.ItemMifare == scannedLendObject.ItemObject.ItemMifare)
                                            {
                                                scannedLendObject.ReturnDate = lendObject.ReturnDate;
                                                scannedLendObject.LendDate = lendObject.LendDate;
                                            }
                                        }

                                        PrintItemToList(scannedLendObject);//printer itemmet til listen over scannet items

                                        userInUse = lendedObject.UserObject;//Vi sætter userInUse for at sikre os at man ikke scanner forskellige brugere sammen
                                        if (lendedObject.UserObject == null)
                                        {
                                            MessageBox.Show("Brugeren der har lånt dette produkt kan ikke findes i databasen. Der kan stadig afleveres men du kan ikke se hvem der har haft lånt dette.");
                                        }
                                        else
                                        {
                                            PrintUserData(lendedObject);//Printer alle hans data og hans nuværende lån/arkiv

                                            CommentCheck(lendedObject);//tjekker om der er nogle noter skrevet om personen
                                        }
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
                            if (isItemsLended != true)//tjekker at du ikke scanner udlånt udstyr med ikke-udlånt udstyr
                            {
                                isItemsLended = false;
                                try
                                {
                                    if (!ScannedItemMifares.Contains(scannedItem.ItemMifare)) //Tjekker at du ikke scanner det samme mifare mere end én gang
                                    {
                                        if (userInUse != null)//Hvis vi har scannet en bruger
                                        {
                                            if (userInUse.HasPC && !userInUse.IsTeacher && scannedLendObject.ItemObject.Type == "Computer") //tjekker om han har en computer og om han er lærer
                                            {
                                                MessageBox.Show("Denne Bruger Har Allerede 1 Computer Og Er Ikke Lærer");
                                            }
                                            else
                                            {
                                                PrintItemToList(scannedLendObject);//printer itemmet til listen over scannet items
                                            }
                                        }
                                        else
                                        {
                                            PrintItemToList(scannedLendObject);//printer itemmet til listen over scannet items
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
                    TextBoxMain.Text = ""; //clear Main TextBox
                    TextBoxMain.Focus();//focuser Main TextBox
                    if (isItemsLended == true) //hvis vi har scannet et udstyr der er udlånt skal den disable ButtonLend
                    {
                        ButtonReturn.IsEnabled = true;
                        ButtonReturnWIthoutCard.IsEnabled = true;
                        ButtonLend.IsEnabled = false;
                    }
                    else//hvis vi har scannet et udstyr der ikke er udlånt skal den disable ButtonReturn
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
            ScannedItemMifares.Add(lendObject.ItemObject.ItemMifare);
            this.ListViewItems.Items.Add(new ListViewObject(lendObject.ItemObject.ItemMifare, lendObject.ItemObject.Type,
                lendObject.ItemObject.Manufacturer, lendObject.ItemObject.Model, lendObject.ItemObject.Id, lendObject.ItemObject.SerialNumber,
                lendObject.LendDate, lendObject.ReturnDate, lendObject.ReturnedDate, null, ""));
        }
        public void PrintUserData(LendedObject lendedObject)
        {

            LabelNameResult.Content = ($"{lendedObject.UserObject.FName} {lendedObject.UserObject.LName}");
            LabelNameResult.Visibility = Visibility.Visible;

            LabelZbcNameResult.Content = lendedObject.UserObject.ZbcName;
            LabelZbcNameResult.Visibility = Visibility.Visible;

            LabelPhoneResult.Content = lendedObject.UserObject.PhoneNumber;
            LabelPhoneResult.Visibility = Visibility.Visible;

            LabelTeacherResult.Content = (lendedObject.UserObject.IsTeacher) ? "Ja" : "Nej";

            LabelIsDisabledResult.Content = (userInUse.IsDisabled) ? "Ja" : "Nej";
            LabelIsDisabledResult.Foreground = (userInUse.IsDisabled) ? new SolidColorBrush(Colors.Red) : new SolidColorBrush(Colors.Green);

            LabelIsScannedResult.Content = (isUserScanned) ? "Ja" : "Nej";
            LabelIsScannedResult.Foreground = (isUserScanned) ? new SolidColorBrush(Colors.Green) : new SolidColorBrush(Colors.Red);

            //if (userInUse.IsDisabled)
            //{
            //    LabelIsDisabledResult.Content = "Ja";
            //    LabelIsDisabledResult.Foreground = new SolidColorBrush(Colors.Red);
            //}
            //else
            //{
            //    LabelIsDisabledResult.Content = "Nej";
            //    LabelIsDisabledResult.Foreground = new SolidColorBrush(Colors.Green);
            //}

            //if (isUserScanned)
            //{
            //    LabelIsScannedResult.Content = "Ja";
            //    LabelIsScannedResult.Foreground = new SolidColorBrush(Colors.Green);
            //}
            //else
            //{
            //    LabelIsScannedResult.Content = "Nej";
            //    LabelIsScannedResult.Foreground = new SolidColorBrush(Colors.Red);
            //}

            LabelTeacherResult.Visibility = Visibility.Visible;
            LabelIsDisabledResult.Visibility = Visibility.Visible;
            LabelIsScannedResult.Visibility = Visibility.Visible;

            ButtonComment.IsEnabled = true;

            this.ListViewLend.Items.Clear();

            foreach (LendObject lendObject in lendedObject.LendObjects)
            {
                bool? isOverdue = null;

                if (lendObject.ReturnDate.Date < DateTime.Now.Date && lendObject.ReturnedDate == null)
                {
                    isOverdue = true;
                }
                else if (lendObject.ReturnDate.Date > DateTime.Now.Date && lendObject.ReturnedDate == null)
                {
                    isOverdue = false;
                }
                else if (lendObject.ReturnDate.Date == DateTime.Now.Date && lendObject.ReturnedDate == null)
                {
                    if (lendObject.ReturnDate.TimeOfDay < DateTime.Now.TimeOfDay && lendObject.ReturnedDate == null)
                    {
                        isOverdue = true;
                    }
                    else if (lendObject.ReturnDate.TimeOfDay >= DateTime.Now.TimeOfDay && lendObject.ReturnedDate == null)
                    {
                        isOverdue = false;
                    }
                }
                this.ListViewLend.Items.Add(new ListViewObject(lendObject.ItemObject.ItemMifare, lendObject.ItemObject.Type,
                    lendObject.ItemObject.Manufacturer, lendObject.ItemObject.Model, lendObject.ItemObject.Id, lendObject.ItemObject.SerialNumber,
                    lendObject.LendDate, lendObject.ReturnDate, lendObject.ReturnedDate, isOverdue, ""));
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
                    if (scannedItem.ItemObject.Type == "Computer")
                    {
                        scannedItemsContainsComputer++;
                    }
                }
                if (!userInUse.IsTeacher && userInUse.HasPC && scannedItemsContainsComputer > 0)
                {
                    MessageBox.Show("OPS, udstyret blev IKKE udlånt! Brugeren har i forvejen 1 computer og denne bruger er ikke lærer");
                }
                else if (!userInUse.IsTeacher && !userInUse.HasPC && scannedItemsContainsComputer > 1)
                {
                    MessageBox.Show("OPS, udstyret blev IKKE udlånt! Der er scannet mere end 1 computer og denne bruger er ikke lærer");
                }
                else
                {
                    if (!Settings.Default.PartSmsService)
                    {
                        GenerateLend();
                    }
                    else
                    {
                        if (SmsController.Instance.GenerateVerificationSms(userInUse.PhoneNumber))
                        {
                            GenerateLend();
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
                MoveToArchive();
            }
            else
            {
                MessageBox.Show("Brugeren SKAL scannes for at kunne aflevere sit udstyr!");
            }
        }

        private void ButtonReturnWithoutCard_Click(object sender, RoutedEventArgs e)
        {
            MoveToArchive();
        }

        private void ButtonStat_Click(object sender, RoutedEventArgs e)
        {
            UIStat uiUserNote = new UIStat();
            uiUserNote.ShowDialog();
        }

        // Configuration Panel Button | Load up login page, to access Config Panel 
        private void ButtonConfig_Click(object sender, RoutedEventArgs e)
        {
            UIConfigPanelDetail configPanelLogin = new UIConfigPanelDetail();
            configPanelLogin.ShowDialog();
        }

        private void ButtonEditItem_Click(object sender, RoutedEventArgs e)
        {
            UIEditProduct uiEditProduct = new UIEditProduct();
            uiEditProduct.ShowDialog();
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
            ButtonReturnWIthoutCard.IsEnabled = false;
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
            if (lendedObject.UserObject.Comment != "")
            {
                MessageBox.Show(userInUse.Comment);
            }
        }

        private void ButtonDeleteItems_Click(object sender, RoutedEventArgs e)
        {
            foreach (ListViewObject listViewObject in ListViewItems.SelectedItems)
            {
                foreach (LendObject lendObject in scannedItems.ToList())
                {
                    if (lendObject.ItemObject.ItemMifare == listViewObject.ItemMifare)
                    {
                        scannedItems.Remove(lendObject);
                        ScannedItemMifares.Remove(lendObject.ItemObject.ItemMifare);
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
                this.ListViewItems.Items.Add(new ListViewObject(lendObject.ItemObject.ItemMifare, lendObject.ItemObject.Type,
                    lendObject.ItemObject.Manufacturer, lendObject.ItemObject.Model, lendObject.ItemObject.Id, lendObject.ItemObject.SerialNumber,
                    lendObject.LendDate, lendObject.ReturnDate, lendObject.ReturnedDate, null, ""));
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
                            if (lendObject.ItemObject.ItemMifare == listViewObject.ItemMifare)
                            {
                                lendObject.ReturnDate = datePickerReturn.SelectedDate.Value.Date;
                            }
                        }
                    }
                    RefreshListViewItems();
                }
            }
            TextBoxMain.Focus();
        }

        private void RestartScanner_Click(object sender, RoutedEventArgs e)
        {
            StartScanner();
        }

        private void StartScanner()
        {
            try
            {
                foreach (Process proc in Process.GetProcessesByName("javaw"))
                {
                    proc.Kill();
                }
                Thread.Sleep(100);
                Process p = new Process();
                p.StartInfo.FileName = Settings.Default.JavaPath;
                p.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void GenerateLend()
        {
            if (LendController.Instance.GenLendedObject(userInUse, scannedItems))
            {
                if (userInUse.PhoneNumber.ToString().Length < 8)
                {
                    MessageBox.Show("Udstyret er nu udlånt men personen har ikke et gyldigt nummer så ingen kvitering sendt");
                }
                else
                {
                    SmsController.Instance.GenerateLendReceipt(userInUse, scannedItems);
                    MessageBox.Show("Udstyret er nu udlånt og der er sendt en kvitering til personen via SMS");
                }

                ClearUI();
            }
            else
            {
                MessageBox.Show("OPS, udstyret blev IKKE udlånt! Hvis dette fortsætter, kontakt IT.");
            }
        }

        private void MoveToArchive()
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

        // Get Informations about the system, and the client system.
        public void GetSystemDetails()
        {
            // ################################################# Udlaan System Build Version #################################################
            // Get Instance of Assembly File, Get assembly version
            Version version = Assembly.GetExecutingAssembly().GetName().Version;

            // Display AssemblyVersion in SystemVersion x:Name applied by Index {0}.{1}.{2}.{3}
            SystemVersion.Text = String.Format(SystemVersion.Text, version.Major, version.Minor, version.Build, version.Revision);

            // Debug - Assembly Version
            //Debug.Print("########################### VERSION ########################### " + String.Format(SystemVersion.Text, version.Major, version.Minor, version.Build, version.Revision));

            // ################################################# Operative System Version #################################################
            // Display Current Operative System
            OSVersion.Text = String.Format(OSVersion.Text, Environment.OSVersion.VersionString);

            // Debug - OS Version
            //Debug.Print("########################### OS VERSION ###########################" + Environment.OSVersion.VersionString);

            // ################################################# Location of the System | Ringsted | Roskilde | Næstved | Vordingborg #################################################
            // Instance Settings File
            Settings settings = new Settings();

            // Look up Foreach Item in the Settings List
            foreach (SettingsProperty item in settings.Properties)
            {
                // Look all items up which contains Location.
                if (item.Name.Contains("Location"))
                {
                    // Look if the item is the same as System.Boolean - means True or False
                    //Debug.Print("########################### LOCATION ###########################" + item.Name);
                    if (item.PropertyType.ToString() == "System.Boolean")
                    {
                        // Write out everything that has the True value.
                        //Debug.Print("########################### Is Bool ########################### " + item.Name);
                        if (item.DefaultValue.ToString().Equals("True"))
                        {
                            // Remove "Location", in the string, one char at a time.
                            //Debug.Print("########################### Is True ###########################" + item.Name);
                            string str = item.Name.Remove(0, 8);

                            // Write out Location that is true in the MainWindow.xaml
                            SystemLocationConnection.Text = String.Format(SystemLocationConnection.Text, str);
                        }
                        else
                        {
                            // If the item is False - We remove the "Location", chars too.
                            string falseStr = item.Name.Remove(0, 8);

                            //Debug.Print("FALSE " + falseStr);
                        }
                    }
                }
                else
                {
                    //Debug.Print("########################### OTHER ###########################" + item.Name);
                }
            }            
        }

        private void CloseProgram_Click(object sender, RoutedEventArgs e)
        {
            CloseProgram();
        }

        private void CloseProgram()
        {
            Debug.Print("########################### User Closed Program ###########################");
            Environment.Exit(0);
        }
    }
}
