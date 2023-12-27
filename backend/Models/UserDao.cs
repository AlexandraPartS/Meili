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
        public long Id { get; init; }

        [Required]
        public string NickName { get; set; } = null!;

        public UserPhone PhoneNumber { get; set; } = null!;

        public string? CountryResidence { get; set; }

        public string? AvatarRelativePath { get; set; }

        [DefaultValue(false)]
        public bool IsDeleted { get; set; }

        public UserDao()
        {
        }

        public UserDao(string nickName, string phoneNumber)
        {
            if (string.IsNullOrEmpty(nickName))
            {
                throw new Infrastructure.ValidationException("Username not specified", "NickName");
            }
            else if (nickName.Length < 2 || nickName.Length > 128)
            {
                throw new Infrastructure.ValidationException("Length numbers must be between 2 and 128 character in length.", "NickName");
            }
            NickName = nickName;
            PhoneNumber = new UserPhone(phoneNumber);
        }

    }
}
