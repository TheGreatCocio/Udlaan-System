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
using UdlaanSystem.Managers;

namespace UdlaanSystem.Views
{
    /// <summary>
    /// Interaction logic for UIEditProduct.xaml
    /// </summary>
    public partial class UIEditProduct : Window
    {
        List<string[]> types = new List<string[]>();
        int selectedTypeID;

        List<string[]> manufacturers = new List<string[]>();
        int selectedManufacturerID;

        List<string[]> models = new List<string[]>();
        int selectedModelID;

        ItemObject item = null;

        public UIEditProduct()
        {
            InitializeComponent();
            CreateTypeList();
        }

        public void CreateTypeList()
        {
            types = ItemController.Instance.GetItemTypes();
            foreach (string[] arrayStr in types)
            {
                ComboBoxTypes.Items.Add(arrayStr[1]);
                Debug.WriteLine("############################: " + arrayStr[1]);
            }
        }

        private void ComboBoxTypes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedManufacturerID = 0;
            TextboxID.Text = "";
            string selectedTypeName = ComboBoxTypes.SelectedItem.ToString();
            foreach (string[] arrayStr in types)
            {
                if (arrayStr.Contains(selectedTypeName))
                {
                    selectedTypeID = Convert.ToInt16(arrayStr[0]);
                }
            }
            if (!selectedTypeID.Equals(1))
            {
                TextboxSerialNumber.IsEnabled = false;
                TextboxSerialNumber.Clear();
            }
            else
            {
                TextboxSerialNumber.IsEnabled = true;
                TextboxSerialNumber.Clear();
            }
            CreateManufacturerList(selectedTypeID);
        }

        public void CreateManufacturerList(int typeID)
        {
            ComboBoxModels.Items.Clear();
            ComboBoxManufacturers.Items.Clear();
            manufacturers = ItemController.Instance.GetItemManufacturers(typeID);
            foreach (string[] arrayStr in manufacturers)
            {
                ComboBoxManufacturers.Items.Add(arrayStr[1]);
                Debug.WriteLine("############################: " + arrayStr[1]);
            }
        }

        private void ComboBoxManufacturer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!ComboBoxManufacturers.Items.IsEmpty)
            {
                string selectedManufacturerName = ComboBoxManufacturers.SelectedItem.ToString();

                foreach (string[] arrayStr in manufacturers)
                {
                    if (arrayStr.Contains(selectedManufacturerName))
                    {
                        selectedManufacturerID = Convert.ToInt16(arrayStr[0]);
                    }
                }
                selectedModelID = 0;
                CreateModelList(selectedManufacturerID, selectedTypeID);
            }
        }

        public void CreateModelList(int manufacturerID, int typeID)
        {            
            ComboBoxModels.Items.Clear();
            models = ItemController.Instance.GetItemModels(manufacturerID, typeID);
            foreach (string[] arrayStr in models)
            {
                ComboBoxModels.Items.Add(arrayStr[1]);
                Debug.WriteLine("############################: " + arrayStr[1]);
            }
        }

        private void ComboBoxModels_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!ComboBoxModels.Items.IsEmpty)
            {
                string selectedModelsName = ComboBoxModels.SelectedItem.ToString();

                foreach (string[] arrayStr in models)
                {
                    if (arrayStr.Contains(selectedModelsName))
                    {
                        selectedModelID = Convert.ToInt16(arrayStr[0]);
                    }
                }
            }
        }

        private void ButtonSearch_Click(object sender, RoutedEventArgs e)
        {
            if (selectedModelID == 0 || selectedManufacturerID == 0 || selectedTypeID == 0 || TextboxID.Text == "")
            {
                MessageBox.Show("Du skal udfylde alting inden du søger!");
            }
            else
            {
                if (selectedTypeID == 1 && TextboxSerialNumber.Text == "")
                {
                    MessageBox.Show("Da det er en computer du skal ændre i, bedes du venligst indtaste Serie Nummer som står bag på (efter S/N)");
                }
                else
                {
                    try
                    {
                        item = new ItemObject("", selectedTypeID.ToString(), selectedManufacturerID.ToString(), selectedModelID.ToString(), Convert.ToInt16(TextboxID.Text), TextboxSerialNumber.Text);
                        if (ItemController.Instance.FindItemByID(item))
                        {
                            LabelFound.Visibility = Visibility.Visible;
                            LabelFound.Foreground = new SolidColorBrush(Colors.Green);
                            LabelFound.Content = "Fundet!";
                        }
                        else
                        {
                            LabelFound.Visibility = Visibility.Visible;
                            LabelFound.Foreground = new SolidColorBrush(Colors.Red);
                            LabelFound.Content = "Ikke Fundet!";
                        }
                        
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Noget Gik Galt!");
                    }
                }
            }
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            if (item == null ||TextboxNewMifare.Text == "")
            {
                MessageBox.Show("Nyt mifare skal være scannet og produkt skal være fundet!");
            }
            else
            {
                item.ItemMifare = TextboxNewMifare.Text;
                if (ItemController.Instance.UpdateMifareOnItem(item))
                {
                    MessageBox.Show("Mifare er nu opdateret");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Noget Gik Galt! Produktet blev ikke indsat");
                }
            }
            
        }
    }
}
