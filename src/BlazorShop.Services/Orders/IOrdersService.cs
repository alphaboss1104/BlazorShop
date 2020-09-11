﻿namespace BlazorShop.Services.Orders
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Models.Orders;

    public interface IOrdersService
    {
        Task<string> PurchaseAsync(string userId, OrdersRequestModel model);

        Task<OrdersDetailsResponseModel> DetailsAsync(string id);

        Task<IEnumerable<OrdersListingResponseModel>> ByUserAsync(string userId);
    }
}