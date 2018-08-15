using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UdlaanSystem
{
    public class UserObject
    {
        public UserObject(string fName, string lName, string zbcName, string userMifare, int phoneNumber, bool isTeacher, bool hasPC, bool isDisabled, string comment)
        {
            FName = fName;
            LName = lName;
            ZbcName = zbcName;
            UserMifare = userMifare;
            PhoneNumber = phoneNumber;
            IsTeacher = isTeacher;
            HasPC = hasPC;
            IsDisabled = isDisabled;
            Comment = comment;
        }

        public string FName { get; set; }
        public string LName { get; set; }
        public string ZbcName { get; set; }
        public string UserMifare { get; set; }
        public int PhoneNumber { get; set; }
        public bool IsTeacher { get; set; }
        public bool HasPC { get; set; }
        public bool IsDisabled { get; set; }
        public string Comment { get; set; }
    }
}
