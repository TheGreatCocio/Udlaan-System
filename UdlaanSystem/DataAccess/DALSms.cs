using System;
using System.Diagnostics;
using System.Net;

namespace UdlaanSystem.DataAccess
{
    class DALSms
    {
        public string key = "2e9a42f3-1353-46e2-ba61-3ef81c5d8693";

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

        public void SendSms(int receiver, string msg)
        {
                var json = new WebClient().DownloadString("https://data.efif.dk/JSON/SMS.ashx?key=" + key + "&receivers=" + receiver + "&message=" + msg + "");

                Debug.WriteLine(json);
        }
    }
}
