using backend.Interfaces;
using backend.Models;
using backend.Infrastructure;
using backend.Data;
using Microsoft.EntityFrameworkCore;
using System.Data;
using AutoMapper;

namespace backend.Services
{
    public class UserService : IUserService
    {
        private readonly DataStorage<UserDao> _dataStorage;
        private readonly Mapper _mapper;

        public UserService(DataStorage<UserDao> dataStorage, Mapper mapper)
        {
            _dataStorage = dataStorage;
            _mapper = mapper;
        }

        public async Task<UserDto> CreateUser(UserDto userDto)
        {
            try
            {
                var user = _mapper.Map<UserDao>(userDto); 
                await _dataStorage.Save(user);
                return _mapper.Map<UserDto>(user);
            }
            catch
            {
                throw new ValidationException("The Login is already in use.", "NickName");
            }
        }

        public async Task<IEnumerable<UserDto>> GetUsers()
        {
            return (await _dataStorage.Get())
                .Where(x => x.IsDeleted == false)
                .Include(u => u.PhoneNumber)
                .Select(x => _mapper.Map<UserDto>(x))
                .ToList();
        }

        public async Task<UserDto> GetUser(long id)
        {
            if (id < 0)
            {
                throw new ValidationException("Invalid value of Id.", "Id");
            }
            var user = (await _dataStorage.Get())
                .Where(x => x.Id == id)
                .Include(u => u.PhoneNumber).First();

            ValidationUserDoesNotExist(user);
            return _mapper.Map<UserDao, UserDto>(user);
        }

        public async Task UpdateUser(UserDto userDto)
        {
            var id = userDto.Id;
            var user = await _dataStorage.Get(id);
            ValidationUserDoesNotExist(user);
            _mapper.Map(userDto, user);
            await UpdateUserDataAndCatchExceptions(user, id);
        }

        async Task IUserService.DeleteUser(long id)
        {
            var user = await _dataStorage.Get(id);
            ValidationUserDoesNotExist(user);
            user.IsDeleted = true;
            await UpdateUserDataAndCatchExceptions(user, id);
        }

        public async Task UpdateUserDataAndCatchExceptions(UserDao user, long id)
        {
            try
            {
                await _dataStorage.Update(user);
            }
            catch (DbUpdateConcurrencyException) when (!IsUserExists(id))
            {
                throw new ObjectNotFoundException("User is not found.", "");
            }
            catch(DbUpdateException)
            {
                throw new ValidationException("The Login is already in use.", "NickName");
            }
        }

        public void Dispose()
        {
            _dataStorage.Dispose(); 
        }

        private  bool IsUserExists(long id)
        {
            return _dataStorage.Get(id) != null;
        }

        public void ValidationUserDoesNotExist(UserDao user) 
        {
            if (user == null || user.IsDeleted)
            {
                throw new ObjectNotFoundException("User is not found.", "");
            }
        }
    }
}
