using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using test_app.api.Models;

namespace test_app.api.Data
{
    /// <summary>
    /// Ошибка открытия кейса
    /// </summary>
    public class CaseFaultLog : BaseDataObject<Int64>
    {
        public Case Case { get; set; }

        public ApplicationUser User { get; set; }

        public String Text { get; set; }
    }
}
