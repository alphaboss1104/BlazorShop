﻿namespace BlazorShop.Web.Server.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using Infrastructure.Extensions;
    using Models.Wishlists;
    using Services.Identity;
    using Services.Wishlists;

    [Authorize]
    public class WishlistsController : ApiController
    {
        private readonly IWishlistsService wishlists;
        private readonly ICurrentUserService currentUser;

        public WishlistsController(
            IWishlistsService wishlists,
            ICurrentUserService currentUser)
        {
            this.wishlists = wishlists;
            this.currentUser = currentUser;
        }

        [HttpGet]
        public async Task<IEnumerable<WishlistsProductsResponseModel>> Mine()
            => await this.wishlists.ByUserAsync(this.currentUser.UserId);

        [HttpPost(Id)]
        public async Task<ActionResult> AddProduct(int id)
            => await this.wishlists
                .AddProductAsync(id, this.currentUser.UserId)
                .ToActionResult();

        [HttpDelete(Id)]
        public async Task<ActionResult> RemoveProduct(int id)
            => await this.wishlists
                .RemoveProductAsync(id, this.currentUser.UserId)
                .ToActionResult();
    }
}
