using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Intsa.Hubs
{
    public class NoticeHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
