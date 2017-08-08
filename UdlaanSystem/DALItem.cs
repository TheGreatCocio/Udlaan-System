using MySql.Data.MySqlClient;
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
            sqlConn = @"server=10.108.48.19; Database=supply_ri; User Id=udlaan; Password=RFIDrules; integrated security=false";

            if(MysqlConnection == null)
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
    }
}
