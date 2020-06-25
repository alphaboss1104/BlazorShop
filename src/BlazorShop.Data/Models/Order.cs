﻿namespace BlazorShop.Data.Models
{
    using System;

    using Contracts;

    public class Order : IAuditInfo
    {
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public int ProductId { get; set; }

        public Product Product { get; set; }

        public int Quantity { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public int DeliveryAddressId { get; set; }

        public Address DeliveryAddress { get; set; }
    }
}
