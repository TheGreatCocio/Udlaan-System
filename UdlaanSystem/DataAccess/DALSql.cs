using System;
using MySql.Data.MySqlClient;
using System.Diagnostics;
using UdlaanSystem.Properties;

namespace UdlaanSystem.DataAccess
{
    class DALSql
    {
        public DALSql() {
        }

        private static DALSql instance;

        public static DALSql Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DALSql();
                }
                return instance;
            }
        }

        private string sqlConn;
        public MySqlConnection mySqlConnection = null;

        #region ConnectMySql
        public void ConnectMySql()
        {
            /* Developer Database Connection */
            #region Developer Database
            if (Settings.Default.LocationTestdb == true)
            {
                sqlConn = @"server=10.108.48.19; Database=supply_testdb; User Id=developer; Password=jZrQV6+cfsjq;persistsecurityinfo=True;port=3306;SslMode=none;";
            }
            #endregion

            /* Næstved Database Connection */
            #region Næstved Database
            else if (Settings.Default.LocationNæstved == true)
            {
                sqlConn = @"server=10.108.48.19; Database=supply_nv; User Id=udlaan; Password=RFIDrules;persistsecurityinfo=True;port=3306;SslMode=none;";
            }
            #endregion

            /* Ringsted Database Connection */
            #region Næstved Database
            else if (Settings.Default.LocationRingsted == true)
            {
                sqlConn = @"server=10.108.48.19; Database=supply_ri; User Id=udlaan; Password=RFIDrules;persistsecurityinfo=True;port=3306;SslMode=none;";
            }
            #endregion

            /* Roskilde Database Connection */
            #region Roskilde Database
            else if (Settings.Default.LocationRoskilde == true)
            {
                sqlConn = @"server=10.108.48.19; Database=supply_ro; User Id=udlaan; Password=RFIDrules;persistsecurityinfo=True;port=3306;SslMode=none;";
            }
            #endregion

            /* Vordingborg Database Connection */
            #region Vordingborg Database
            else if (Settings.Default.LocationVordingborg == true)
            {
                sqlConn = @"server=10.108.48.19; Database=supply_vb; User Id=udlaan; Password=RFIDrules;persistsecurityinfo=True;port=3306;SslMode=none;";
            } 
            #endregion

            if (mySqlConnection == null)
            {
                mySqlConnection = new MySqlConnection(sqlConn);
            }
            try
            {
                mySqlConnection.Open();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("####################Failed to connect to sql server: " + ex);
            }
        }
        #endregion ConnectMySql
    }
}
