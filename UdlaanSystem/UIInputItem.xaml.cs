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
        List<string[]> types = new List<string[]>();
        int selectedTypeID;

        List<string[]> manufacturers = new List<string[]>();
        int selectedManufacturerID;

        List<string[]> models = new List<string[]>();
        int selectedModelID;

        List<ItemObject> itemsToInsert = new List<ItemObject>();
        List<int> listOfIds = new List<int>();
        List<string[]> typesOnItemsToInsert = new List<string[]>();


        public UIInputItem()
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
                textBoxID.IsEnabled = false;
                textBoxSerialNumber.IsEnabled = false;
            }
            else
            {
                textBoxID.IsEnabled = true;
                textBoxSerialNumber.IsEnabled = true;
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

        private void BtnAddItem_Click(object sender, RoutedEventArgs e)
        {
            ItemObject itemToAdd = null;

            if (ComboBoxTypes.SelectedItem.Equals("Computer"))
            {
                itemToAdd = new ItemObject(textBoxItemMifare.Text, selectedTypeID.ToString(), selectedManufacturerID.ToString(), selectedModelID.ToString(), Convert.ToInt16(textBoxID.Text), textBoxSerialNumber.Text);
                this.ListViewAddItems.Items.Add(new ItemObject(textBoxItemMifare.Text, ComboBoxTypes.SelectedItem.ToString(), ComboBoxManufacturers.SelectedItem.ToString(), ComboBoxModels.SelectedItem.ToString(), Convert.ToInt16(textBoxID.Text), textBoxSerialNumber.Text));
                itemsToInsert.Add(itemToAdd);
            }
            else
            {
                foreach (ItemObject item in itemsToInsert)
                {
                    if (!listOfIds.Contains(item.id) || item.model == ComboBoxModels.SelectedItem.ToString())
                    {
                        listOfIds.Add(item.id);
                    }
                }
                itemToAdd = new ItemObject(textBoxItemMifare.Text, selectedTypeID.ToString(), selectedManufacturerID.ToString(), selectedModelID.ToString(), ItemController.Instance.CalculateNextID(selectedTypeID, selectedManufacturerID, selectedModelID, listOfIds) , textBoxSerialNumber.Text);
                this.ListViewAddItems.Items.Add(new ItemObject(textBoxItemMifare.Text, ComboBoxTypes.SelectedItem.ToString(), ComboBoxManufacturers.SelectedItem.ToString(), ComboBoxModels.SelectedItem.ToString(),itemToAdd.id, ""));
                itemsToInsert.Add(itemToAdd);
            }
        }

        private void btnAddAllItemsToDB_Click(object sender, RoutedEventArgs e)
        {
            if (ItemController.Instance.InsertItems(itemsToInsert))
            {
                MessageBox.Show("Success!!");
            }
            else
            {
                MessageBox.Show("Fuck!!");
            }
        }
    }
}
