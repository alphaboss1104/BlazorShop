﻿namespace BlazorShop.Services.Implementations
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;
    using Microsoft.EntityFrameworkCore;

    using Data;
    using Data.Models;
    using Shared.Models.Orders;

    public class OrdersService : BaseService<Order>, IOrdersService
    {
        public OrdersService(ApplicationDbContext data, IMapper mapper)
            : base(data, mapper)
        {
        }

        public async Task PurchaseAsync(string userId, int deliveryAddressId)
        {
            await this
                .Data
                .ShoppingCarts
                .Where(sc => sc.UserId == userId)
                .ForEachAsync(async product =>
                {
                    await this.Data.Orders.AddAsync(new Order
                    {
                        UserId = userId,
                        ProductId = product.ProductId,
                        Quantity = product.Quantity,
                        DeliveryAddressId = deliveryAddressId
                    });
                });

            await this.Data.SaveChangesAsync();
        }

        public async Task<IEnumerable<OrdersListingResponseModel>> GetAllByUserIdAsync(string userId)
            => await this.Mapper
                .ProjectTo<OrdersListingResponseModel>(this
                    .AllAsNoTracking()
                    .Where(o => o.UserId == userId))
                .ToListAsync();
    }
}