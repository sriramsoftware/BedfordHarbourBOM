﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class StockTakeHistory : BaseEntity
    {
        //Replace with abstract StockBase class?
        public Part Part { get; set; }
        public Item Item { get; set; }
        public RawMaterial RawMaterial { get; set; }

        public int PreviousCount { get; set; }
        public int Count { get; set; }
        public DateTime CountDate { get; set; }
        public int Cost { get; set; }
    }
}