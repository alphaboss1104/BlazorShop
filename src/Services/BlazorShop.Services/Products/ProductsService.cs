﻿namespace BlazorShop.Services.Products
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using Data;
    using Data.Models;
    using DateTime;
    using Mapping;
    using Web.Shared.Products;

    public class ProductsService : IProductsService
    {
        private readonly ApplicationDbContext db;
        private readonly IDateTimeProvider dataProvider;

        public ProductsService(
            ApplicationDbContext db,
            IDateTimeProvider dataProvider)
        {
            this.db = db;
            this.dataProvider = dataProvider;
        }

        public async Task<int> CreateAsync(
            string name,
            string description,
            string imageSource,
            int quantity,
            decimal price,
            int categoryId)
        {
            var product = new Product
            {
                Name = name,
                Description = description,
                ImageSource = imageSource,
                Quantity = quantity,
                Price = price,
                CategoryId = categoryId,
                CreatedOn = this.dataProvider.Now()
            };

            await this.db.Products.AddAsync(product);
            await this.db.SaveChangesAsync();

            return product.Id;
        }

        public async Task<bool> UpdateAsync(
            int id,
            string name,
            string description,
            string imageSource,
            int quantity,
            decimal price,
            int categoryId)
        {
            var product = await this.GetByIdAsync(id);
            if (product == null)
            {
                return false;
            }

            product.Name = name;
            product.Description = description;
            product.ImageSource = imageSource;
            product.Quantity = quantity;
            product.Price = price;
            product.CategoryId = categoryId;
            product.ModifiedOn = this.dataProvider.Now();

            await this.db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var product = await this.GetByIdAsync(id);
            if (product == null)
            {
                return false;
            }

            product.IsDeleted = true;
            product.DeletedOn = this.dataProvider.Now();

            await this.db.SaveChangesAsync();

            return true;
        }

        public async Task<ProductsDetailsResponseModel> DetailsAsync(int id)
            => await this
                .All()
                .Where(p => p.Id == id)
                .To<ProductsDetailsResponseModel>()
                .FirstOrDefaultAsync();

        public async Task<IEnumerable<ProductsListingResponseModel>> GetAllAsync()
            => await this
                .All()
                .To<ProductsListingResponseModel>()
                .ToListAsync();

        public async Task<IEnumerable<ProductsListingResponseModel>> GetAllByCategoryIdAsync(int categoryId)
            => await this
                .All()
                .Where(p => p.CategoryId == categoryId)
                .To<ProductsListingResponseModel>()
                .ToListAsync();

        private async Task<Product> GetByIdAsync(int id)
            => await this.db
                .Products
                .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);

        private IQueryable<Product> All()
            => this.db
                .Products
                .AsNoTracking()
                .Where(p => !p.IsDeleted);
    }
}
