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
        public UIStat()
        {
            InitializeComponent();

            List<ListViewObject> statList = LendController.Instance.GetStatInformation();

            foreach (ListViewObject stat in statList)
            {
                if (stat.returnDate.Date == DateTime.Now.Date)
                {
                    this.listViewStatToday.Items.Add(stat);
                }
                else if (stat.returnDate.Date < DateTime.Now.Date)
                {
                    this.listViewStatAllTime.Items.Add(stat);
                }
            }
        }
    }
}
