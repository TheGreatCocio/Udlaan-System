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
                Debug.WriteLine("###Failed to connect to sql server: " + ex);
            }
        }

        public ItemObject GetItemByMifare(string mifare)
        {
            ItemObject item = null;
            try
            {
                ConnectMySql();
                MySqlCommand cmd = new MySqlCommand("SELECT items.item_mifare, types.type_name, manufacturers.manufacturer_name, models.model_name, items.item_id, items.item_serialnumber FROM items JOIN types ON types.type_id = items.item_type JOIN manufacturers ON manufacturers.manufacturer_id = items.item_manufacturer JOIN models ON models.model_id = items.item_model WHERE items.item_mifare = '" + mifare + "'", MysqlConnection);
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    item = new ItemObject(rdr.GetString(0), rdr.GetString(1), rdr.GetString(2), rdr.GetString(3), rdr.GetInt16(4), rdr.GetString(5));
                }
            }
            finally
            {

                MysqlConnection.Close();
            }
            return item;
        }

        public List<string[]> GetItemTypes()
        {
            List<string[]> types = null;
            string[] type = null;

            try
            {
                ConnectMySql();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM types", MysqlConnection);
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    type[1] = rdr.GetString(0);
                    type[2] = rdr.GetString(1);
                    types.Add(type);
                }
            }
            finally
            {
                MysqlConnection.Close();
            }

            return types;
        }
    }
}
