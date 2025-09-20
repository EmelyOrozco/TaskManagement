using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using TaskManagement.Application.Hubs;

namespace TaskManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly IHubContext<TaskHub> _hubContext;

        public NotificationController(IHubContext<TaskHub> hubContext)
        {
            _hubContext = hubContext;
        }

        [HttpGet("Send")]
        public async Task<IActionResult> SendNotifications(string message)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveTaskCreateNotification1", message);

            return Ok("Notificación enviada a todos los clientes.");
        }

       
    }
}
