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
            this.fName = fName;
            this.lName = lName;
            this.zbcName = zbcName;
            this.userMifare = userMifare;
            this.phoneNumber = phoneNumber;
            this.isTeacher = isTeacher;
            this.hasPC = hasPC;
            this.isDisabled = isDisabled;

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
