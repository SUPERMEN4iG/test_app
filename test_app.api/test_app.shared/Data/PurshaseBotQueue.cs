﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace test_app.shared.Data
{
    public class PurshaseBotQueue : BaseDataObject<Int64>
    {
        public Boolean Locked { get; set; }

        public Bot LastBot { get; set; }
         
        public decimal MaxPriceUsd { get; set; }

        public DateTime? DateLastRequest { get; set; }

        public int TriesCount { get; set; }

        public string MarketHashName { get; set; }

        // Mapping
        internal class PurshaseBotQueueConfiguration : DbEntityConfiguration<PurshaseBotQueue, Int64>
        {
            public override void Configure(EntityTypeBuilder<PurshaseBotQueue> entity)
            {
            }
        }
    }
}
