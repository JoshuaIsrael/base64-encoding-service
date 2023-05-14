using Microsoft.AspNetCore.Mvc;

namespace base64_encoding_service.Controllers
{
    public class EncodeController : ControllerBase
    {
        private readonly IEncodeService _encodeService;

        public EncodeController(IEncodeService encodeService)
        {
            _encodeService = encodeService;
        }

        [Route("/encode")]
        public async Task Encode()
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                await _encodeService.Encode(webSocket);
            }
            else
            {
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            }
        }
    }

}