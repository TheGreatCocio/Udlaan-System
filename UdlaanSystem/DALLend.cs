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
            sqlConn = @"server=10.108.48.19; Database=supply_ri; User Id=udlaan; Password=RFIDrules; integrated security=false";

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
    }

}
