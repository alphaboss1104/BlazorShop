﻿namespace BlazorShop.Web.Server.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using Infrastructure.Extensions;
    using Models.Products;
    using Services.Products;

    using static Common.Constants;

    public class ProductsController : ApiController
    {
        private readonly IProductsService productsService;

        public ProductsController(IProductsService productsService)
            => this.productsService = productsService;

        [HttpGet]
        public async Task<IEnumerable<ProductsListingResponseModel>> All(int page = 1)
            => await this
                .productsService
                .AllAsync(page);

        [HttpGet(Id)]
        public async Task<ActionResult<ProductsDetailsResponseModel>> Details(int id)
            => await this
                .productsService
                .DetailsAsync(id);

        [HttpPost]
        [Authorize(Roles = AdministratorRole)]
        public async Task<ActionResult> Create(ProductsRequestModel model)
        {
            var id = await this.productsService.CreateAsync(
                model.Name,
                model.Description,
                model.ImageSource,
                model.Quantity,
                model.Price,
                model.CategoryId);

            return Created(nameof(this.Create), id);
        }

        [HttpPut(Id)]
        [Authorize(Roles = AdministratorRole)]
        public async Task<ActionResult> Update(int id, ProductsRequestModel model)
            => await this
                .productsService
                .UpdateAsync(
                    id,
                    model.Name,
                    model.Description,
                    model.ImageSource,
                    model.Quantity,
                    model.Price,
                    model.CategoryId)
                .ToActionResult();

        [HttpDelete(Id)]
        [Authorize(Roles = AdministratorRole)]
        public async Task<ActionResult> Delete(int id)
            => await this
                .productsService
                .DeleteAsync(id)
                .ToActionResult();
    }
}