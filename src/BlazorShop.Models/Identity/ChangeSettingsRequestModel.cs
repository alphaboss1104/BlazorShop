﻿namespace BlazorShop.Models.Identity
{
    using System.ComponentModel.DataAnnotations;

    using static ErrorMessages;
    using static Data.ModelConstants.Common;

    public class ChangeSettingsRequestModel
    {
        public string UserId { get; set; }

        [Required]
        [StringLength(
            MaxNameLength,
            ErrorMessage = StringLengthErrorMessage,
            MinimumLength = MinNameLength)]
        public string Username { get; set; }

        [Required]
        [StringLength(
            MaxNameLength,
            ErrorMessage = StringLengthErrorMessage,
            MinimumLength = MinNameLength)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(
            MaxNameLength,
            ErrorMessage = StringLengthErrorMessage,
            MinimumLength = MinNameLength)]
        public string LastName { get; set; }
    }
}