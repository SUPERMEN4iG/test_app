using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace test_app.shared.Data
{
    /// <summary>
    /// Ошибка открытия кейса
    /// </summary>
    public class CaseFaultLog : BaseDataObject<Int64>
    {
        public Case Case { get; set; }
        public long CaseId { get; set; }

        public ApplicationUser User { get; set; }
        public string UserId { get; set; }

        public String Text { get; set; }

        // Mapping
        internal class CaseFaultLogConfiguration : DbEntityConfiguration<CaseFaultLog, Int64>
        {
            public override void Configure(EntityTypeBuilder<CaseFaultLog> entity)
            {
            }
        }
    }
}
