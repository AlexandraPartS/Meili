using Microsoft.AspNetCore.Mvc.ModelBinding;
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
        [StringLength(100)]
        public string NickName { get; set; } = null!;

        [BindRequired]
        [Phone]
        public string PhoneNumber { get; set; } = null!;
        public string? CountryResidence { get; set; }
        public string? LocalCurrency { get; set; }
        public string? Avatar { get; set; }

    }
}
