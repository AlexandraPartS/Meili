using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Microsoft.EntityFrameworkCore;

namespace backend.Models
{
    /// <summary>
    /// The property names of models UserDao and UserDto must be identical
    /// </summary>
    [Index(nameof(NickName), IsUnique = true)]
    public class UserDao
    {
        public long Id { get; init; }

        [BindRequired]
        [StringLength(100)]
        public string NickName { get; set; } = null!;

        [BindRequired]
        [Phone]
        public string PhoneNumber { get; set; } = null!;

        public string? CountryResidence { get; set; }
        public string? AvatarRelativePath { get; set; }

        [DefaultValue(false)]
        public bool IsDeleted { get; set; }

    }
}
