using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace POSApplicationAPI.Models
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        public string ContactNumber { get; set; }

        public DateTime BirthDay { get; set; }

        public string LoyaltyCodes { get; set; }

        public double LoyaltyPoints { get; set; }

        public ICollection<Order> Orders { get; set; }

    }
}
