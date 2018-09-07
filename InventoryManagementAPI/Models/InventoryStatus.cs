using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagementAPI.Models
{
    public class InventoryStatus
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Status { get; set; }

        public ICollection<Inventory> Inventories { get; set; }

    }
}
