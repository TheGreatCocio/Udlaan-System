using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UdlaanSystem
{
    public class LendObject
    {
        public LendObject(ItemObject itemObject, DateTime lendDate, DateTime returnDate, DateTime? returnedDate)
        {
            itemObject = this.itemObject;
            lendDate = this.lendDate;
            returnDate = this.returnDate;
            returnedDate = this.returnedDate;
        }

        public ItemObject itemObject { get; set; }
        public DateTime lendDate { get; set; }
        public DateTime returnDate { get; set; }
        public DateTime? returnedDate { get; set; }
    }
}
