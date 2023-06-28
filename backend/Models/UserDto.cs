using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace backend.Models
{
    /// <summary>
    /// The property names of models UserDao and UserDto must be identical
    /// </summary>
    public record class UserDto
    {
        public long Id { get; init; }

        [BindRequired]
        public string NickName { get; set; } = "";

        [BindRequired]
        public string PhoneNumber { get; set; } = "";
        public string? CountryResidence { get; set; }
        public string? LocalCurrency { get; set; }
        public string? Avatar { get; set; }

        public UserDto() { }

        public UserDto(UserDao userDao)
        {
            if (!userDao.IsDeleted)
            {
                (Id, NickName, PhoneNumber) = (userDao.Id, userDao.NickName, userDao.PhoneNumber);
            }
        }

    }
}
