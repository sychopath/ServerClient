using System.Collections.Concurrent;
using System.Net.Sockets;
using System.Text;

namespace AsynServer
{
    internal class Server
    {
        static ConcurrentBag<TcpClient> connectedSockets = new ConcurrentBag<TcpClient>();
        public async Task SampleServerApp(int port)
        {
            var listener = new TcpListener(port);
            listener.Start();
            Console.WriteLine("Starting server app on port {0}.... ",port);
            
            while (true)
            {
                // Accept client request asynchronosly
                var connection = await listener.AcceptTcpClientAsync();

                ProcessConnection(connection);

                connectedSockets.Add(connection);
            }
        }

        // Process connection to fetch stream object
        private async void ProcessConnection(TcpClient connection)
        {
            var stream = connection.GetStream();

            Task.Run(async () => await HandlCommunication(stream));
        }


        // Read & write message to stream
        private async Task HandlCommunication(NetworkStream stream)
        {
            byte[] buffer = new byte[1024];

            try
            {
                while (stream.CanRead)
                {
                    var bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                    string receivedMessage = Encoding.ASCII.GetString(buffer, 0, bytesRead);

                    Console.WriteLine($"Received: {receivedMessage}");

                    await BroadcastMessage();
                }
            } 
            catch(Exception ex)
            {
                Console.WriteLine("Exception occured in HandlCommunication. Exception message : {0}", ex.Message);
            }

        }

        // Broadcast message to all clients
        private async Task BroadcastMessage()
        {
            var id = Guid.NewGuid();
            string message = "Server Message : " + id.ToString();
            var data = Encoding.UTF8.GetBytes(message);

            foreach (var connection in connectedSockets)
            {
                if (connection.Connected)
                {
                    await connection.GetStream().WriteAsync(data, 0, data.Length);
                }
            }
        }


    }
}
