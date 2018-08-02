using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagementAPI.Dto
{
    public class LocationFormDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Address { get; set; }

        public string Phone { get; set; }

        [Required]
        public int LocationTypeId { get; set; }

    }
}
