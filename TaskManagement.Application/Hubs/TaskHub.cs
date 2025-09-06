using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Application.Hubs
{
    public class TaskHub: Hub
    {
        public async Task SendTaskCreateNotification(string message)
        {
            await Clients.All.SendAsync("ReceiveTaskCreateNotification", message);
        }
    }
}
