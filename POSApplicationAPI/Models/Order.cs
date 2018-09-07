using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POSApplicationAPI.Models
{
    public class Order
    {
        public int Id { get; set; }

        public string InvoiceNumber { get; set; }

        public DateTime OrderDate { get; set; }

        public double TotalExGst { get; set; }

        public double TotalGst { get; set; }

        public double Total { get; set; }

        public Customer Customer { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; }

    }
}
