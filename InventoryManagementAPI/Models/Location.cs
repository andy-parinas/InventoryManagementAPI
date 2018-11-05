using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagementAPI.Models
{
    public class Location
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Address { get; set; }

        public string Phone { get; set; }

        public LocationType LocationType { get; set; }

        public ICollection<Inventory> Inventories { get; set; }

        public bool IsArchived { get; set; }


    }
}
