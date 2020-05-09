﻿namespace JewelleryShop.Web.Client.Infrastructure.Extensions
{
    using System;
    using System.Net.Http;

    using Microsoft.AspNetCore.Components.Authorization;
    using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
    using Microsoft.Extensions.DependencyInjection;

    using Services.Client;

    public static class WebAssemblyHostBuilderExtensions
    {
        public static WebAssemblyHostBuilder AddRootComponents(this WebAssemblyHostBuilder builder)
        {
            builder.RootComponents.Add<App>("app");
            return builder;
        }

        public static WebAssemblyHostBuilder AddClientServices(this WebAssemblyHostBuilder builder)
        {
            builder
                .Services
                .AddAuthorizationCore()
                .AddScoped<TokenAuthenticationStateProvider>()
                .AddScoped<AuthenticationStateProvider>(provider => provider
                    .GetRequiredService<TokenAuthenticationStateProvider>())
                .AddTransient<IAuthService, AuthService>()
                .AddTransient(sp => new HttpClient 
                { 
                    BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) 
                });

            return builder;
        }
    }
}
