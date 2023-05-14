using System.Net.WebSockets;
using System.Text;

namespace base64_encoding_service.Services.EncodeService
{
    public class EncodeService : IEncodeService
    {
        public async Task Encode(WebSocket webSocket)
        {
            var buffer = new byte[1024 * 4];
            var receiveResult = await webSocket.ReceiveAsync(
                new ArraySegment<byte>(buffer), CancellationToken.None);

            // Convert the received byte array to a Base64-encoded string
            var receivedData = Encoding.UTF8.GetString(buffer, 0, receiveResult.Count);
            var encodedData = Convert.ToBase64String(Encoding.UTF8.GetBytes(receivedData));

            // Send each character of the Base64-encoded string separately with a delay
            for (int i = 0; i <= encodedData.Length - 1; i++)
            {
                var responseBuffer = Encoding.UTF8.GetBytes(new[] { encodedData[i] });

                // Send the character as a binary message
                await webSocket.SendAsync(
                    new ArraySegment<byte>(responseBuffer),
                    WebSocketMessageType.Text,
                    true, // Send EndOfMessage for each character
                    CancellationToken.None);

                // Wait for a random period of 1-5 seconds before sending the next character
                var random = new Random();
                var delay = TimeSpan.FromSeconds(random.Next(1, 6));

                if(i != encodedData.Length - 1){
                    await Task.Delay(delay);
                }
            }

            await webSocket.CloseAsync(
                WebSocketCloseStatus.NormalClosure,
                receiveResult.CloseStatusDescription,
                CancellationToken.None);
        }
    }
}