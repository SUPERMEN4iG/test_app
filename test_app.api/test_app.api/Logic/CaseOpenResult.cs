using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using test_app.api.Data;

namespace test_app.api.Logic
{
    public class CaseOpenResult
    {
        public Boolean IsSuccess { get; set; }
        public String Message { get; set; }
        public Object Winner { get; set; }

        public static CaseOpenResult GenerateError(string message)
        {
            return new CaseOpenResult() { IsSuccess = false, Winner = null, Message = message };
        }

        public static CaseOpenResult GenerateSuccess(object winner, string message)
        {
            return new CaseOpenResult() { IsSuccess = true, Winner = winner, Message = message};
        }
    }
}
