using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using test_app.api.Logic.LastWinnersSocket;

namespace test_app.api.Logic.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddWebSocketManager(this IServiceCollection services)
        {
            //services.AddTransient<WebSocketConnectionManager>();

            //foreach (var type in Assembly.GetEntryAssembly().ExportedTypes)
            //{
            //    if (type.GetTypeInfo().BaseType == typeof(WebSocketHandler))
            //    {
            //        services.AddSingleton(type);
            //        Debug.WriteLine("added");
            //    }
            //}

            return services;
        }
    }
}
