﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagementAPI.Helpers
{
    public class InventoryParams
    {
        public string Sku { get; set; }

        public string Product { get; set; }

        public string Location { get; set; }

        public int Status { get; set; } = 0;

        public string OrderBy { get; set; } = "location";

        public string Direction { get; set; } = "ASC";

        private const int MAX_PAGE_SIZE = 50;

        public int PageNumber { get; set; } = 1;

        private int pageSize = 10;

        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = (value > MAX_PAGE_SIZE ? MAX_PAGE_SIZE : value); }
        }
    }
}
