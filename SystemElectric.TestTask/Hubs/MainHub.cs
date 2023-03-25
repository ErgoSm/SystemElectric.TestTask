using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using SystemElectric.TestTask.Domain.Entities;

namespace SystemElectric.TestTask.Hubs
{
    public class MainHub : Hub
    {
        /*public async Task SendMessage(Entry entry)
        {
            await Clients.All.SendAsync("EntryAdded", entry);
        }*/
    }
}
