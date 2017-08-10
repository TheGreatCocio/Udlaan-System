using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UdlaanSystem
{
    class ListViewObject
    {
        public ListViewObject(string itemMifare, string type, string manufacturer, string model, int id, string serialNumber, DateTime lendDate, DateTime returnDate, DateTime? returnedDate)
        {
            this.itemMifare = itemMifare;
            this.type = type;
            this.manufacturer = manufacturer;
            this.model = model;
            this.id = id;
            this.serialNumber = serialNumber;
            this.lendDate = lendDate;
            this.returnDate = returnDate;
            this.returnedDate = returnedDate;

        }

        public string itemMifare { get; set; }
        public string type { get; set; }
        public string manufacturer { get; set; }
        public string model { get; set; }
        public int id { get; set; }
        public string serialNumber { get; set; }
        public DateTime lendDate { get; set; }
        public DateTime returnDate { get; set; }
        public DateTime? returnedDate { get; set; }
    }
}
