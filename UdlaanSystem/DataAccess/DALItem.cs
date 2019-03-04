using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace UdlaanSystem.DataAccess
{
    internal class DALItem
    {
        public DALItem()
        {
        }
        private static DALItem instance;

        public static DALItem Instance
        {
            get
            {
                if (instance == null)
                { instance = new DALItem(); }
                return instance;
            }
        }

        /* Object instance of Sql Connection */
        private DALSql SqlConnection = new DALSql();

        #region GetItemByMifare
        public ItemObject GetItemByMifare(string mifare)
        {
            ItemObject item = null;

            try
            {
                SqlConnection.ConnectMySql();
                MySqlCommand cmd = new MySqlCommand($"SELECT items.item_mifare, types.type_name, manufacturers.manufacturer_name, models.model_name, items.item_id, items.item_serialnumber FROM items JOIN types ON items.item_type = types.type_id JOIN manufacturers ON items.item_manufacturer = manufacturers.manufacturer_id JOIN models ON items.item_model = models.model_id WHERE items.item_mifare = '{mifare}', {SqlConnection.mySqlConnection}");
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    item = new ItemObject(rdr.GetString(0), rdr.GetString(1), rdr.GetString(2), rdr.GetString(3), rdr.GetInt16(4), rdr.GetString(5));
                }
            }
            catch (Exception ex)
            {
                SqlConnection.mySqlConnection.Close();
                Debug.WriteLine($"############################FAILED Getting item by mifare id: {ex}");
            }
            finally
            {
                SqlConnection.mySqlConnection.Close();
            }

            return item;
        }
        #endregion GetItemByMifare

        #region GetItemTypes
        public List<string[]> GetItemTypes()
        {
            List<string[]> types = new List<string[]>();

            try
            {
                SqlConnection.ConnectMySql();
                MySqlCommand cmd = new MySqlCommand($"SELECT * FROM types, {SqlConnection.mySqlConnection}");
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    string[] type = new string[2];
                    type[0] = rdr.GetInt32(0).ToString();
                    type[1] = rdr.GetString(1);
                    types.Add(type);
                    Debug.WriteLine("############################ Success!");
                }
            }
            catch (Exception ex)
            {
                SqlConnection.mySqlConnection.Close();
                Debug.WriteLine("############################FAILED: " + ex);
            }
            finally
            {
                SqlConnection.mySqlConnection.Close();
            }

            return types;
        }
        #endregion GetItemTypes

        #region GetItemManufacturers
        public List<string[]> GetItemManufacturers(int typeID)
        {
            /* Instanciate List of manufactors with a string array */ 
            List<string[]> manufacturers = new List<string[]>();

            try
            {
                SqlConnection.ConnectMySql();
                MySqlCommand cmd = new MySqlCommand($"SELECT * FROM manufacturers WHERE manufacturer_selected_type = '{typeID}', {SqlConnection.mySqlConnection}");
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    string[] manufacturer = new string[2];
                    manufacturer[0] = rdr.GetInt32(0).ToString();
                    manufacturer[1] = rdr.GetString(1);
                    manufacturers.Add(manufacturer);
                    Debug.WriteLine("############################ Success!");
                }
            }
            catch (Exception ex)
            {
                SqlConnection.mySqlConnection.Close();
                Debug.WriteLine("############################FAILED: " + ex);
            }
            finally
            {
                SqlConnection.mySqlConnection.Close();
            }

            return manufacturers;
        }
        #endregion GetItemManufacturers

        #region GetItemModels
        public List<string[]> GetItemModels(int manufacturerID, int typeID)
        {
            /* Instanciate list of Item Models, with a string array. */
            List<string[]> models = new List<string[]>();

            try
            {
                SqlConnection.ConnectMySql();
                MySqlCommand cmd = new MySqlCommand($"SELECT * FROM models WHERE model_selected_type = '{typeID}' AND model_selected_manufacturer = '{manufacturerID}' ORDER BY model_name;, {SqlConnection.mySqlConnection}");
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    string[] model = new string[2];
                    model[0] = rdr.GetInt32(0).ToString();
                    model[1] = rdr.GetString(1);
                    models.Add(model);
                    Debug.WriteLine($"############################ Success!");
                }
            }
            catch (Exception ex)
            {
                SqlConnection.mySqlConnection.Close();
                Debug.WriteLine($"############################ FAILED GetItemModels: {ex}");
            }
            finally
            {
                SqlConnection.mySqlConnection.Close();
            }

            return models;
        }
        #endregion GetItemModels

        #region RetrieveIdInformation
        public List<int> RetrieveIdInformation(int model)
        {
            List<int> id = new List<int>();

            try
            {
                SqlConnection.ConnectMySql();
                MySqlCommand cmd = new MySqlCommand($"SELECT item_id FROM items WHERE item_model = '{model}' ORDER BY `items`.`item_id` ASC;, {SqlConnection.mySqlConnection}");
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    id.Add(rdr.GetInt16(0));
                }
            }
            catch (Exception ex)
            {
                SqlConnection.mySqlConnection.Close();
                Debug.WriteLine("############################FAILED: " + ex);
            }
            finally
            {
                SqlConnection.mySqlConnection.Close();
            }
            return id;
        }
        #endregion RetrieveIdInformation

        #region InsertItemsIntoDB
        public bool InsertItemsIntoDB(List<ItemObject> itemsToInsert)
        {
            try
            {
                SqlConnection.ConnectMySql();
                /* Foreach scanned item in the object, insert them into the database. */
                foreach (ItemObject item in itemsToInsert)
                {
                    MySqlCommand cmd = new MySqlCommand($"INSERT INTO items (item_mifare, item_type, item_manufacturer, item_model, item_id, item_serialnumber) VALUES ('{item.ItemMifare}', '{ Convert.ToInt16(item.Type) }', '{ Convert.ToInt16(item.Manufacturer)}', '{ Convert.ToInt16(item.Model)}', '{ item.Id }', '{item.SerialNumber}');, {SqlConnection.mySqlConnection}");
                    Debug.WriteLine(cmd.ToString());
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                SqlConnection.mySqlConnection.Close();
                Debug.WriteLine("############################FAILED: " + ex);
                return false;
            }
            finally
            {
                SqlConnection.mySqlConnection.Close();
            }
            return true;
        }
        #endregion InsertItemsIntoDB

        #region GetItemModelName
        public string GetItemModelName(int modelID)
        {
            string modelName = "";

            try
            {
                SqlConnection.ConnectMySql();
                MySqlCommand cmd = new MySqlCommand($"SELECT model_name FROM models WHERE model_id = '{modelID}', {SqlConnection.mySqlConnection}");
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    modelName = rdr.GetString(0);

                    Debug.WriteLine("############################ Success!");
                }
            }
            catch (Exception ex)
            {
                SqlConnection.mySqlConnection.Close();
                Debug.WriteLine("############################FAILED: " + ex);
            }
            finally
            {
                SqlConnection.mySqlConnection.Close();
            }

            return modelName;
        }
        #endregion

        #region FindItemByID
        public bool FindItemByID(ItemObject item)
        {
            string statement = string.Empty;
            bool wentThrough = false;
            if (item.Type.Equals("Computer"))
            {
                statement = ($" AND item_serialnumber = '{item.SerialNumber}'");
            }
            try
            {
                SqlConnection.ConnectMySql();
                MySqlCommand cmd = new MySqlCommand($"SELECT * FROM items WHERE item_type = {item.Type} AND item_manufacturer = {item.Manufacturer} AND item_model = {item.Model} AND item_id = {item.Id}{statement}, {SqlConnection.mySqlConnection}");
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    wentThrough = true;
                }
            }
            catch (Exception ex)
            {
                SqlConnection.mySqlConnection.Close();
                Debug.WriteLine("############################FAILED: " + ex);
                wentThrough = false;
            }
            finally
            {
                SqlConnection.mySqlConnection.Close();
            }

            return wentThrough;
        }
        #endregion

        #region UpdateMifareOnItem
        public bool UpdateMifareOnItem(ItemObject item)
        {
            string statement = string.Empty;
            bool wentThrough = false;
            if (item.Type.Equals("1"))
            {
                statement = ($" AND item_serialnumber = '{item.SerialNumber}'");
            }
            try
            {
                SqlConnection.ConnectMySql();
                MySqlCommand cmd = new MySqlCommand($"UPDATE items SET item_mifare = {item.ItemMifare} WHERE item_type = {item.Type} AND item_manufacturer = {item.Manufacturer} AND item_model = {item.Model} AND item_id = {item.Id}{statement}, {SqlConnection.mySqlConnection}");
                if (cmd.ExecuteNonQuery() > 0)
                {
                    wentThrough = true;
                }
            }
            catch (Exception ex)
            {
                SqlConnection.mySqlConnection.Close();
                Debug.WriteLine("############################FAILED: " + ex);
                wentThrough = false;
            }
            finally
            {
                SqlConnection.mySqlConnection.Close();
            }

            return wentThrough;
        } 
        #endregion
    }
}
