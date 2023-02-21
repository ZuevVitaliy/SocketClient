using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Infrastructure.Extensions;

namespace SocketServer
{
    internal class ServerProgram
    {
        static async Task Main(string[] args)
        {
            await SocketServerMethod();

            Console.ReadLine();
        }

        private static async Task SocketServerMethod()
        {
            TcpListener tcpListener = new TcpListener(IPAddress.Parse("127.0.0.1"), 8888);
            try
            {
                tcpListener.Start();
                Console.WriteLine("Сервер запущен. Ожидание подключений...");
                using (var tcpClient = await tcpListener.AcceptTcpClientAsync())
                {
                    Console.WriteLine($"Адрес подключенного клиента: {tcpClient.Client.RemoteEndPoint}");
                    while (true)
                    {
                        var receivedMessage = await tcpClient.ReceiveMessageAsync();
                        Console.WriteLine(receivedMessage);

                        string sendMessage = $"Принято сообщение: {receivedMessage}";
                        await tcpClient.SendMessageAsync(sendMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

       
    }
}