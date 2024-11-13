using Microsoft.AspNetCore.SignalR;

namespace Application.HubGathering.Stockhub;

public class StockHub : Hub
{
    public async Task UpdateStock(string partId, int newStock)
    {
        await Clients.All.SendAsync("ReceiveStockUpdate", partId, newStock);
    }
}
