using System;
using Microsoft.AspNetCore.SignalR;


namespace AdminPanel.Web.hubs
{
    public class ChatHub : Hub
    {
        public async Task Send(string message, string userName)
        {
            await this.Clients.All.SendAsync("Receive", message, userName);
        }

        public override async Task OnConnectedAsync()
        {
            await Clients.All.SendAsync("Notify", $"{Context.ConnectionId} вошел в чат");
            await base.OnConnectedAsync();
        }
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await Clients.All.SendAsync("Notify", $"{Context.ConnectionId} покинул чат");
            await base.OnDisconnectedAsync(exception);
        }
    }
}