using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UdlaanSystem
{
    class ItemController
    {
        public ItemController()
        {
        }
        private static ItemController instance;

        public static ItemController Instance
        {
            get
            {
                if (instance == null)
                { instance = new ItemController(); }
                return instance;
            }
        }

        public void CheckIfMifareIsItem(string itemMifare)
        {
            ItemObject item = DALItem.Instance.GetItemByMifare(itemMifare);

            if (item == null )
            {
                LendController.Instance.GetLendedUserData(itemMifare);

            }
            else
            {
                string userMifare = LendController.Instance.CheckIfLended(itemMifare);

                if (userMifare == "")
                {
                    
                }
            }
            
        }


        public List<string[]> GetItemTypes()
        {
            return DALItem.Instance.GetItemTypes();
        }
    }
}
