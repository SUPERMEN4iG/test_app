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
        ValidationError = 4
    }

    public class BaseHttpResult
    {
        public Boolean IsSuccess { get; set; }
        public String Message { get; set; }
        public Object Data { get; set; }
        public ResponseType Type { get; set; }
        public DateTime DateTime { get; set; } = DateTime.Now;

        public static BaseHttpResult GenerateError(string message, ResponseType type)
        {
            return new BaseHttpResult() { IsSuccess = false, Data = null, Message = message, Type = type };
        }

        public static BaseHttpResult GenerateSuccess(object data, string message, ResponseType type = ResponseType.Ok)
        {
            return new BaseHttpResult() { IsSuccess = true, Data = data, Message = message, Type = type };
        }
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
