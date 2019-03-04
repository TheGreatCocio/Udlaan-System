using System.Diagnostics;
using System.Net;

namespace UdlaanSystem.DataAccess
{
    internal class DALSms
    {
        public string smsKey = "2e9a42f3-1353-46e2-ba61-3ef81c5d8693";
        public string url = "https://data.efif.dk/JSON/SMS.ashx?key=";

        public DALSms()
        {
        }
        private static DALSms instance;

        public static DALSms Instance
        {
            get
            {
                if (instance == null)
                { instance = new DALSms(); }
                return instance;
            }
        }

        #region Send SMS to reciever
        public void SendSms(int receiver, string msg)
        {
            /* Making a string interpolation with the url and key, matched closely together. */
            var json = new WebClient().DownloadString($"{url}{smsKey}&receivers={receiver}&message={msg}");

            Debug.WriteLine(json);
        } 
        #endregion
    }
}
