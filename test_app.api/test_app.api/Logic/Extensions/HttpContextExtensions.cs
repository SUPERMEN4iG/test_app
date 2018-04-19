using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace test_app.api.Logic.Extensions
{
    public static class HttpContextExtensions
    {
        public static String GetIpAddress(this HttpContext context)
        {
            string ip = "";

            #if DEBUG
            ip = context.Connection.LocalIpAddress.ToString();
            #endif
            #if DEBUG
            ip = context.Connection.RemoteIpAddress.ToString();
            #endif

            return ip;
        }
    }
}
