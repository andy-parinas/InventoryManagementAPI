using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagementAPI.Models
{
    public class InventoryTransaction
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime TimeStamp { get; set; }

        public double Quantity { get; set; }

        public string Details { get; set; }

        public TransactionType TransactionType { get; set; }

        public Inventory Inventory { get; set; }

    }
}
