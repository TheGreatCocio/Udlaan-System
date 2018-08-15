using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace UdlaanSystem
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
