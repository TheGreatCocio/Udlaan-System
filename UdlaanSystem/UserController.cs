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

        public string[] CheckIfUserExist(string zbcName)
        {
            return DALUser.Instance.GetUserByZbcNameAD(zbcName);
        }

        public void CreateUserObjectToAddInDB(string userMifare, string fName, string lName, string zbcName, int phoneNumber, bool isDisabled, bool isTeacher)
        {
            UserObject userObject = new UserObject(fName, lName, zbcName, userMifare, phoneNumber, isDisabled, false, isTeacher);
            DALUser.Instance.AddUserInDB(userObject);
        }
    }
}
