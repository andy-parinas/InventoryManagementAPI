using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagementAPI.Dto
{
    public class ProductUpdateDto
    {
        [Required]
        public string Upc { get; set; }

        [Required]
        public string Product { get; set; }

        public string Descriptions { get; set; }

        [Required]
        public int Category { get; set; }

        [Required]
        public double Cost { get; set; }

        [Required]
        public double Price { get; set; }


    }
}
