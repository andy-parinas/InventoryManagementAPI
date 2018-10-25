using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagementAPI.Dto
{
    public class LocationFormDto
    {

        public string Name { get; set; }


        public string Address { get; set; }

        public string Phone { get; set; }

        public int LocationTypeId { get; set; }

    }
}
