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

        public LendedObject GetUserData (string userMifare) {
            UserObject userObject = UserController.Instance.GetUserObject(userMifare);
            List<LendObject> lendObjectList = DALLend.Instance.GetLendedByUserMifare(userMifare).Concat(DALLend.Instance.GetArchiveByUserMifare(userMifare)).ToList();
            foreach (LendObject obj in lendObjectList)
            {
                if (obj.returnedDate == null && obj.itemObject.type == "Computer")
                {
                    userObject.hasPC = true;
                }
            }
            LendedObject lendedObject = new LendedObject(userObject, lendObjectList);
            return lendedObject;
        }

        public bool GenLendedObject(UserObject scannedUser, List<LendObject> scannedItems)
        {
            LendedObject lendedObjectToAddToDB = new LendedObject(scannedUser, scannedItems);
            return DALLend.Instance.AddLendedObjectToLend(lendedObjectToAddToDB);
        }

        public bool MoveLendedIntoArchive(List<LendObject> lendObjectsToReturn)
        {
            return DALLend.Instance.MoveLendedIntoArchive(lendObjectsToReturn);
        }

        public List<ListViewObject> GetStatInformation()
        {
            return DALLend.Instance.GetStatInformation();
        }

        public List<LendedObject> GetStatisticsInformation()
        {
            List<LendedObject> allStatisticsInformation = new List<LendedObject>();
            List<UserObject> users = UserController.Instance.GetUserStatInformation();

            if (users.Any())
            {
                foreach (UserObject user in users)
                {
                    allStatisticsInformation.Add(new LendedObject(user, DALLend.Instance.GetStatisticsInformation(user.userMifare)));
                }
            }

            return allStatisticsInformation;
        }
    }
}
