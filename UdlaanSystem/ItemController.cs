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

        public ItemObject CheckIfMifareIsItem(string mifare)
        {
            ItemObject item = DALItem.Instance.GetItemByMifare(mifare);
            if (item == null)
            {
                LendController.Instance.GetLendedUserData(mifare);
            }
            
            return item;
        }


        public List<string[]> GetItemTypes()
        {
            return DALItem.Instance.GetItemTypes();
        }
    }
}
