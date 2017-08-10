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
            this.itemObject = itemObject;
            this.lendDate = lendDate;
            this.returnDate = returnDate;
            this.returnedDate = returnedDate;
        }

        public ItemObject itemObject { get; set; }
        public DateTime lendDate { get; set; }
        public DateTime returnDate { get; set; }
        public DateTime? returnedDate { get; set; }
    }
}
