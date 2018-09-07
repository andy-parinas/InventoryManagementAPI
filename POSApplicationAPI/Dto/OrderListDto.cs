using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POSApplicationAPI.Dto
{
    public class OrderListDto
    {
        public int Id { get; set; }

        public DateTime OrderDate { get; set; }

        public string InvoiceNumber { get; set; }

        public double TotalExGst { get; set; }

        public double Gst { get; set; }

        public double Total { get; set; }

    }
}
