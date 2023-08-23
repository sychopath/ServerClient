namespace AsynServer
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            int port = 12345;
            var server = new Server();
            await server.SampleServerApp(port);
        }
    }
}