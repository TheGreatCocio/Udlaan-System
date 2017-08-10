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
            this.itemMifare = itemMifare;
            this.type = type;
            this.manufacturer = manufacturer;
            this.model = model;
            this.id = id;
            this.serialNumber = serialNumber;
            
        }

        public string itemMifare { get; set; }
        public string type { get; set; }
        public string manufacturer { get; set; }
        public string model { get; set; }
        public int id { get; set; }
        public string serialNumber { get; set; }
        
    }
}
