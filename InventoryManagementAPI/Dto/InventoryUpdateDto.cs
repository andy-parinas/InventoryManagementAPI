using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagementAPI.Dto
{
    public class InventoryUpdateDto
    {

        public string Sku { get; set; }

        //public int Quantity { get; set; }

        public int ThresholdWarning { get; set; }

        public int ThresholdCritical { get; set; }

        public string Product { get; set; }

        public string Location { get; set; }
    }
}
