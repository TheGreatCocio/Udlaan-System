using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UdlaanSystem
{
    class LendController
    {
        public LendController()
        {
        }
        private static LendController instance;

        public static LendController Instance
        {
            get
            {
                if (instance == null)
                { instance = new LendController(); }
                return instance;
            }
        }

        public string CheckIfLended (string itemMifare) {

            string userMifare = DALLend.Instance.GetLendedByItemMifare(itemMifare);

            return userMifare;

        }

        public LendedObject GetLendedUserData (string userMifare) {

            return null;
        }
    }
}
