using System.Net.Sockets;
using System.Text;

namespace Infrastructure.Extensions
{
    public static class TcpExtensions
    {
        public static async Task SendMessageAsync(this TcpClient tcpClient, string message)
        {
            if (string.IsNullOrEmpty(message))
                throw new ArgumentNullException("message");

            var stream = tcpClient.GetStream();
            byte[] buffer = Encoding.UTF8.GetBytes(message);
            await stream.WriteAsync(buffer, 0, buffer.Length);
        }

        public static async Task<string> ReceiveMessageAsync(this TcpClient tcpClient)
        {
            var stream = tcpClient.GetStream();
            var builder = new StringBuilder();
            byte[] buffer = new byte[256];

            do
            {
                int bytes = await stream.ReadAsync(buffer, 0, buffer.Length);
                string addingText = Encoding.UTF8.GetString(buffer, 0, bytes);
                builder.Append(addingText);
            } while (stream.DataAvailable);

            return builder.ToString();
        }
    }
}
