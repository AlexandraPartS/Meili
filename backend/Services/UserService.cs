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

        public UserService(DataStorage<UserDao> dataStorage)
        {
            _dataStorage = dataStorage;
        }

        public async Task<UserDto> CreateUser(UserDto userDto)
        {
            try
            {
                var user = ItemToDAO(userDto);
                await _dataStorage.Save(user);
                return ItemToDTO(user);
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
                .Select(x => ItemToDTO(x))
                .ToList();
        }

        public async Task<UserDto> GetUser(long id)
        {
            if (id < 0)
            {
                throw new ValidationException("Invalid value of Id.", "Id");
            }
            var user = await _dataStorage.Get(id);
            ValidationUser(user);
            return ItemToDTO(user);
        }

        public async Task UpdateUser(UserDto userDto)
        {
            var id = userDto.Id;
            var user = await _dataStorage.Get(id);
            ValidationUser(user);
            MapItemToDAO(userDto, user);
            await UpdateUserDataAndCatchExceptions(user, id);
        }

        async Task IUserService.DeleteUser(long id)
        {
            var user = await _dataStorage.Get(id);
            ValidationUser(user);
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

        private static UserDto ItemToDTO(UserDao user) 
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<UserDao, UserDto>());
            var mapper = new Mapper(config);
            UserDto _userDto = mapper.Map<UserDao, UserDto>(user);
            return _userDto;
        }

        private static UserDao ItemToDAO(UserDto userDto) 
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<UserDto, UserDao>());
            var mapper = new Mapper(config);
            UserDao _user = mapper.Map<UserDto,UserDao>(userDto);
            return _user;
        }
        private static void MapItemToDAO(UserDto userDto, UserDao user) 
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<UserDto, UserDao>());
            var mapper = new Mapper(config);
            mapper.Map(userDto, user);
        }

        private  bool IsUserExists(long id)
        {
            return _dataStorage.Get(id) != null;
        }

        public void ValidationUser(UserDao user)
        {
            if (user == null || user.IsDeleted)
            {
                throw new ObjectNotFoundException("User is not found.", "");
            }
        }
    }
}
