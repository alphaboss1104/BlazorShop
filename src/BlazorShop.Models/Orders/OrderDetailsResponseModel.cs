﻿namespace BlazorShop.Models.Orders
{
    using System.Collections.Generic;

    using AutoMapper;

    using Data.Models;
    using Products;

    public class OrderDetailsResponseModel : OrderListingResponseModel
    {
        public IEnumerable<ProductListingResponseModel> Products { get; set; }

        public override void Mapping(Profile profile) 
            => profile
                .CreateMap<OrderProduct, ProductListingResponseModel>()
                .ForMember(m => m.Id, m => m.MapFrom(op => op.Product.Id))
                .ForMember(m => m.Name, m => m.MapFrom(op => op.Product.Name))
                .ForMember(m => m.ImageSource, m => m.MapFrom(op => op.Product.ImageSource))
                .ForMember(m => m.Quantity, m => m.MapFrom(op => op.Product.Quantity))
                .ForMember(m => m.Price, m => m.MapFrom(op => op.Product.Price));
    }
}
