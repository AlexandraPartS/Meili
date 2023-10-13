using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using backend.Infrastructure;

namespace backend.Models
{
    /// <summary>
    /// The property names of models UserDao and UserDto must be identical
    /// </summary>
    [Index(nameof(NickName), IsUnique = true)]
    public class UserDao 
    {
        [Column("id")]
        public long Id { get; init; }

        [Column("nickname")]
        [BindRequired]//(ErrorMessage = "Username not specified")
        [StringLength(128, MinimumLength = 2, ErrorMessage = "{0} numbers must be between {2} and {1} character in length.")]
        public string NickName { get; set; } = null!;

        [Column("phonenumber")]
        [Required]
        [RegularExpression(GlobalVariables.RegexPhonePattern, ErrorMessage = "Wrong phone number.")]
        public string PhoneNumber { get; set; } = null!;

        [Column("countryresidence")]
        public string? CountryResidence { get; set; }

        [Column("avatarrelativepath")]
        public string? AvatarRelativePath { get; set; }

        [Column("isdeleted")]
        [DefaultValue(false)]
        public bool IsDeleted { get; set; }

    }
}
