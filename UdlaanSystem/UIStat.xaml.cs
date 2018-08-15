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
    /// Interaction logic for UIStat.xaml
    /// </summary>
    public partial class UIStat : Window
    {
        List<LendedObject> statList = LendController.Instance.GetStatisticsInformation();

        public UIStat()
        {
            InitializeComponent();

            foreach (LendedObject stat in statList)
            {
                foreach (LendObject item in stat.LendObjects)
                {
                    if (item.ReturnDate.Date == DateTime.Now.Date)
                    {
                        this.listViewStatToday.Items.Add(new ListViewObject(item.ItemObject.ItemMifare, item.ItemObject.Type, item.ItemObject.Manufacturer, item.ItemObject.Model, item.ItemObject.Id, item.ItemObject.SerialNumber, item.LendDate, item.ReturnDate, null, false, stat.UserObject.ZbcName));
                    }
                    else if (item.ReturnDate.Date < DateTime.Now.Date)
                    {
                        this.listViewStatAllTime.Items.Add(new ListViewObject(item.ItemObject.ItemMifare, item.ItemObject.Type, item.ItemObject.Manufacturer, item.ItemObject.Model, item.ItemObject.Id, item.ItemObject.SerialNumber, item.LendDate, item.ReturnDate, null, false, stat.UserObject.ZbcName));
                    }
                }

            }
        }

        public void ListViewStatAllTime_ItemSelectionChanged(object sender, EventArgs e)
        {
            ListViewObject selectedItem = listViewStatAllTime.SelectedItem as ListViewObject;

            foreach (LendedObject statObject in statList)
            {
                foreach (LendObject item in statObject.LendObjects)
                {
                    if (selectedItem.ItemMifare == item.ItemObject.ItemMifare)
                    {
                        CallStatDetails(statObject);
                    }
                }

            }
        }

        public void ListViewStatToday_ItemSelectionChanged(object sender, EventArgs e)
        {
            ListViewObject selectedItem = listViewStatToday.SelectedItem as ListViewObject;

            foreach (LendedObject statObject in statList)
            {
                foreach (LendObject item in statObject.LendObjects)
                {
                    if (selectedItem.ItemMifare == item.ItemObject.ItemMifare)
                    {
                        CallStatDetails(statObject);
                    }
                }

            }
        }

        private void CallStatDetails(LendedObject selectedItem)
        {
            UIStatDetail uIStatDetail = new UIStatDetail(selectedItem);
            uIStatDetail.ShowDialog();
        }
    }
}
