using System.Net.Sockets;
using System;
using System.Net.Sockets;
using System.Text;


namespace Client
{

    class Program
    {
        static void Main()
        {
            string serverIP = "127.0.0.1"; // Server IP address
            int serverPort = 12345;       // Server port

            // Create a TCP client socket
            TcpClient client = new TcpClient(serverIP, serverPort);

            
            try
            {
                // Get a network stream from the client socket
                NetworkStream stream = client.GetStream();
                while (true)
                {                  
                    Console.WriteLine("Sending message to server ......");

                    // Send a message to the server
                    string messageToSend = "Hello, server!";
                    byte[] dataToSend = Encoding.ASCII.GetBytes(messageToSend);
                    stream.Write(dataToSend, 0, dataToSend.Length);

                    // Receive response from the server
                    byte[] dataReceived = new byte[1024];
                    int bytesRead = stream.Read(dataReceived, 0, dataReceived.Length);
                    string response = Encoding.ASCII.GetString(dataReceived, 0, bytesRead);

                    Console.WriteLine($"Server response: {response}");
                    Thread.Sleep(2000);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                // Close the client socket
                client.Close();
            }
            Console.Read();
        }
    }

}