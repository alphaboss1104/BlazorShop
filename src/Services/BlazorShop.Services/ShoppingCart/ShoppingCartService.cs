﻿namespace BlazorShop.Services.ShoppingCart
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Microsoft.EntityFrameworkCore;

    using Data;
    using Data.Models;
    using DateTime;
    using Web.Shared.ShoppingCart;

    public class ShoppingCartService : IShoppingCartService
    {
        private readonly ApplicationDbContext db;
        private readonly IMapper mapper;
        private readonly IDateTimeProvider dateTimeProvider;

        public ShoppingCartService(
            ApplicationDbContext db, 
            IMapper mapper,
            IDateTimeProvider dateTimeProvider)
        {
            this.db = db;
            this.mapper = mapper;
            this.dateTimeProvider = dateTimeProvider;
        }

        public async Task AddAsync(int productId, string userId, int quantity)
        {
            var shoppingCart = new ShoppingCart
            {
                UserId = userId,
                ProductId = productId,
                Quantity = quantity,
                CreatedOn = this.dateTimeProvider.Now()
            };

            await this.db.ShoppingCarts.AddAsync(shoppingCart);
            await this.db.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(int productId, string userId, int quantity)
        {
            var shoppingCart = await this.GetByProductIdAndUserIdAsync(productId, userId);
            if (shoppingCart == null)
            {
                return false;
            }

            shoppingCart.Quantity = quantity;
            shoppingCart.ModifiedOn = this.dateTimeProvider.Now();

            await this.db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RemoveAsync(int productId, string userId)
        {
            var shoppingCart = await this.GetByProductIdAndUserIdAsync(productId, userId);
            if (shoppingCart == null)
            {
                return false;
            }

            this.db.Remove(shoppingCart);

            await this.db.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<ShoppingCartProductsResponseModel>> GetByUserIdAsync(string userId)
            => await this
                .AllByUserId(userId)
                .AsNoTracking()
                .ProjectTo<ShoppingCartProductsResponseModel>(this.mapper.ConfigurationProvider)
                .ToListAsync();

        private async Task<ShoppingCart> GetByProductIdAndUserIdAsync(int productId, string userId)
            => await this
                .AllByUserId(userId)
                .FirstOrDefaultAsync(c => c.ProductId == productId);

        private IQueryable<ShoppingCart> AllByUserId(string userId)
            => this.db
                .ShoppingCarts
                .Where(c => c.UserId == userId);
    }
}