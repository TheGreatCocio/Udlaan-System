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
    /// Interaction logic for UIInputItem.xaml
    /// </summary>
    public partial class UIInputItem : Window
    {
        public UIInputItem()
        {
            InitializeComponent();


        }

        public void CreateTypeList()
        {
            ComboBoxTypes.SelectedValue = "FISK";
            ItemController.Instance.GetItemTypes();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
