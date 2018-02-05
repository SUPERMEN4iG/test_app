using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using test_app.api.Models;

namespace test_app.api.Data
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

        public string Currency { get; set; }

        public G2APaymentStatus Status { get; set; }
    }
}
