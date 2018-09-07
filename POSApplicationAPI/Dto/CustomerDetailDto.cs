using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POSApplicationAPI.Dto
{
    public class CustomerDetailDto
    {

        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string ContactNumber { get; set; }

        public DateTime BirthDay { get; set; }

        public string LoyaltyCodes { get; set; }

        public double LoyaltyPoints { get; set; }

    }
}
