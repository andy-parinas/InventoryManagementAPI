using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagementAPI.Models
{
    public class Inventory
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Sku { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public int ThresholdWarning { get; set; }

        [Required]
        public int ThresholdCritical { get; set; }

        public Product Product { get; set; }

        public Location Location { get; set; }

        public InventoryStatus Status { get; set; }

        public bool IsArchived { get; set; }

        public ICollection<InventoryTransaction> Transactions { get; set; }


    }
}
