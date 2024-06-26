﻿using Microsoft.AspNetCore.SignalR;
using ShopShoe.Application.ViewModel.Query;

namespace ShopShoe.WebApi.SignalR
{
    public class ShopShoeHub:Hub
    {
        public async Task SendMessage(AnnouncementViewModel message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message);
        }
    }
}
