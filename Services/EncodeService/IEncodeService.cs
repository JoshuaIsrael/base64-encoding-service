using System.Net.WebSockets;

namespace base64_encoding_service.Services.EncodeService
{
    public interface IEncodeService
    {
        public Task Encode(WebSocket webSocket);
    }
}