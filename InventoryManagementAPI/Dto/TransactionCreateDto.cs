using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagementAPI.Dto
{
    public class TransactionCreateDto
    {
        public int Transaction { get; set; }

        public int Quantity { get; set; }

        public string Details { get; set; }


    }
}
