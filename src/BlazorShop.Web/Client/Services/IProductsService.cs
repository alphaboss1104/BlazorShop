﻿namespace BlazorShop.Web.Client.Services
{
    using System.Threading.Tasks;

    using Models;
    using Models.Products;

    public interface IProductsService
    {
        Task<int> CreateAsync(ProductsRequestModel model);

        Task<Result> UpdateAsync(int id, ProductsRequestModel model);

        Task<Result> DeleteAsync(int id);

        Task<ProductsDetailsResponseModel> DetailsAsync(int id);
    }
}
