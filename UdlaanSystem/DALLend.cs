using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace UdlaanSystem
{
    class DALLend
    {
        public DALLend()
        {
        }
        private static DALLend instance;

        public static DALLend Instance
        {
            get
            {
                if (instance == null)
                { instance = new DALLend(); }
                return instance;
            }
        }

        private string sqlConn;
        private MySqlConnection MysqlConnection = null;

        private void ConnectMySql()
        {
            sqlConn = @"server=10.108.48.19; Database=supply_ri; User Id=udlaan; Password=RFIDrules;integrated security=false";

            if (MysqlConnection == null)
            {
                MysqlConnection = new MySqlConnection(sqlConn);
            }
            try
            {
                MysqlConnection.Open();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("####################Failed to connect to sql server: " + ex);
            }
        }

        public string GetLendedByItemMifare (string itemMifare)
        {
            string userMifare = "";

            try
            {
                ConnectMySql();
                MySqlCommand cmd = new MySqlCommand("SELECT lend.lend_usermifare FROM lend WHERE lend.lend_itemmifare = '" + itemMifare + "'", MysqlConnection);
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    userMifare = rdr.GetString(0);
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

            return userMifare;
        }

        public List<LendObject> GetLendedByUserMifare(string userMifare)
        {
            List<LendObject> lendObjectList = new List<LendObject>();
            try
            {
                ConnectMySql();
                MySqlCommand cmd = new MySqlCommand("SELECT lend.lend_itemmifare, lend.lend_usermifare, lend.lend_lenddate, lend.lend_returndate, types.type_name, models.model_name, manufacturers.manufacturer_name, items.item_id, items.item_serialnumber FROM lend JOIN items ON lend.lend_itemmifare = items.item_mifare JOIN types ON items.item_type = types.type_id JOIN manufacturers ON items.item_manufacturer = manufacturers.manufacturer_id JOIN models ON items.item_model = models.model_id WHERE lend.lend_usermifare = '" + userMifare + "'", MysqlConnection);
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    lendObjectList.Add(new LendObject(new ItemObject(rdr.GetString(0), rdr.GetString(4), rdr.GetString(6), rdr.GetString(5), rdr.GetInt16(7), rdr.GetString(8)), rdr.GetDateTime(2), rdr.GetDateTime(3), null));
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
            return lendObjectList;
        }

        public List<LendObject> GetArchiveByUserMifare(string userMifare)
        {
            List<LendObject> lendObjectList = new List<LendObject>();
            try
            {
                ConnectMySql();
                MySqlCommand cmd = new MySqlCommand("SELECT archive.archive_itemmifare, archive.archive_usermifare, archive.archive_lenddate, archive.archive_returndate, types.type_name, models.model_name, manufacturers.manufacturer_name, items.item_id, items.item_serialnumber, archive.archive_returneddate FROM archive JOIN items ON archive.archive_itemmifare = items.item_mifare JOIN types ON items.item_type = types.type_id JOIN manufacturers ON items.item_manufacturer = manufacturers.manufacturer_id JOIN models ON items.item_model = models.model_id WHERE archive.archive_usermifare = '" + userMifare + "'", MysqlConnection);
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    lendObjectList.Add(new LendObject(new ItemObject(rdr.GetString(0), rdr.GetString(4), rdr.GetString(6), rdr.GetString(5), rdr.GetInt16(7), rdr.GetString(8)), rdr.GetDateTime(2), rdr.GetDateTime(3), rdr.GetDateTime(9)));
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
            return lendObjectList;
        }

        public bool AddLendedObjectToLend(LendedObject lendedObjectToAddToDB)
        {
            bool success = false;
            foreach (LendObject lendObject in lendedObjectToAddToDB.LendObjects)
            {
                try
                {
                    ConnectMySql();
                    MySqlCommand cmd = new MySqlCommand("INSERT INTO lend (lend_itemmifare, lend_usermifare, lend_lenddate, lend_returndate) VALUES ('" + lendObject.itemObject.itemMifare + "', '" + lendedObjectToAddToDB.UserObject.userMifare + "', '" + FormatDateBackEnd(lendObject.lendDate.ToString()) + "', '" + FormatDateBackEnd(lendObject.returnDate.ToString()) + "')", MysqlConnection);
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                    
                    }
                    success = true;
                }
                catch (Exception ex)
                {
                    MysqlConnection.Close();
                    Debug.WriteLine("############################FAILED: " + ex);
                    success = false;
                }
                finally
                {
                    MysqlConnection.Close();
                }
            }
            return success;
        }

        private string FormatDateBackEnd(string date)
        {
            DateTime temp = Convert.ToDateTime(date);
            return temp.ToString("yyyy-MM-dd HH:mm:ss");
        }
    }

}
