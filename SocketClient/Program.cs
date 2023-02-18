using System.Net.Sockets;
using System.Text;

namespace SocketClient
{
    internal class Program
    {
        static void Main(string[] args)
        {
            SimpleSocketClient();

            //SocketClientMethod();

            Console.ReadLine();
        }

        private static async Task SimpleSocketClient()
        {
            using (var socket = new Socket(
                AddressFamily.InterNetwork,
                SocketType.Stream, 
                ProtocolType.Tcp))
            {
                try
                {
                    await socket.ConnectAsync("127.0.0.1", 8888);
                    Console.WriteLine($"Подключение к {socket.RemoteEndPoint} установлено.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Не удалось установить подключение {socket.RemoteEndPoint}.");
                }
            }
        }

        private static async Task SocketClientMethod()
        {
            var port = 80;
            var url = "www.google.com";
            var response = await SocketSendAndReceiveAsync(url, port);
            Console.WriteLine(response);
        }

        private static async Task<string> SocketSendAndReceiveAsync(string url, int port)
        {
            using (var socket = await ConnectSocketAsync(url, port))
            {
                if (socket == null)
                    return $"Не удалось установить соединение с {url}";

                try
                {
                    var message = $"GET / HTTP/1.1\r\nHost: {url}\r\nConnection: close\r\n\r\n";

                    var messageBytes = Encoding.UTF8.GetBytes(message);
                    int bytesSent = await socket.SendAsync(messageBytes, SocketFlags.None);

                    var responseBytes = new byte[512];
                    var builder = new StringBuilder();
                    int bytes;

                    do
                    {
                        bytes = await socket.ReceiveAsync(responseBytes, SocketFlags.None);
                        var responsePart = Encoding.UTF8.GetString(responseBytes, 0, bytes);
                        builder.Append(responsePart);
                    } while (bytes > 0);

                    socket.Shutdown(SocketShutdown.Both);
                    await socket.DisconnectAsync(true);

                    return builder.ToString();

                    //Console.WriteLine($"На адрес {url} отправлено {bytesSent} байт(а)");

                    //Console.WriteLine($"Подключение к {url} установлено.");
                    //Console.WriteLine($"Адрес подключения {socket.RemoteEndPoint}");
                    //Console.WriteLine($"Адрес приложения {socket.LocalEndPoint}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Не удалось установить подключение к {url}.");
                    throw;
                }
                finally
                {
                    socket.Close();
                }
            }
        }

        private static async Task<Socket> ConnectSocketAsync(string url, int port)
        {
            var tempSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                await tempSocket.ConnectAsync(url, port);
                return tempSocket;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                tempSocket.Close();
            }

            return null;
        }
    }
}