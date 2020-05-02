﻿namespace SheryLady.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Data;
    using Data.Models;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Microsoft.EntityFrameworkCore;

    public class ProductsService : IProductsService
    {
        private readonly ApplicationDbContext db;
        private readonly IMapper mapper;
        private readonly IDateTimeProvider dataProvider;

        public ProductsService(
            ApplicationDbContext db,
            IMapper mapper, 
            IDateTimeProvider dataProvider)
        {
            this.db = db;
            this.mapper = mapper;
            this.dataProvider = dataProvider;
        }

        public async Task<int> Create(
            string name,
            string description,
            string image,
            int quantity,
            decimal price,
            int categoryId)
        {
            var product = new Product
            {
                Name = name,
                Description = description,
                Image = image,
                Quantity = quantity,
                Price = price,
                CategoryId = categoryId,
                CreatedOn = this.dataProvider.Now()
            };

            await this.db.Products.AddAsync(product);
            await this.db.SaveChangesAsync();

            return product.Id;
        }

        public async Task<bool> Update(
            int id,
            string name,
            string description,
            string image,
            int quantity,
            decimal price,
            int categoryId)
        {
            var product = await this.GetById(id);
            if (product == null)
            {
                return false;
            }

            product.Name = name;
            product.Description = description;
            product.Image = image;
            product.Quantity = quantity;
            product.Price = price;
            product.CategoryId = categoryId;
            product.ModifiedOn = this.dataProvider.Now();

            await this.db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Delete(int id)
        {
            var product = await this.GetById(id);
            if (product == null)
            {
                return false;
            }

            product.IsDeleted = true;
            product.DeletedOn = this.dataProvider.Now();

            await this.db.SaveChangesAsync();

            return true;
        }

        public async Task<TModel> GetById<TModel>(int id)
            => await this.db
                .Products
                .AsNoTracking()
                .Where(p => p.Id == id && !p.IsDeleted)
                .ProjectTo<TModel>(this.mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

        public async Task<IEnumerable<TModel>> GetAll<TModel>()
            => await this.db
                .Products
                .AsNoTracking()
                .Where(p => !p.IsDeleted)
                .ProjectTo<TModel>(this.mapper.ConfigurationProvider)
                .ToListAsync();

        public async Task<IEnumerable<TModel>> GetAllByCategoryId<TModel>(int categoryId)
            => await this.db
                .Products
                .AsNoTracking()
                .Where(p => p.CategoryId == categoryId && !p.IsDeleted)
                .ProjectTo<TModel>(this.mapper.ConfigurationProvider)
                .ToListAsync();

        private async Task<Product> GetById(int id)
            => await this.db
                .Products
                .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);
    }
}
