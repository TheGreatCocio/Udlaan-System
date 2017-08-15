using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.DirectoryServices.AccountManagement;

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
                Debug.WriteLine("############################FAILED: " + ex);
            }
            finally
            {
                MysqlConnection.Close();
            }

            return userObject;
        }

        public string[] GetUserByZbcNameAD(string zbcName)
        {
            string[] temp = new string[2];
            try
            {
                DirectoryContext con = new DirectoryContext(DirectoryContextType.Domain, "efif.dk", "zbc-maskinnavn", "Zorro.b.c");
                DomainController dc = DomainController.FindOne(con);
                DirectorySearcher sercher = dc.GetDirectorySearcher();
                sercher.Filter = "(&(objectCategory=person)(CN=" + zbcName + "))";
                SearchResult findUser = sercher.FindOne();
                DirectoryEntry user = findUser.GetDirectoryEntry();
                temp[0] = (String)user.Properties["givenName"].Value;
                temp[1] = (String)user.Properties["sn"].Value;
            }
            catch (Exception)
            {
                temp[0] = "";
                temp[1] = "";
                return temp;
                //System.Diagnostics.Debug.WriteLine(ex);
            }
            return temp;
        }

        public UserObject GetUserByZbcName(string zbcName)
        {
            UserObject userObject = null;
            try
            {
                ConnectMySql();
                MySqlCommand cmd = new MySqlCommand("SELECT user_mifare, user_fname, user_lname, user_zbcname, user_phonenumber, user_isdisabled, user_isteacher FROM users WHERE user_zbcname = '" + zbcName + "'", MysqlConnection);
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    userObject = new UserObject(rdr.GetString(1), rdr.GetString(2), rdr.GetString(3), rdr.GetString(0), rdr.GetInt32(4), Convert.ToBoolean(rdr.GetInt16(6)), false, Convert.ToBoolean(rdr.GetInt16(5)));
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
            return userObject;
        }

        public void AddUserInDB(UserObject userObject)
        {
            try
            {
                ConnectMySql();
                MySqlCommand cmd = new MySqlCommand("INSERT INTO users (user_mifare, user_fname, user_lname, user_zbcname, user_phonenumber, user_isdisabled, user_isteacher) VALUES ('" + userObject.userMifare + "', '" + userObject.fName + "', '" + userObject.lName + "', '" + userObject.zbcName + "', '" + userObject.phoneNumber + "', '" + userObject.isDisabled + "', '" + userObject.isTeacher + "')", MysqlConnection);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("############################FAILED TO ADD USER IN DB: " + ex);
            }
            finally
            {
                MysqlConnection.Close();
            }
        }

        public void UpdateUserInDB(UserObject userObject)
        {
            try
            {
                ConnectMySql();
                MySqlCommand cmd = new MySqlCommand("UPDATE users SET user_mifare = '" + userObject.userMifare + "', user_fname = '" + userObject.fName + "', user_lname = '" + userObject.lName + "', user_zbcname = '" + userObject.zbcName + "', user_phonenumber = '" + userObject.phoneNumber + "', user_isdisabled = " + userObject.isDisabled + ", user_isteacher = " + userObject.isTeacher + " WHERE user_zbcname = '" + userObject.zbcName + "'", MysqlConnection);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("############################FAILED TO UPDATE USER IN DB: " + ex);
            }
            finally
            {
                MysqlConnection.Close();
            }
        }
    }
}
