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
        public int ProductId { get; set; }

        [Required]
        public int LocationId { get; set; }
    }
}
