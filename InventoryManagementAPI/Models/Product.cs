using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagementAPI.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Upc { get; set; }

        [Required]
        public string Name { get; set; }

        public string Descriptions { get; set; }

        public double Price { get; set; }

        public double Cost { get; set; }

        public DateTime UpdatedAt { get; set; }

        public ProductCategory ProductCategory { get; set; }

        public ICollection<Inventory> Inventories { get; set; }

        public bool IsArchived { get; set; }


    }
}
