using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace test_app.api.Logic.LastWinnersSocket
{
    public class WebSocketManagerMiddleware
    {
        private readonly RequestDelegate _next;
        private WebSocketHandler _webSocketHandler { get; set; }

        public WebSocketManagerMiddleware(RequestDelegate next,
                                          WebSocketHandler webSocketHandler = null)
        {
            _next = next;
            _webSocketHandler = webSocketHandler;
        }

        public async Task Invoke(HttpContext context)
        {
            if (!context.WebSockets.IsWebSocketRequest)
            {
                await _next.Invoke(context);
                return;
            }

            var socket = await context.WebSockets.AcceptWebSocketAsync().ConfigureAwait(false);
            await _webSocketHandler.OnConnected(socket).ConfigureAwait(false);

            await Receive(socket, async (result, buffer) =>
            {
                if (result.MessageType == WebSocketMessageType.Text)
                {
                    await _webSocketHandler.ReceiveAsync(socket, result, buffer).ConfigureAwait(false);
                    return;
                }
                else if (result.MessageType == WebSocketMessageType.Close)
                {
                    try
                    {
                        await _webSocketHandler.OnDisconnected(socket);
                    }
                    catch (WebSocketException)
                    {
                        throw;
                    }
                    return;
                }

            });

            //await _next.Invoke(context);
        }

        private async Task Receive(WebSocket socket, Action<WebSocketReceiveResult, byte[]> handleMessage)
        {
            while (socket.State == WebSocketState.Open)
            {
                var buffer = new byte[1024 * 4];
                try
                {
                    var result = await socket.ReceiveAsync(buffer: new ArraySegment<byte>(buffer),
                                                       cancellationToken: CancellationToken.None);

                    handleMessage(result, buffer);
                }
                catch (WebSocketException ex)
                {
                    if (ex.WebSocketErrorCode != WebSocketError.Success)
                    {
                        socket.Abort();
                    }
                }

                await _webSocketHandler.OnDisconnected(socket);
            }
        }
    }
}
