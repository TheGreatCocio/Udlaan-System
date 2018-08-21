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
            ItemObject = itemObject;
            LendDate = lendDate;
            ReturnDate = returnDate;
            ReturnedDate = returnedDate;
        }

        public ItemObject ItemObject { get; set; }
        public DateTime LendDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public DateTime? ReturnedDate { get; set; }
    }
}
