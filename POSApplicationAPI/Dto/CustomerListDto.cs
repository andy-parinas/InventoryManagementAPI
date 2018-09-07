using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POSApplicationAPI.Dto
{
    public class CustomerListDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string ContactNumber { get; set; }

        public double LoyaltyPoints { get; set; }
    }
}
