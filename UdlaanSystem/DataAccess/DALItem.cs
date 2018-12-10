using MySql.Data.MySqlClient;
using MySql.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UdlaanSystem.Properties;

namespace UdlaanSystem.DataAccess
{
    class DALItem
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

        private string sqlConn;
        private MySqlConnection MysqlConnection = null;

        private void ConnectMySql()
        {
            if (Settings.Default.LocationTestdb == true)
            {
                sqlConn = @"server=10.108.48.19; Database=supply_testdb; User Id=developer; Password=jZrQV6+cfsjq;persistsecurityinfo=True;port=3306;SslMode=none;";
            }
            else if (Settings.Default.LocationNæstved == true)
            {
                sqlConn = @"server=10.108.48.19; Database=supply_nv; User Id=udlaan; Password=RFIDrules;persistsecurityinfo=True;port=3306;SslMode=none;";
            }
            else if (Settings.Default.LocationRingsted == true)
            {
                sqlConn = @"server=10.108.48.19; Database=supply_ri; User Id=udlaan; Password=RFIDrules;persistsecurityinfo=True;port=3306;SslMode=none;";
            }
            else if (Settings.Default.LocationRoskilde == true)
            {
                sqlConn = @"server=10.108.48.19; Database=supply_ro; User Id=udlaan; Password=RFIDrules;persistsecurityinfo=True;port=3306;SslMode=none;";
            }
            else if (Settings.Default.LocationVordingborg == true)
            {
                sqlConn = @"server=10.108.48.19; Database=supply_vb; User Id=udlaan; Password=RFIDrules;persistsecurityinfo=True;port=3306;SslMode=none;";
            }

            if (MysqlConnection == null)
            {
                MysqlConnection = new MySqlConnection(sqlConn);
            }
            try
            {
                MysqlConnection.Open();
            }
            catch(Exception ex)
            {
                Debug.WriteLine("####################Failed to connect to sql server: " + ex);
            }
        }

        public ItemObject GetItemByMifare(string mifare)
        {
            ItemObject item = null;

            try
            {
                ConnectMySql();
                MySqlCommand cmd = new MySqlCommand("SELECT items.item_mifare, types.type_name, manufacturers.manufacturer_name, models.model_name, items.item_id, items.item_serialnumber FROM items JOIN types ON items.item_type = types.type_id JOIN manufacturers ON items.item_manufacturer = manufacturers.manufacturer_id JOIN models ON items.item_model = models.model_id WHERE items.item_mifare = '" + mifare +  "'", MysqlConnection);
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    item = new ItemObject(rdr.GetString(0), rdr.GetString(1), rdr.GetString(2), rdr.GetString(3), rdr.GetInt16(4), rdr.GetString(5));
                }
            }
            catch (Exception ex)
            {
                MysqlConnection.Close();
                Debug.WriteLine("############################FAILED: " + ex);
            }
            finally
            {
                MysqlConnection.Close();
            }

            return item;
        }

        public List<string[]> GetItemTypes()
        {
            List<string[]> types = new List<string[]>();

            try
            {
                ConnectMySql();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM types", MysqlConnection);
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
            catch(Exception ex)
            {
                MysqlConnection.Close();
                Debug.WriteLine("############################FAILED: " + ex);
            }
            finally
            {
                MysqlConnection.Close();
            }

            return types;
        }

        public List<string[]> GetItemManufacturers(int typeID)
        {
            List<string[]> manufacturers = new List<string[]>();

            try
            {
                ConnectMySql();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM manufacturers WHERE manufacturer_selected_type = '" + typeID + "'", MysqlConnection);
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
                MysqlConnection.Close();
                Debug.WriteLine("############################FAILED: " + ex);
            }
            finally
            {
                MysqlConnection.Close();
            }

            return manufacturers;
        }

        public List<string[]> GetItemModels(int manufacturerID, int typeID)
        {
            List<string[]> models = new List<string[]>();

            try
            {
                ConnectMySql();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM models WHERE model_selected_type = '" + typeID + "' AND model_selected_manufacturer = '" + manufacturerID + "' ORDER BY model_name", MysqlConnection);
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    string[] model = new string[2];
                    model[0] = rdr.GetInt32(0).ToString();
                    model[1] = rdr.GetString(1);
                    models.Add(model);
                    Debug.WriteLine("############################ Success!");
                }
            }
            catch (Exception ex)
            {
                MysqlConnection.Close();
                Debug.WriteLine("############################FAILED: " + ex);
            }
            finally
            {
                MysqlConnection.Close();
            }

            return models;
        }

        public List<int> RetrieveIdInformation (int model)
        {
            List<int> id = new List<int>();    

            try
            {
                ConnectMySql();

                MySqlCommand cmd = new MySqlCommand("SELECT item_id FROM items WHERE item_model = '" + model + "' ORDER BY `items`.`item_id` ASC", MysqlConnection);
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    id.Add(rdr.GetInt16(0));
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("############################FAILED: " + ex);
            }
            finally
            {
                MysqlConnection.Close();
            }
            return id;
        }

        public bool InsertItemsIntoDB(List<ItemObject> itemsToInsert) 
        {
            try
            {
                ConnectMySql();
                foreach (ItemObject item in itemsToInsert)
                {
                    MySqlCommand cmd = new MySqlCommand("INSERT INTO items (item_mifare, item_type, item_manufacturer, item_model, item_id, item_serialnumber) VALUES ('" + item.ItemMifare + "', '" + Convert.ToInt16(item.Type) + "', '" + Convert.ToInt16(item.Manufacturer) + "', '" + Convert.ToInt16(item.Model) + "', '" + item.Id + "', '" + item.SerialNumber + "')", MysqlConnection);
                    Debug.WriteLine(cmd.ToString());
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("############################FAILED: " + ex);
                return false;
            }
            finally
            {
                MysqlConnection.Close();
            }
            return true;
        }

        public string GetItemModelName(int modelID)
        {
            string modelName = "";

            try
            {
                ConnectMySql();
                MySqlCommand cmd = new MySqlCommand("SELECT model_name FROM models WHERE model_id = '" + modelID + "'", MysqlConnection);
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    modelName = rdr.GetString(0);

                    Debug.WriteLine("############################ Success!");
                }
            }
            catch (Exception ex)
            {
                MysqlConnection.Close();
                Debug.WriteLine("############################FAILED: " + ex);
            }
            finally
            {
                MysqlConnection.Close();
            }

            return modelName;
        }

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
                ConnectMySql();
                MySqlCommand cmd = new MySqlCommand($"SELECT * FROM items WHERE item_type = {item.Type} " +
                    $"AND item_manufacturer = {item.Manufacturer} AND item_model = {item.Model} AND item_id = {item.Id}{statement}", MysqlConnection);
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    wentThrough = true;
                }
            }
            catch (Exception ex)
            {
                MysqlConnection.Close();
                Debug.WriteLine("############################FAILED: " + ex);
                wentThrough = false;
            }
            finally
            {
                MysqlConnection.Close();
            }

            return wentThrough;
        }

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
                ConnectMySql();
                MySqlCommand cmd = new MySqlCommand($"UPDATE items SET item_mifare = {item.ItemMifare} WHERE item_type = {item.Type} " +
                    $"AND item_manufacturer = {item.Manufacturer} AND item_model = {item.Model} AND item_id = {item.Id}{statement}", MysqlConnection);
                if (cmd.ExecuteNonQuery() > 0)
                {
                    wentThrough = true;
                }
            }
            catch (Exception ex)
            {
                MysqlConnection.Close();
                Debug.WriteLine("############################FAILED: " + ex);
                wentThrough = false;
            }
            finally
            {
                MysqlConnection.Close();
            }

            return wentThrough;
        }
    }
}
