using Microsoft.AspNet.SignalR;

namespace dotnet_core_MVC.Hubs
{
    public class Chathub:Hub
    {
         public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
