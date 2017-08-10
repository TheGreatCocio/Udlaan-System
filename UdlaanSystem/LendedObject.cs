using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UdlaanSystem
{
    public class LendedObject
    {
        public LendedObject(UserObject UserObject, List<LendObject> LendObjects)
        {
            this.UserObject = UserObject;
            this.LendObjects = LendObjects;
        }

        public UserObject UserObject { get; set; }
        public List<LendObject> LendObjects { get; set; }
    }
}
