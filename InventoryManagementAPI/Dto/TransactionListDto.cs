﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagementAPI.Dto
{
    public class TransactionListDto
    {

        public int Id { get; set; }

        public DateTime TimeStamp { get; set; }

        public double Quantity { get; set; }

        public string Details { get; set; }

        public string TransactionType { get; set; }

    }
}
