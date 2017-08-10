using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UdlaanSystem
{
    class DALUser
    {
        public DALUser()
        {
        }
        private static DALUser instance;

        public static DALUser Instance
        {
            get
            {
                if (instance == null)
                { instance = new DALUser(); }
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

        public UserObject GetUserByMifare(string userMifare)
        {
            UserObject userObject = null;

            try
            {
                ConnectMySql();
                MySqlCommand cmd = new MySqlCommand("SELECT user_mifare, user_fname, user_lname, user_zbcname, user_phonenumber, user_isdisabled, user_isteacher FROM users WHERE user_mifare = '" + userMifare + "'", MysqlConnection);
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    userObject = new UserObject(rdr.GetString(1), rdr.GetString(2), rdr.GetString(3), rdr.GetString(0), rdr.GetInt32(4), Convert.ToBoolean(rdr.GetInt16(6)), false, Convert.ToBoolean(rdr.GetInt16(5)));
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

            return userObject;
        }
    }
}
