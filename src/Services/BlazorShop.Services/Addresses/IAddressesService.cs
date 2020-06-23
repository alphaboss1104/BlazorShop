﻿namespace BlazorShop.Services.Addresses
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Web.Shared.Addresses;

    public interface IAddressesService
    {
        Task<int> CreateAsync(
            string country,
            string state,
            string city,
            string description,
            int postalCode,
            string phoneNumber,
            string userId);

        Task<bool> DeleteAsync(int id);

        Task<IEnumerable<AddressListingResponseModel>> GetAllByUserIdAsync(string userId);
    }
}