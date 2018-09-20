using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagementAPI.Dto
{
    public class InventoryCreateDto
    {

        [Required]
        public string Sku { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public int ThresholdWarning { get; set; }

        [Required]
        public int ThresholdCritical { get; set; }

        [Required]
        public string Product { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        public string Status { get; set; }
    }
}
