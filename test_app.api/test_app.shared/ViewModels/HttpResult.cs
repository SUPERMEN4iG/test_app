using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace test_app.shared.ViewModels
{
    public enum ResponseType
    {
        Ok = 0,
        NotEnoughMoney = 1,
        AccessDenied = 2,
        ServerError = 3,
        ValidationError = 4,
        NotFound = 5,
    }

    public class BaseHttpResult
    {
        public Boolean IsSuccess { get; set; }

        public String Message { get; set; }

        public Object Data { get; set; }

        public ResponseType Type { get; set; }

        public DateTime DateTime { get; set; } = DateTime.Now;

        /// <summary>
        /// byte size of data
        /// </summary>
        public int Size { get; set; }

        public static BaseHttpResult GenerateError(string message, ResponseType type)
        {
            return new BaseHttpResult() { IsSuccess = false, Data = null, Message = message, Type = type, Size = 0 };
        }

        public static BaseHttpResult GenerateSuccess(object data, string message, ResponseType type = ResponseType.Ok)
        {
            // TODO: double serialize object to json need to optimization (probably)
            var dataString = JsonConvert.SerializeObject(data);
            return new BaseHttpResult() { IsSuccess = true, Data = data, Message = message, Type = type, Size = System.Text.Encoding.Default.GetBytes(dataString).Length };
        }
    }

    public class CaseOpenResult
    {
        public Boolean IsSuccess { get; set; }
        public String Message { get; set; }
        public WinnerViewModel Winner { get; set; }
        public ResponseType Type { get; set; }

        public static CaseOpenResult GenerateError(string message, ResponseType type)
        {
            return new CaseOpenResult() { IsSuccess = false, Winner = null, Message = message, Type = type };
        }

        public static CaseOpenResult GenerateSuccess(WinnerViewModel winner, string message, ResponseType type = ResponseType.Ok)
        {
            return new CaseOpenResult() { IsSuccess = true, Winner = winner, Message = message, Type = type };
        }

        public static CaseOpenResult GenerateSuccessTest(WinnerViewModel winner, string message, ResponseType type = ResponseType.Ok)
        {
            return new CaseOpenResult() { IsSuccess = true, Winner = winner, Message = message, Type = type };
        }
    }
}
