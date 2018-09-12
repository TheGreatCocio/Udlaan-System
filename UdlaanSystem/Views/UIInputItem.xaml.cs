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

        //public void CardInserted(string hex)
        //{
        //    Application.Current.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, new Action(() => textBoxItemMifare.Text = hex));
        //    System.Windows.Forms.SendKeys.SendWait("{ENTER}");
        //}

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
            ClearBoxes();
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
                ClearBoxes();
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
                ClearBoxes();
            }
        }

        private void BtnAddItem_Click(object sender, RoutedEventArgs e)
        {
            ItemObject itemToAdd = null;
            bool goodToGo = true;
            if (ItemController.Instance.CheckIfMifareIsItem(textBoxItemMifare.Text) == null)
            {
                if (itemsToInsert.Any())
                {
                    foreach (ItemObject itemMifare in itemsToInsert)
                    {
                        if (itemMifare.ItemMifare != textBoxItemMifare.Text)
                        {
                            goodToGo = true;
                        }
                        else
                        {
                            MessageBox.Show("Dette Produkt Er Allerede I Listen");
                            goodToGo = false;
                            ClearBoxes();
                            textBoxItemMifare.Focus();
                        }
                    }

                    if (ComboBoxTypes.SelectedItem.Equals("Computer") && goodToGo)
                    {
                        itemToAdd = new ItemObject(textBoxItemMifare.Text, selectedTypeID.ToString(), selectedManufacturerID.ToString(), selectedModelID.ToString(), Convert.ToInt16(textBoxID.Text), textBoxSerialNumber.Text);
                        if (!itemsToInsert.Contains(itemToAdd))
                        {
                            this.ListViewAddItems.Items.Add(new ItemObject(textBoxItemMifare.Text, ComboBoxTypes.SelectedItem.ToString(), ComboBoxManufacturers.SelectedItem.ToString(), ComboBoxModels.SelectedItem.ToString(), Convert.ToInt16(textBoxID.Text), textBoxSerialNumber.Text));
                            itemsToInsert.Add(itemToAdd);
                        }
                        else
                        {
                            MessageBox.Show("Dette Mifare Er Allerede I Brug!");
                        }
                        ClearBoxes();
                        textBoxItemMifare.Focus();
                    }
                    else if (goodToGo)                    
                    {
                        foreach (ItemObject item in itemsToInsert)
                        {
                            if (!listOfIds.Contains(item.Id) && ItemController.Instance.GetItemModelName(Convert.ToUInt16(item.Model)) == ComboBoxModels.SelectedItem.ToString())
                            {
                                listOfIds.Add(item.Id);
                            }
                        }
                        itemToAdd = new ItemObject(textBoxItemMifare.Text, selectedTypeID.ToString(), selectedManufacturerID.ToString(), selectedModelID.ToString(), ItemController.Instance.CalculateNextID(selectedModelID, listOfIds), textBoxSerialNumber.Text);
                        if (!itemsToInsert.Contains(itemToAdd))
                        {
                            UIShowID bigIdBox = new UIShowID(itemToAdd.Id);
                            bigIdBox.ShowDialog();
                            this.ListViewAddItems.Items.Add(new ItemObject(textBoxItemMifare.Text, ComboBoxTypes.SelectedItem.ToString(), ComboBoxManufacturers.SelectedItem.ToString(), ComboBoxModels.SelectedItem.ToString(), itemToAdd.Id, ""));
                            itemsToInsert.Add(itemToAdd);
                        }
                        else
                        {
                            MessageBox.Show("Dette Mifare Er Allerede I Brug!");
                        }
                        ClearBoxes();
                        listOfIds.Clear();
                        textBoxItemMifare.Focus();
                    }
                }                
                else
                {
                    if (ComboBoxTypes.SelectedItem.Equals("Computer"))
                    {
                        itemToAdd = new ItemObject(textBoxItemMifare.Text, selectedTypeID.ToString(), selectedManufacturerID.ToString(), selectedModelID.ToString(), Convert.ToInt16(textBoxID.Text), textBoxSerialNumber.Text);
                        if (!itemsToInsert.Contains(itemToAdd))
                        {
                            this.ListViewAddItems.Items.Add(new ItemObject(textBoxItemMifare.Text, ComboBoxTypes.SelectedItem.ToString(), ComboBoxManufacturers.SelectedItem.ToString(), ComboBoxModels.SelectedItem.ToString(), Convert.ToInt16(textBoxID.Text), textBoxSerialNumber.Text));
                            itemsToInsert.Add(itemToAdd);
                        }
                        else
                        {
                            MessageBox.Show("Dette Mifare Er Allerede I Brug!");
                        }
                        ClearBoxes();
                        textBoxItemMifare.Focus();
                    }
                    else
                    {
                        foreach (ItemObject item in itemsToInsert)
                        {
                            if (!listOfIds.Contains(item.Id) && ItemController.Instance.GetItemModelName(Convert.ToUInt16(item.Model)) == ComboBoxModels.SelectedItem.ToString())
                            {
                                listOfIds.Add(item.Id);
                            }
                        }
                        itemToAdd = new ItemObject(textBoxItemMifare.Text, selectedTypeID.ToString(), selectedManufacturerID.ToString(), selectedModelID.ToString(), ItemController.Instance.CalculateNextID(selectedModelID, listOfIds), textBoxSerialNumber.Text);
                        if (!itemsToInsert.Contains(itemToAdd))
                        {
                            UIShowID bigIdBox = new UIShowID(itemToAdd.Id);
                            bigIdBox.ShowDialog();
                            this.ListViewAddItems.Items.Add(new ItemObject(textBoxItemMifare.Text, ComboBoxTypes.SelectedItem.ToString(), ComboBoxManufacturers.SelectedItem.ToString(), ComboBoxModels.SelectedItem.ToString(), itemToAdd.Id, ""));
                            itemsToInsert.Add(itemToAdd);

                        }
                        else
                        {
                            MessageBox.Show("Dette Mifare Er Allerede I Brug!");
                        }
                        ClearBoxes();
                        listOfIds.Clear();
                        textBoxItemMifare.Focus();
                    }
                }
            }
            else
            {
                MessageBox.Show("Dette Produkt Er Allerede I DataBasen");
                ClearBoxes();
                textBoxItemMifare.Focus();
            }
        }

        private void btnAddAllItemsToDB_Click(object sender, RoutedEventArgs e)
        {
            if (ItemController.Instance.InsertItems(itemsToInsert))
            {
                MessageBox.Show("Det Lykkedes!");
                this.Close();
            }
            else
            {
                MessageBox.Show("Der Skete En Fejl, Kontakt Venligst IT-Afdelingen");
            }
        }

        private void BtnDeleteSelectedItems_Click(object sender, RoutedEventArgs e)
        {
            for(int i = 0; i < ListViewAddItems.Items.Count; i++)
            {
                foreach (ItemObject itemObject in itemsToInsert.ToList())
                {
                    ItemObject TESTITEM = ListViewAddItems.Items[i] as ItemObject;
                    
                    if (ListViewAddItems.SelectedItems.Contains(TESTITEM))
                    {
                        if (itemObject.ItemMifare == TESTITEM.ItemMifare)
                        {
                            itemsToInsert.Remove(itemObject);
                            ListViewAddItems.Items.RemoveAt(i);
                        }
                    }
                }
            }
        }

        private void textBoxMifareInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && textBoxItemMifare.Text != "" )
            {
                if (textBoxSerialNumber.IsEnabled && textBoxSerialNumber.Text != "" && textBoxID.IsEnabled && textBoxID.Text != "")
                {
                    BtnAddItem_Click(this, new RoutedEventArgs());
                }
                else if (!textBoxSerialNumber.IsEnabled && !textBoxID.IsEnabled)
                {
                    BtnAddItem_Click(this, new RoutedEventArgs());
                }
            }
        }

        private void ClearBoxes()
        {
            textBoxSerialNumber.Clear();
            textBoxItemMifare.Clear();
            textBoxID.Clear();
        }
    }
}
