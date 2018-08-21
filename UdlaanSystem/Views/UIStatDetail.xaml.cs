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
    /// Interaction logic for UIStatDetail.xaml
    /// </summary>
    public partial class UIStatDetail : Window
    {
        public UIStatDetail()
        {
            InitializeComponent();
        }

        public UIStatDetail(LendedObject selectedItem)
        {
            InitializeComponent();

            labelFnameResult.Content = selectedItem.UserObject.FName;
            labelFnameResult.Visibility = Visibility.Visible;
            labelLnameResult.Content = selectedItem.UserObject.LName;
            labelLnameResult.Visibility = Visibility.Visible;
            labelZbcnameResult.Content = selectedItem.UserObject.ZbcName;
            labelZbcnameResult.Visibility = Visibility.Visible;
            labelPhoneNumberResult.Content = selectedItem.UserObject.PhoneNumber;
            labelPhoneNumberResult.Visibility = Visibility.Visible;

            if (selectedItem.UserObject.IsTeacher)
            {
                labelIsTeacherResult.Content = "Ja";
            }
            else
            {
                labelIsTeacherResult.Content = "Nej";
            }
            labelIsTeacherResult.Visibility = Visibility.Visible;

            foreach (LendObject information in selectedItem.LendObjects)
            {
                labelTypeResult.Content = information.ItemObject.Type;
                labelTypeResult.Visibility = Visibility.Visible;
                labelManufacturerResult.Content = information.ItemObject.Manufacturer;
                labelManufacturerResult.Visibility = Visibility.Visible;
                labelModelResult.Content = information.ItemObject.Model;
                labelModelResult.Visibility = Visibility.Visible;
                labelSerialNumberResult.Content = information.ItemObject.SerialNumber;
                labelSerialNumberResult.Visibility = Visibility.Visible;
                labelIDResult.Content = information.ItemObject.Id;
                labelIDResult.Visibility = Visibility.Visible;
                labelLendDateResult.Content = information.LendDate.ToString("dd-MM-yyyy HH:mm");
                labelLendDateResult.Visibility = Visibility.Visible;
                labelReturnDateResult.Content = information.ReturnDate.ToString("dd-MM-yyyy HH:mm");
                labelReturnDateResult.Visibility = Visibility.Visible;
            }
        }

        private void ButtonOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
