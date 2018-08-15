using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UdlaanSystem
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
            if (Settings1.Default.LocationNæstved == true)
            {
                sqlConn = @"server=10.108.48.19; Database=pc_udlaan_nv; User Id=udlaan; Password=RFIDrules;integrated security=false";
            }
            else if (Settings1.Default.LocationRingsted == true)
            {
                sqlConn = @"server=10.108.48.19; Database=pc_udlaan_ri; User Id=udlaan; Password=RFIDrules;integrated security=false";
            }
            else if (Settings1.Default.LocationRoskilde == true)
            {
                sqlConn = @"server=10.108.48.19; Database=pc_udlaan_ro; User Id=udlaan; Password=RFIDrules;integrated security=false";
            }
            else if (Settings1.Default.LocationVordingborg == true)
            {
                sqlConn = @"server=10.108.48.19; Database=pc_udlaan_vb; User Id=udlaan; Password=RFIDrules;integrated security=false";
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

        public bool CheckIfItemIsLendedInOldDB(string itemMifare)
        {
            bool itemIslended = false;

            try
            {
                ConnectMySql();
                MySqlCommand cmd = new MySqlCommand("SELECT pc_mifare FROM udlaant WHERE pc_mifare = '" + itemMifare + "'", MysqlConnection);
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    itemIslended = true;
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

            return itemIslended;
        }

        public bool ReturnItemInOldDB(string itemMifareToReturnInOldDB)
        {
            try
            {
                ConnectMySql();
                
                MySqlCommand cmd = new MySqlCommand("CALL removeBorrower('" + itemMifareToReturnInOldDB + "', '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')", MysqlConnection);
                cmd.ExecuteNonQuery();
                
                return true;
            }
            catch (Exception)
            {
                Debug.WriteLine("BOOOOOOOOOOOOOOOOOOOOOOOM");
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
