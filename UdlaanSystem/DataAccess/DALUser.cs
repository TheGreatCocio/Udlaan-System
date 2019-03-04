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
using UdlaanSystem.Properties;

namespace UdlaanSystem.DataAccess
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
                MySqlCommand cmd = new MySqlCommand("SELECT user_mifare, user_fname, user_lname, user_zbcname, user_phonenumber, user_isdisabled, user_isteacher, user_comment FROM users WHERE user_mifare = '" + userMifare + "'", MysqlConnection);
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    userObject = new UserObject(rdr.GetString(1), rdr.GetString(2), rdr.GetString(3), rdr.GetString(0), rdr.GetInt32(4), Convert.ToBoolean(rdr.GetInt16(6)), false, Convert.ToBoolean(rdr.GetInt16(5)), rdr.GetString(7));
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
                MySqlCommand cmd = new MySqlCommand("SELECT user_mifare, user_fname, user_lname, user_zbcname, user_phonenumber, user_isdisabled, user_isteacher, user_comment FROM users WHERE user_zbcname = '" + zbcName + "'", MysqlConnection);
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    userObject = new UserObject(rdr.GetString(1), rdr.GetString(2), rdr.GetString(3), rdr.GetString(0), rdr.GetInt32(4), Convert.ToBoolean(rdr.GetInt16(6)), false, Convert.ToBoolean(rdr.GetInt16(5)), rdr.GetString(7));
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
                MySqlCommand cmd = new MySqlCommand("INSERT INTO users (user_mifare, user_fname, user_lname, user_zbcname, user_phonenumber, user_isdisabled, user_isteacher) VALUES ('" + userObject.UserMifare + "', '" + userObject.FName + "', '" + userObject.LName + "', '" + userObject.ZbcName + "', '" + userObject.PhoneNumber + "', '" + userObject.IsDisabled + "', '" + userObject.IsTeacher + "')", MysqlConnection);
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
                MySqlCommand cmd = new MySqlCommand("UPDATE users SET user_mifare = '" + userObject.UserMifare + "', user_fname = '" + userObject.FName + "', user_lname = '" + userObject.LName + "', user_zbcname = '" + userObject.ZbcName + "', user_phonenumber = '" + userObject.PhoneNumber + "', user_isdisabled = " + userObject.IsDisabled + ", user_isteacher = " + userObject.IsTeacher + " WHERE user_zbcname = '" + userObject.ZbcName + "'", MysqlConnection);
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
        public void UpdateUserNote (string note, string mifare)
        {
            try
            {
                ConnectMySql();
                MySqlCommand cmd = new MySqlCommand("UPDATE users SET user_comment = '" + note +  "' WHERE user_mifare = '" + mifare + "'", MysqlConnection);
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

        public string GetUserNote(string mifare)
        {
            string userNote = "";
            try
            {
                ConnectMySql();
                MySqlCommand cmd = new MySqlCommand("SELECT user_comment FROM users WHERE user_mifare = '" + mifare + "'", MysqlConnection);
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    userNote = rdr.GetString(0);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("############################FAILED TO UPDATE USER IN DB: " + ex);
            }
            finally
            {
                MysqlConnection.Close();
            }
            return userNote;
        }

        public bool IsUser(string mifare)
        {
            bool isUser = false;

                try
                {
                    ConnectMySql();
                    MySqlCommand cmd = new MySqlCommand("SELECT user_mifare FROM users WHERE user_mifare = '" + mifare + "'", MysqlConnection);
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        isUser = true;
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

            return isUser;
        }

        public List<UserObject> GetUserStatInformation()
        {
            List<UserObject> userList = new List<UserObject>();

            try
            {
                ConnectMySql();
                MySqlCommand cmd = new MySqlCommand("SELECT user_fname, user_lname, user_zbcname, user_mifare, user_phonenumber, user_isteacher, user_isdisabled, user_comment FROM lend JOIN users ON lend.lend_usermifare = users.user_mifare GROUP BY lend.lend_usermifare", MysqlConnection);
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    userList.Add(new UserObject(rdr.GetString(0), rdr.GetString(1), rdr.GetString(2), rdr.GetString(3), rdr.GetInt32(4), Convert.ToBoolean(rdr.GetInt16(5)), false, Convert.ToBoolean(rdr.GetInt16(6)), rdr.GetString(7)));
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

            return userList;
        }

        public bool UpdateUserZbcNameToUniLogin(string zbcName, string uniLogin)
        {
            try
            {
                ConnectMySql();
                MySqlCommand cmd = new MySqlCommand("UPDATE users SET user_zbcname = '" + uniLogin + "' WHERE user_zbcname = '" + zbcName + "'", MysqlConnection);
                cmd.ExecuteNonQuery();

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