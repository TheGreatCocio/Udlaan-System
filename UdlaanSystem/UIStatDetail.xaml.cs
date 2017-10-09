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

            labelFnameResult.Content = selectedItem.UserObject.fName;
            labelFnameResult.Visibility = Visibility.Visible;
            labelLnameResult.Content = selectedItem.UserObject.lName;
            labelLnameResult.Visibility = Visibility.Visible;
            labelZbcnameResult.Content = selectedItem.UserObject.zbcName;
            labelZbcnameResult.Visibility = Visibility.Visible;
            labelPhoneNumberResult.Content = selectedItem.UserObject.phoneNumber;
            labelPhoneNumberResult.Visibility = Visibility.Visible;

            if (selectedItem.UserObject.isTeacher)
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
                labelTypeResult.Content = information.itemObject.type;
                labelTypeResult.Visibility = Visibility.Visible;
                labelManufacturerResult.Content = information.itemObject.manufacturer;
                labelManufacturerResult.Visibility = Visibility.Visible;
                labelModelResult.Content = information.itemObject.model;
                labelModelResult.Visibility = Visibility.Visible;
                labelSerialNumberResult.Content = information.itemObject.serialNumber;
                labelSerialNumberResult.Visibility = Visibility.Visible;
                labelIDResult.Content = information.itemObject.id;
                labelIDResult.Visibility = Visibility.Visible;
                labelLendDateResult.Content = information.lendDate.ToString("dd-MM-yyyy HH:mm");
                labelLendDateResult.Visibility = Visibility.Visible;
                labelReturnDateResult.Content = information.returnDate.ToString("dd-MM-yyyy HH:mm");
                labelReturnDateResult.Visibility = Visibility.Visible;
            }
        }

        private void ButtonOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
