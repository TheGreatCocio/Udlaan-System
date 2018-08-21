using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UdlaanSystem
{
    public class ItemObject
    {
        public ItemObject(string itemMifare, string type, string manufacturer, string model, int id , string serialNumber)
        {
            ItemMifare = itemMifare;
            Type = type;
            Manufacturer = manufacturer;
            Model = model;
            Id = id;
            SerialNumber = serialNumber;
            
        }

        public string ItemMifare { get; set; }
        public string Type { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public int Id { get; set; }
        public string SerialNumber { get; set; }
        
    }
}
