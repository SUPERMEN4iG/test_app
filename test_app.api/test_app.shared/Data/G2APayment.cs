using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace test_app.shared.Data
{
    public class G2APayment : BaseDataObject<Int64>
    {
        public enum G2APaymentStatus
        {
            None = 0,
            Success = 1,
            Failure = 2
        }

        public ApplicationUser User { get; set; }

        public decimal Sum { get; set; }

        public string TransactionId { get; set; }

        public string Currency { get; set; }

        public G2APaymentStatus Status { get; set; }

        // Mapping
        internal class G2APaymentConfiguration : DbEntityConfiguration<G2APayment, Int64>
        {
            public override void Configure(EntityTypeBuilder<G2APayment> entity)
            {
            }
        }
    }
}
