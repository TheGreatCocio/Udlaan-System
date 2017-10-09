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
                    if (item.returnDate.Date == DateTime.Now.Date)
                    {
                        this.listViewStatToday.Items.Add(new ListViewObject(item.itemObject.itemMifare, item.itemObject.type, item.itemObject.manufacturer, item.itemObject.model, item.itemObject.id, item.itemObject.serialNumber, item.lendDate, item.returnDate, null, false, stat.UserObject.zbcName));
                    }
                    else if (item.returnDate.Date < DateTime.Now.Date)
                    {
                        this.listViewStatAllTime.Items.Add(new ListViewObject(item.itemObject.itemMifare, item.itemObject.type, item.itemObject.manufacturer, item.itemObject.model, item.itemObject.id, item.itemObject.serialNumber, item.lendDate, item.returnDate, null, false, stat.UserObject.zbcName));
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
                    if (selectedItem.itemMifare == item.itemObject.itemMifare)
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
                    if (selectedItem.itemMifare == item.itemObject.itemMifare)
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
