using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagementAPI.Dto
{
    public class InventoryDetailDto
    {


        public int Id { get; set; }

        public string Sku { get; set; }

        public int Quantity { get; set; }

        public int ThresholdWarning { get; set; }

        public int ThresholdCritical { get; set; }

        public ProductDetailDto Product { get; set; }

        public LocationDetailDto Location { get; set; }
    }
}
