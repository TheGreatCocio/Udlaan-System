using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UdlaanSystem.DataAccess;

namespace UdlaanSystem.Managers
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

        public string[] CheckIfUserExistInAD(string zbcName)
        {
            return DALUser.Instance.GetUserByZbcNameAD(zbcName);
        }

        public UserObject CheckIfUserExist(string zbcName)
        {
            return DALUser.Instance.GetUserByZbcName(zbcName);
        }

        public void CreateUserObjectToAddInDB(string userMifare, string fName, string lName, string zbcName, int phoneNumber, bool isDisabled, bool isTeacher)
        {
            UserObject userObject = new UserObject(fName, lName, zbcName, userMifare, phoneNumber, isDisabled, false, isTeacher, "");
            DALUser.Instance.AddUserInDB(userObject);
        }

        public void CreateUserObjectToUpdateInDB(string userMifare, string fName, string lName, string zbcName, int phoneNumber, bool isDisabled, bool isTeacher)
        {
            UserObject userObject = new UserObject(fName, lName, zbcName, userMifare, phoneNumber, isTeacher, false, isDisabled, "");
            DALUser.Instance.UpdateUserInDB(userObject);
        }

        public void UpdateUserNote (string note, string mifare)
        {
            DALUser.Instance.UpdateUserNote(note, mifare);
        }

        public string GetUserNote(string mifare)
        {
            return DALUser.Instance.GetUserNote(mifare);
        }

        public bool IsUser(string mifare)
        {
            return DALUser.Instance.IsUser(mifare);
        }

        public List<UserObject> GetUserStatInformation()
        {
            return DALUser.Instance.GetUserStatInformation();
        }

        public bool UpdateUserZbcNameToUniLogin(string zbcName, string uniLogin)
        {
            return DALUser.Instance.UpdateUserZbcNameToUniLogin(zbcName, uniLogin);
        }
    }
}
