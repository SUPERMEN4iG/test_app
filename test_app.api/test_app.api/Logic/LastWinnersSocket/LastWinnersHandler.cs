using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace test_app.api.Logic.LastWinnersSocket
{
    public class LastWinnersHandler : WebSocketHandler
    {
        public LastWinnersHandler(WebSocketConnectionManager webSocketConnectionManager) : base(webSocketConnectionManager)
        {
        }

        public override Task ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, byte[] buffer)
        {
            // TODO: Прислали что-то, что с ним будем делать? попытки задудосить сокет
            return Task.FromResult<object>(null);
        }

        public int GetCountConnections()
        {
            int res = 0;

            try
            {
                res = this.WebSocketConnectionManager.GetAll().Count();
            }
            catch (Exception)
            {

            }

            return res;
        }
    }
}
