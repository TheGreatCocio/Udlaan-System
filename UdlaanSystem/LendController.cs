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

        public void GetLendedUserData (string userMifare) {
            UserObject uerObject = UserController.Instance.GetUserObject(userMifare);
            List<LendObject> lendObjectList = DALLend.Instance.GetLendedByUserMifare(userMifare).Concat(DALLend.Instance.GetArchiveByUserMifare(userMifare)).ToList();
            
        }
    }
}
