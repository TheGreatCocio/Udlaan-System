using MySql.Data.MySqlClient;
using MySql.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UdlaanSystem
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
            if (Settings1.Default.LocationNæstved == true)
            {

            }
            else if (Settings1.Default.LocationRingsted == true)
            {
                sqlConn = @"server=10.108.48.19; Database=supply_ri; User Id=udlaan; Password=RFIDrules; integrated security=false";
            }
            else if (Settings1.Default.LocationRoskilde == true)
            {

            }
            else if (Settings1.Default.LocationVordingborg == true)
            {

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
                MySqlCommand cmd = new MySqlCommand("SELECT items.item_mifare, types.type_name, models.model_name, manufacturers.manufacturer_name, items.item_id, items.item_serialnumber FROM items JOIN types ON items.item_type = types.type_id JOIN manufacturers ON items.item_manufacturer = manufacturers.manufacturer_id JOIN models ON items.item_model = models.model_id WHERE items.item_mifare = '" + mifare +  "'", MysqlConnection);
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

        public List<string[]> getItemManufacturers(int typeID)
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

        public List<string[]> getItemModels(int manufacturerID, int typeID)
        {
            List<string[]> models = new List<string[]>();

            try
            {
                ConnectMySql();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM models WHERE model_selected_type = '" + typeID + "' AND model_selected_manufacturer = '" + manufacturerID + "'", MysqlConnection);
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

        public List<int> RetrieveIdInformation (int type, int manufacturer, int model)
        {
            List<int> id = new List<int>();    

            try
            {
                ConnectMySql();

                MySqlCommand cmd = new MySqlCommand("SELECT item_id FROM items WHERE item_type = '" + type + "' AND item_manufacturer = '" + manufacturer + "' AND item_model = '" + model + "' ORDER BY `items`.`item_id` ASC", MysqlConnection);
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
                    MySqlCommand cmd = new MySqlCommand("INSERT INTO items (item_mifare, item_type, item_manufacturer, item_model, item_id, item_serialnumber) VALUES ('" + item.itemMifare + "', '" + Convert.ToInt16(item.type) + "', '" + Convert.ToInt16(item.manufacturer) + "', '" + Convert.ToInt16(item.model) + "', '" + item.id + "', '" + item.serialNumber + "')", MysqlConnection);
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
    }
}
