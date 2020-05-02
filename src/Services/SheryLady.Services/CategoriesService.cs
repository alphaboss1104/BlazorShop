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

    public class CategoriesService : ICategoriesService
    {
        private readonly ApplicationDbContext db;
        private readonly IMapper mapper;
        private readonly IDateTimeProvider dateTimeProvider;

        public CategoriesService(
            ApplicationDbContext db,
            IMapper mapper, 
            IDateTimeProvider dateTimeProvider)
        {
            this.db = db;
            this.mapper = mapper;
            this.dateTimeProvider = dateTimeProvider;
        }

        public async Task<int> Create(string name)
        {
            var category = new Category
            {
                Name = name,
                CreatedOn = this.dateTimeProvider.Now()
            };

            await this.db.Categories.AddAsync(category);
            await this.db.SaveChangesAsync();

            return category.Id;
        }

        public async Task<bool> Update(int id, string name)
        {
            var category = await this.GetById(id);
            if (category == null)
            {
                return false;
            }

            category.Name = name;
            category.ModifiedOn = this.dateTimeProvider.Now();

            await this.db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Delete(int id)
        {
            var category = await this.GetById(id);
            if (category == null)
            {
                return false;
            }

            category.IsDeleted = true;
            category.DeletedOn = this.dateTimeProvider.Now();

            await this.db.SaveChangesAsync();

            return true;
        }

        public async Task<TModel> GetById<TModel>(int id)
            => await this.db
                .Categories
                .AsNoTracking()
                .Where(c => c.Id == id && !c.IsDeleted)
                .ProjectTo<TModel>(this.mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

        public async Task<IEnumerable<TModel>> GetAll<TModel>()
            => await this.db
                .Categories
                .AsNoTracking()
                .Where(c => !c.IsDeleted)
                .ProjectTo<TModel>(this.mapper.ConfigurationProvider)
                .ToListAsync();

        private async Task<Category> GetById(int id)
            => await this.db
                .Categories
                .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);
    }
}
