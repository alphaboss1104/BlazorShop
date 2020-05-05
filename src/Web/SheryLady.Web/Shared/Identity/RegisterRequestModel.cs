﻿namespace SheryLady.Web.Shared.Identity
{
    using System.ComponentModel.DataAnnotations;

    using static Common.ErrorMessages;
    using static Common.ModelConstants;

    public class RegisterRequestModel
    {
        [Required]
        [MaxLength(UserFirstNameMaxLength)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(UserLastNameMaxLength)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(UserUserNameMaxLength, ErrorMessage = StringLengthErrorMessage, MinimumLength = UserUserNameMinLength)]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
