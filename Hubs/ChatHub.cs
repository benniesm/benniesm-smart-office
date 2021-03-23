using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using SmartOffice.Data;
using SmartOffice.Models;
using Newtonsoft.Json.Linq;

namespace SignalRChat.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ApplicationDbContext _context;

        public ChatHub(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task SendMessage(string user, string message)
        {
            var messageObj = JObject.Parse(message);
            string msg = messageObj.SelectToken("msg").Value<string>();
            ChatMessage msgToSave = new ChatMessage {
                Owner = user,
                Message = msg,
                GroupName = "general"
            };

            _context.Add(msgToSave);
                await _context.SaveChangesAsync();
                await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}