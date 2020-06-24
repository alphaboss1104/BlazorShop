﻿namespace BlazorShop.Web.Server.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;

    using Data.Models;
    using Infrastructure.Extensions;
    using Services;
    using Shared.Identity;

    public class IdentityController : ApiController
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IIdentityService identityService;
        private readonly IConfiguration configuration;

        public IdentityController(
            UserManager<ApplicationUser> userManager,
            IIdentityService identityService,
            IConfiguration configuration)
        {
            this.userManager = userManager;
            this.identityService = identityService;
            this.configuration = configuration;
        }

        [HttpPost(nameof(Register))]
        public async Task<ActionResult> Register(RegisterRequestModel model)
        {
            var result = await this.identityService.CreateAsync(
                model.Username, 
                model.Email, 
                model.Password);

            if (result.Succeeded)
            {
                return Ok();
            }

            return BadRequest(result.Errors);
        }

        [HttpPost(nameof(Login))]
        public async Task<ActionResult<LoginResponseModel>> Login(LoginRequestModel model)
        {
            var user = await this.userManager.FindByNameAsync(model.Username);
            if (user == null)
            {
                return Unauthorized();
            }

            var passwordValid = await this.userManager.CheckPasswordAsync(user, model.Password);
            if (!passwordValid)
            {
                return Unauthorized();
            }

            var token = await this.identityService.GenerateJwtAsync(
                user.Id,
                user.UserName,
                this.configuration.GetJwtKey(),
                this.configuration.GetJwtIssuer(),
                this.configuration.GetJwtAudience());

            return new LoginResponseModel { Token = token };
        }

        [HttpPost(nameof(ChangePassword))]
        [Authorize]
        public async Task<ActionResult> ChangePassword(ChangePasswordRequestModel model)
        {
            var result = await this.identityService.ChangePassword(
                this.User.GetId(),
                model.Password,
                model.NewPassword);

            if (result.Succeeded)
            {
                return Ok();
            }

            return BadRequest(result.Errors);
        }
    }
}