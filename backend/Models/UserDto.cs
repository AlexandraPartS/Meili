using backend.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    /// <summary>
    /// The property names of models UserDao and UserDto must be identical
    /// </summary>
    public record class UserDto
    {
        public long Id { get; init; }

        [BindRequired]
        [StringLength(128, MinimumLength = 2, ErrorMessage = "{0} numbers must be between {2} and {1} character in length.")]
        public string NickName { get; set; } = null!;

        [BindRequired]
        [RegularExpression(GlobalVariables.RegexPhonePattern, ErrorMessage = "Wrong phone number")]
        public string PhoneNumber { get; set; } = null!;
        public string? CountryResidence { get; set; }
        public string? LocalCurrency { get; set; }
        public string? AvatarRelativePath { get; set; }

    }
}
