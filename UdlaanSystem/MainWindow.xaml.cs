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

        private void onMyfareScanned(object sender, KeyEventArgs e)// Runs when a key is pressed.
        {
            if (TextBoxMain.IsFocused) // checks if the maintextbox is focused
            {
                if (e.Key == Key.Return) // checks if its the enter button that has been pressed
                {
                    ItemObject item = ItemController.Instance.CheckIfMifareIsItem(TextBoxMain.Text);

                    List<ItemObject> items = new List<ItemObject>();
                    items.Add(new ItemObject() { itemMifare = item.itemMifare, type = item.type, manufacturer = item.manufacturer, model = item.model, ID = 0, serialNumber = "asdf" });
                    items.Add(new User() { Name = "Jane Doe", Age = 39 });
                    items.Add(new User() { Name = "Sammy Doe", Age = 13 });


                    //ListViewItems.ItemsSource


                }
            }
        }


        private void ButtonItem_Click(object sender, RoutedEventArgs e)
        {
            UIInputItem inputItembox = new UIInputItem();
            inputItembox.ShowDialog();
        }

        /*private string ButtonUser_Click(object sender, RoutedEventArgs e)
        {
            UIInputUser inputUserBox = new UIInputUser();
        }*/
    }
}
