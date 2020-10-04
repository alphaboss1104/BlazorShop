﻿namespace BlazorShop.Web.Client.Shared.Products
{
    using System.Collections.Generic;
    using System.Net.Http.Json;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Components;
    using Microsoft.JSInterop;

    using Models.ShoppingCarts;

    public partial class AddToCartForm
    {
        private readonly ShoppingCartRequestModel model = new ShoppingCartRequestModel();

        public bool ShowErrors { get; set; }

        public IEnumerable<string> Errors { get; set; }

        [Parameter]
        public int ProductId { get; set; }

        [Parameter]
        public string ProductName { get; set; }

        [Parameter]
        public int ProductQuantity { get; set; }

        [Inject]
        protected IJSRuntime JsRuntime { get; set; }

        private async Task OnSubmitAsync()
        {
            this.model.ProductId = this.ProductId;

            var response = await this.Http.PostAsJsonAsync("api/shoppingcarts/AddProduct", this.model);

            if (!response.IsSuccessStatusCode)
            {
                this.Errors = await response.Content.ReadFromJsonAsync<string[]>();
                this.ShowErrors = true;
            }
            else
            {
                this.ShowErrors = false;
                this.NavigationManager.NavigateTo("/cart", forceLoad: true);
            }
        }

        private async Task OnDeleteAsync()
        {
            var confirmed = await this.JsRuntime.InvokeAsync<bool>(
                "confirm",
                "Are you sure you want to delete this item?");

            if (confirmed)
            {
                var result = await this.ProductsService.DeleteAsync(this.ProductId);

                if (result.Succeeded)
                {
                    this.ToastService.ShowSuccess($"{this.ProductName} has been deleted successfully.");
                    this.NavigationManager.NavigateTo("/products/page/1");
                }
            }
        }

        private async Task AddToWishlist()
        {
            await this.Http.PostAsJsonAsync($"api/wishlists/AddProduct/{this.ProductId}", this.ProductId);
            this.ToastService.ShowSuccess($"{this.ProductName} has been added to your wishlist.");
        }

        private void IncrementQuantity()
        {
            if (this.model.Quantity < this.ProductQuantity)
            {
                this.model.Quantity++;
                this.ShowErrors = false;
            }
        }

        private void DecrementQuantity()
        {
            if (this.model.Quantity > 1)
            {
                this.model.Quantity--;
                this.ShowErrors = false;
            }
        }
    }
}
