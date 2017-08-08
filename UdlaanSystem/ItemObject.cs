using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UdlaanSystem
{
    class ItemObject
    {
        public ItemObject(string itemMifare, string type, string manufacturer, string model, int ID , string serialNumber)
        {
            itemMifare = this.itemMifare;
            type = this.type;
            manufacturer = this.manufacturer;
            model = this.model;
            ID = this.ID;
            serialNumber = this.serialNumber;
            
        }

        public string itemMifare { get; set; }
        public string type { get; set; }
        public string manufacturer { get; set; }
        public string model { get; set; }
        public int ID { get; set; }
        public string serialNumber { get; set; }
        
    }
}
