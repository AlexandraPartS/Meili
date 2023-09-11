using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
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
        [StringLength(25, MinimumLength = 2, ErrorMessage = "{0} numbers must be between {2} and {1} character in length.")] 
        public string NickName { get; set; } = null!;

        [BindRequired]
        [Phone]
        public string PhoneNumber { get; set; } = null!;
        public string? CountryResidence { get; set; }
        public string? LocalCurrency { get; set; }
        public string? AvatarRelativePath { get; set; }

    }
}
