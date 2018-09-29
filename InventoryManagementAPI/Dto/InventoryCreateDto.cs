using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagementAPI.Dto
{
    public class InventoryCreateDto
    {

        public string Sku { get; set; }

        public int ThresholdWarning { get; set; }

        public int ThresholdCritical { get; set; }

        public string Product { get; set; }

        public string Location { get; set; }
    }
}
