using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using test_app.api.Data;

namespace test_app.api.Logic
{
    public enum ResponseType {
        Ok = 0,
        NotEnoughMoney = 1,
        AccessDenied = 2,
        ServerError = 3,
    }

    public class CaseOpenResult
    {
        public Boolean IsSuccess { get; set; }
        public String Message { get; set; }
        public Object Winner { get; set; }
        public ResponseType Type { get; set; }

        public static CaseOpenResult GenerateError(string message, ResponseType type)
        {
            return new CaseOpenResult() { IsSuccess = false, Winner = null, Message = message, Type = type };
        }

        public static CaseOpenResult GenerateSuccess(object winner, string message, ResponseType type = ResponseType.Ok)
        {
            return new CaseOpenResult() { IsSuccess = true, Winner = winner, Message = message, Type = type };
        }
    }
}
