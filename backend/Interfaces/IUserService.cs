using backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace backend.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> CreateUser(UserDto userDto);
        Task<UserDto> GetUser(long id);
        Task<IEnumerable<UserDto>> GetUsers();
        Task UpdateUser(UserDto userDto);
        Task DeleteUser(long id);
        void Dispose();
    }
}
