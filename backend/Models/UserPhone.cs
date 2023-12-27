using backend.Infrastructure;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace backend.Models
{
    public class UserPhone
    {
        public long Id { get; init; }
        public string PhoneNumber { get; set; } = null!;

        [NotMapped]
        public long UserId { get; set; }
        public UserDao UserDao { get; set; } = null!;

        public UserPhone(string phoneNumber)
        {
            if (!Regex.IsMatch(phoneNumber, GlobalVariables.RegexPhonePattern))
            {
                throw new ValidationException("Phone number entered incorrectly", nameof(Models.UserPhone));
            }
            PhoneNumber = phoneNumber;
        }


        public override string ToString()
        {
            return this.PhoneNumber;
        }


        public static implicit operator string(UserPhone phoneNumber)
        {
            return phoneNumber.PhoneNumber;
        }

        public override bool Equals(object obj)
        {
            UserPhone phoneNumber = obj as UserPhone;

            if (ReferenceEquals(phoneNumber, null))
                return false;

            return PhoneNumber == phoneNumber.PhoneNumber;
        }

        public override int GetHashCode()
        {
            return PhoneNumber.GetHashCode();
        }

    }
}
