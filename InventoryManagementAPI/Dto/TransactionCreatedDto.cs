using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagementAPI.Dto
{
    //DTO to return when the transaction was successfully created.
    //Difference between the detail is the use of InventoryDetailDto
    //which is use in loading the Inventory Reducer in the FrontEnd application
    public class TransactionCreatedDto
    {

        public int Id { get; set; }

        public DateTime TimeStamp { get; set; }

        public double Quantity { get; set; }

        public string Details { get; set; }

        public TransactionTypeDto TransactionType { get; set; }

        public InventoryDetailDto Inventory { get; set; }

    }
}
