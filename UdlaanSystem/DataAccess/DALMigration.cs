using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UdlaanSystem.Properties;

namespace UdlaanSystem.DataAccess
{
    class DALMigration
    { //Denne del af koden er UDELUKKENDE til mmigration så udstyret bliver afleveret i den gamle DB!!!
        public DALMigration()
        {
        }
        private static DALMigration instance;

        public static DALMigration Instance
        {
            get
            {
                if (instance == null)
                { instance = new DALMigration(); }
                return instance;
            }
        }

        private string sqlConn;
        private MySqlConnection MysqlConnection = null;

        private void ConnectMySql()
        {
            // Test Connection
            if (Settings.Default.LocationTestdb == true)
            {
                sqlConn = @"server=10.108.48.19; Database=supply_testdb; User Id=developer; Password=jZrQV6+cfsjq;persistsecurityinfo=True;port=3306;SslMode=none;";
            }

            // Næstved Connection
            else if (Settings.Default.LocationNæstved == true)
            {
                sqlConn = @"server=10.108.48.19; Database=supply_nv; User Id=udlaan; Password=RFIDrules;persistsecurityinfo=True;port=3306;SslMode=none;";
            }

            // Ringsted Connection
            else if (Settings.Default.LocationRingsted == true)
            {
                sqlConn = @"server=10.108.48.19; Database=supply_ri; User Id=udlaan; Password=RFIDrules;persistsecurityinfo=True;port=3306;SslMode=none;";
            }

            // Roskilde Connection
            else if (Settings.Default.LocationRoskilde == true)
            {
                sqlConn = @"server=10.108.48.19; Database=supply_ro; User Id=udlaan; Password=RFIDrules;persistsecurityinfo=True;port=3306;SslMode=none;";
            }

            // Vordingborg Connection
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
                Debug.WriteLine($"####################Failed to connect to sql server: {ex}");
            }
        }

        public bool CheckIfItemIsLendedInOldDB(string itemMifare)
        {
            bool itemIslended = false;

            try
            {
                ConnectMySql();
                MySqlCommand cmd = new MySqlCommand($"SELECT pc_mifare FROM udlaant WHERE pc_mifare = '{itemMifare}', {MysqlConnection}");
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    itemIslended = true;
                }
            }
            catch (Exception ex)
            {
                MysqlConnection.Close();
                Debug.WriteLine($"############################FAILED TO CHECK OLD DB: {ex}");
            }
            finally
            {
                MysqlConnection.Close();
            }

            return itemIslended;
        }

        public bool ReturnItemInOldDB(string itemMifareToReturnInOldDB)
        {
            try
            {
                ConnectMySql();
                MySqlCommand cmd = new MySqlCommand();
                if (Settings.Default.LocationRingsted == true)
                {
                    cmd = new MySqlCommand($"CALL removeBorrower('{itemMifareToReturnInOldDB}', '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}'), {MysqlConnection}");
                }
                else if (Settings.Default.LocationRoskilde == true)
                {
                    cmd = new MySqlCommand($"DELETE FROM udlaant WHERE pc_mifare = '{itemMifareToReturnInOldDB}', {MysqlConnection}");
                }
                
                cmd.ExecuteNonQuery();
                
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("BOOOOOOOOOOOOOOOOOOOOOOOM: " + ex);
                return false;
                throw;
            }
            finally
            {
                MysqlConnection.Close();
            }
        }
    }
}