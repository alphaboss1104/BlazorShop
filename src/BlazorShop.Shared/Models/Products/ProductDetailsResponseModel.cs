﻿namespace BlazorShop.Shared.Models.Products
{
    public class ProductDetailsResponseModel : ProductListingResponseModel
    {
        public string Description { get; set; }

        public int CategoryId { get; set; }

        public string CategoryName { get; set; }
    }
}
