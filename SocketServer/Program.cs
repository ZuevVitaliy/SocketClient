using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Text;

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
                try
                {
                    socket.Bind(ipPoint);
                    socket.Listen();
                    Console.WriteLine("Сервер запущен. Ожидание подключений...");
                    using (var client = await socket.AcceptAsync())
                    {
                        Console.WriteLine(
                            $"Адрес подключенного клиента: {client.RemoteEndPoint}");

                        var buffer = new List<byte>();
                        var bytesReadBuffer = new byte[1];
                        while (true)
                        {
                            var bytesCount = await client
                            .ReceiveAsync(bytesReadBuffer, SocketFlags.Partial);
                            if (bytesCount == 0 || bytesReadBuffer[0] == '\n') break;
                            buffer.Add(bytesReadBuffer[0]);
                        }
                        string receivedMessage =
                            Encoding.UTF8.GetString(buffer.ToArray());
                        Console.WriteLine(receivedMessage);

                        string sendMessage = $"Принято сообщение: {receivedMessage}";
                        byte[] sendBytes = Encoding.UTF8.GetBytes(sendMessage);
                        await client.SendAsync(sendBytes, SocketFlags.Partial);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}