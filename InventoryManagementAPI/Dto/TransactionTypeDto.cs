using InventoryManagementAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagementAPI.Dto
{
    public class TransactionTypeDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Action { get; set; }
    }
}
