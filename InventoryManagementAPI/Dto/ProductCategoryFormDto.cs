﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagementAPI.Dto
{
    public class ProductCategoryFormDto
    {
        [Required]
        public string Name { get; set; }

    }
}
