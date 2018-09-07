using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POSApplicationAPI.Models
{
    public class OrderItem
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public string Description { get; set; }

        public string Upc { get; set; }

        public double Quantity { get; set; }

        public double UnitPrice { get; set; }

        public double TotalExGst { get; set; }

        public double Gst { get; set; }

        public double TotalIncGst { get; set; }

        public Order Order { get; set; }


    }
}
