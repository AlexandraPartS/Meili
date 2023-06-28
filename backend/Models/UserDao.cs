using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace backend.Models
{
    /// <summary>
    /// The property names of models UserDao and UserDto must be identical
    /// </summary>
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
        public string? Avatar { get; set; }

        [DefaultValue(false)]
        public bool IsDeleted { get; set; }

        public UserDao() { }

        public UserDao(UserDto userDto) =>
            (NickName, PhoneNumber, CountryResidence, Avatar) = (userDto.NickName, userDto.PhoneNumber, userDto.CountryResidence, userDto.Avatar);

    }
}
