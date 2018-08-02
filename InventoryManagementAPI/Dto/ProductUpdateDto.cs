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
        public string Name { get; set; }

        public string Descriptions { get; set; }

        [Required]
        public int ProductCategoryId { get; set; }
    }
}
