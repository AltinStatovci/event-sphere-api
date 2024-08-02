using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSphere.Business.Hubs
{
    public class TicketHub : Hub
    {
        public async Task UpdateTicketCount(int eventId, int availableTickets)
        {
            await Clients.All.SendAsync("ReceiveTicketCountUpdate", eventId, availableTickets);
        }
    }
}
