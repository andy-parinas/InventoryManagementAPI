﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagementAPI.Dto
{
    public class ProductListDto
    {
        public int Id { get; set; }

        public string Upc { get; set; }

        public string Name { get; set; }

        public string Category { get; set; }


    }
}
