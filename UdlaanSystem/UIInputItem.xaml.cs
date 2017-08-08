using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            CreateTypeList();

        }

        public void CreateTypeList()
        {
            List<string[]> types = ItemController.Instance.GetItemTypes();
            foreach (string[] arrayStr in types)
            {
                ComboBoxTypes.Items.Add(arrayStr[1]);
                Debug.WriteLine("############################: " + arrayStr[1]);
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }
    }
}
