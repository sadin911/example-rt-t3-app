using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PcStats
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var server = new WebSocketServer();
            await server.Start("http://localhost:8000/");
        }
    }

    public class WebSocketServer
    {
        private readonly HttpListener _listener = new HttpListener();

        public async Task Start(string url)
        {
            _listener.Prefixes.Add(url);
            _listener.AuthenticationSchemes = AuthenticationSchemes.Anonymous;
            _listener.Start();
            while (true)
            {
                var context = await _listener.GetContextAsync();
                if (context.Request.IsWebSocketRequest)
                {
                    try
                    {
                        var wsContext = await context.AcceptWebSocketAsync(null);
                        _ = Task.Run(async () => await SendPcStats(wsContext.WebSocket));
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine("Error accepting WebSocket connection: {0}", ex);
                    }
                }
            }
        }

        private async Task SendPcStats(WebSocket webSocket)
        {
            try
            {
                while (webSocket.State == WebSocketState.Open)
                {
                    var pcStats = GetPcStats();
                    var json = JsonConvert.SerializeObject(pcStats);
                    var buffer = Encoding.UTF8.GetBytes(json);
                    await webSocket.SendAsync(
                        new ArraySegment<byte>(buffer),
                        WebSocketMessageType.Text,
                        true,
                        CancellationToken.None
                    );
                    await Task.Delay(500);
                }
            }
            finally
            {
                await webSocket.CloseAsync(
                    WebSocketCloseStatus.NormalClosure,
                    "",
                    CancellationToken.None
                );
            }
        }

        private Dictionary<string, object> GetPcStats()
        {
            // Retrieve CPU and Memory statistics using PerformanceCounter
            var cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            cpuCounter.NextValue();
            Thread.Sleep(1000);
            var cpuPercent = cpuCounter.NextValue();

            var memoryCounter = new PerformanceCounter("Memory", "% Committed Bytes In Use");
            var memoryPercent = memoryCounter.NextValue();

            // Create the PC stats dictionary
            var pcStats = new Dictionary<string, object>
            {
                { "CPU Percent", cpuPercent },
                { "Memory Percent", memoryPercent }
            };

            return pcStats;
        }
    }
}
