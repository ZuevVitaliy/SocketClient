using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace SocketServer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            SocketServerMethod();

            Console.ReadLine();
        }

        private static async Task SocketServerMethod()
        {
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8888);
            using (Socket socket = new Socket(
                AddressFamily.InterNetwork,
                SocketType.Stream,
                ProtocolType.Tcp))
            {
                socket.Bind(ipPoint);
                socket.Listen(1000);
                Console.WriteLine("Сервер запущен. Ожидание подключений...");
                using (var client = await socket.AcceptAsync())
                {
                    Console.WriteLine(
                        $"Адрес подключенного клиента: {client.RemoteEndPoint}");
                }
            }
        }
    }
}