﻿namespace BlazorShop.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Shared.Models.Products;

    public interface IWishlistsService
    {
        Task AddAsync(int productId, string userId);

        Task<bool> RemoveAsync(int productId, string userId);

        Task<IEnumerable<ProductsListingResponseModel>> GetByUserIdAsync(string userId);
    }
}
