using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UdlaanSystem
{
    public class UserObject
    {
        public UserObject(string fName, string lName, string zbcName, string userMifare, int phoneNumber, bool isTeacher, bool hasPC, bool isDisabled)
        {
            fName = this.fName;
            lName = this.lName;
            zbcName = this.zbcName;
            userMifare = this.userMifare;
            phoneNumber = this.phoneNumber;
            isTeacher = this.isTeacher;
            hasPC = this.hasPC;
            isDisabled = this.isDisabled;

        }

        public string fName { get; set; }
        public string lName { get; set; }
        public string zbcName { get; set; }
        public string userMifare { get; set; }
        public int phoneNumber { get; set; }
        public bool isTeacher { get; set; }
        public bool hasPC { get; set; }
        public bool isDisabled { get; set; }
    }
}
