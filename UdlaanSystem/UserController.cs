using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UdlaanSystem
{
    class UserController
    {
        public UserController()
        {
        }
        private static UserController instance;

        public static UserController Instance
        {
            get
            {
                if (instance == null)
                { instance = new UserController(); }
                return instance;
            }
        }

        public UserObject GetUserObject(string userMifare)
        {
            return DALUser.Instance.GetUserByMifare(userMifare);
        }
    }
}
