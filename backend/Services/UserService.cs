using backend.Interfaces;
using backend.Models;
using backend.Infrastructure;
using backend.Data;
using Microsoft.EntityFrameworkCore;

namespace backend.Services
{
    public class UserService : IUserService
    {
        private readonly UserContext _dbcontext;

        public UserService(UserContext context)
        {
            _dbcontext = context;
        }

        public async Task<UserDto> CreateUser(UserDto userDto)
        {
            //Validation1
            if (_dbcontext.Users == null)
            {
                throw new ValidationException("Database is not available.", ""); 
            }
            //Validation2
            if (_dbcontext.Users.Any(i => i.PhoneNumber == userDto.PhoneNumber))
            {
                throw new ValidationException("The Phone number is already in use.", "PhoneNumber"); 
            }

            var user = new UserDao(userDto);
            _dbcontext.Users.Add(user);
            await _dbcontext.SaveChangesAsync().ConfigureAwait(false);

            return ItemToDTO(user);
        }

        public async Task<IEnumerable<UserDto>> GetUsers()
        {
            //Validation1
            if (_dbcontext.Users == null)
            {
                throw new ValidationException("Database is not available.", "");
            }

            return await _dbcontext.Users.Where(x => x.IsDeleted == false)
                .Select(x => ItemToDTO(x))
                .ToListAsync()
                .ConfigureAwait(false);
        }

        public async Task<UserDto> GetUser(long id)
        {
            //Validation1
            if (id < 0)
            {
                throw new ValidationException("Invalid value of Id.", "Id");
            }
            //Validation2
            if (_dbcontext.Users == null)
            {
                throw new ValidationException("Database is not available.", "");
            }

            var user = await _dbcontext.Users.FindAsync(id).ConfigureAwait(false);
            //Validation3
            if (user == null)
            {
                throw new ValidationException("User with Id not found.", "Id");
            }
            //Validation4
            if (user.IsDeleted)
            {
                throw new ValidationException("User deleted.", "IsDeleted");
            }

            return ItemToDTO(user);
        }

        public async Task UpdateUser(UserDto userDto)
        {
            var id = userDto.Id;
            var user = await _dbcontext.Users.FindAsync(id).ConfigureAwait(false);
            //Validation1
            if (user.IsDeleted)
            {
                throw new ValidationException("User deleted.", "IsDeleted");
            }

            SetItemDAONewPropValue(user, userDto);

            try
            {
                await _dbcontext.SaveChangesAsync().ConfigureAwait(false);
            }
            catch (DbUpdateConcurrencyException) when (!IsUserExists(id))
            {
                throw new ValidationException("User deleted.", "IsDeleted");
            }
        }

        async Task IUserService.DeleteUser(long id)
        {
            if (_dbcontext.Users == null)
            {
                throw new ValidationException("Database is not available.", "");
            }

            var user = await _dbcontext.Users.FindAsync(id).ConfigureAwait(false);
            if (user == null)
            {
                throw new ValidationException("User with Id not found.", "Id");
            }

            user.IsDeleted = true;
            await _dbcontext.SaveChangesAsync().ConfigureAwait(false);
        }

        public void Dispose()
        {
            _dbcontext.Dispose();
        }


        private static UserDto ItemToDTO(UserDao user) => new UserDto(user);

        private bool IsUserExists(long id)
        {
            return (_dbcontext.Users?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private static UserDao SetItemDAONewPropValue(UserDao user, UserDto userDto)
        {
            foreach (var propDao in typeof(UserDao).GetProperties())
            {
                foreach (var propDto in typeof(UserDto).GetProperties())
                {
                    if (propDao.Name == propDto.Name)
                    {
                        propDao.SetValue(user, propDto.GetValue(userDto));
                    }
                }
            }
            return user;
        }

    }
}
