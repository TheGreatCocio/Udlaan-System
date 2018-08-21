using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UdlaanSystem
{
    public class LendedObject
    {
        public LendedObject(UserObject userObject, List<LendObject> lendObjects)
        {
            UserObject = userObject;
            LendObjects = lendObjects;
        }

        public UserObject UserObject { get; set; }
        public List<LendObject> LendObjects { get; set; }
    }
}
