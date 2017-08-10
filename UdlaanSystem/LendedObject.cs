using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UdlaanSystem
{
    public class LendedObject
    {
        public LendedObject(UserObject UserObject, List<LendObject> LendedObjects)
        {
            UserObject = this.UserObject;
            LendedObjects = this.LendedObjects;
        }

        public UserObject UserObject { get; set; }
        public List<LendObject> LendedObjects { get; set; }
    }
}
