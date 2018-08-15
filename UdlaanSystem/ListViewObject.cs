using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UdlaanSystem
{
    class ListViewObject
    {
        public ListViewObject(string itemMifare, string type, string manufacturer, string model, int id, string serialNumber, DateTime lendDate, DateTime returnDate, DateTime? returnedDate, bool? isOverdue, string zbcName)
        {
            ItemMifare = itemMifare;
            Type = type;
            Manufacturer = manufacturer;
            Model = model;
            Id = id;
            SerialNumber = serialNumber;
            LendDate = lendDate;
            ReturnDate = returnDate;
            ReturnedDate = returnedDate;
            IsOverdue = isOverdue;
            ZbcName = zbcName;
        }

        public string ItemMifare { get; set; }
        public string Type { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public int Id { get; set; }
        public string SerialNumber { get; set; }
        public DateTime LendDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public DateTime? ReturnedDate { get; set; }
        public bool? IsOverdue { get; set; }
        public string ZbcName { get; set; }
    }
}
