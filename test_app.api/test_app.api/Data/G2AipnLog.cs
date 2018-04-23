using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace test_app.api.Data
{
    public class G2AIPNLog : BaseDataObject<Int64>
    {
        public string Request { get; set; }

        public string Response { get; set; }

        // Mapping
        internal class G2AIPNLogConfiguration : DbEntityConfiguration<G2AIPNLog, Int64>
        {
            public override void Configure(EntityTypeBuilder<G2AIPNLog> entity)
            {
            }
        }
    }
}
