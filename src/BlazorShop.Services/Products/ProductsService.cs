﻿namespace BlazorShop.Services.Products
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;
    using Microsoft.EntityFrameworkCore;

    using Data;
    using Data.Models;
    using Models;
    using Models.Products;
    using Specifications;
    using Specifications.Products;

    using static Common.Constants;

    public class ProductsService : BaseService<Product>, IProductsService
    {
        public ProductsService(ApplicationDbContext db, IMapper mapper)
            : base(db, mapper)
        {
        }

        public async Task<int> CreateAsync(ProductsRequestModel model)
        {
            var product = new Product
            {
                Name = model.Name,
                Description = model.Description,
                ImageSource = model.ImageSource,
                Quantity = model.Quantity,
                Price = model.Price,
                CategoryId = model.CategoryId
            };

            await this.Data.AddAsync(product);
            await this.Data.SaveChangesAsync();

            return product.Id;
        }

        public async Task<Result> UpdateAsync(
            int id, ProductsRequestModel model)
        {
            var product = await this.FindByIdAsync(id);

            if (product == null)
            {
                return false;
            }

            product.Name = model.Name;
            product.Description = model.Description;
            product.ImageSource = model.ImageSource;
            product.Quantity = model.Quantity;
            product.Price = model.Price;
            product.CategoryId = model.CategoryId;

            await this.Data.SaveChangesAsync();

            return true;
        }

        public async Task<Result> DeleteAsync(int id)
        {
            var product = await this.FindByIdAsync(id);

            if (product == null)
            {
                return false;
            }

            this.Data.Remove(product);

            await this.Data.SaveChangesAsync();

            return true;
        }

        public async Task<ProductsDetailsResponseModel> DetailsAsync(
            int id)
            => await this.Mapper
                .ProjectTo<ProductsDetailsResponseModel>(this
                    .AllAsNoTracking()
                    .Where(p => p.Id == id))
                .FirstOrDefaultAsync();

        public async Task<IEnumerable<ProductsListingResponseModel>> SearchAsync(
            ProductsSearchRequestModel model)
            => await this.Mapper
                .ProjectTo<ProductsListingResponseModel>(this
                    .AllAsNoTracking()
                    .Where(this.GetProductSpecification(model))
                    .Skip((model.Page - 1) * ItemsPerPage)
                    .Take(ItemsPerPage))
                .ToListAsync();

        private async Task<Product> FindByIdAsync(
            int id)
            => await this
                .All()
                .Where(p => p.Id == id)
                .FirstOrDefaultAsync();

        private Specification<Product> GetProductSpecification(
            ProductsSearchRequestModel model)
            => new ProductByNameSpecification(model.Query)
                .And(new ProductByPriceSpecification(model.MinPrice, model.MaxPrice))
                .And(new ProductByCategorySpecification(model.Category));
    }
}
