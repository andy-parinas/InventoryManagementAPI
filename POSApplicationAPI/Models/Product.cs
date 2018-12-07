using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace POSApplicationAPI.Models
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

        public bool IsGstApplicable { get; set; } = true;

        public ICollection<OrderItem> OrderItems { get; set; }



    }
}
