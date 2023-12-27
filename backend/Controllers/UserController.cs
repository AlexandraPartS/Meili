using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Models;
using backend.Interfaces;
using NuGet.Protocol;
using Newtonsoft.Json;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class UserController : Controller
    {
        private IUserService _userService;
        private ILogger _logger;
        private IFileService _fileService;

        public UserController(IUserService serv, IFileService fileserv, ILogger<UserController> logger)
        {
            _userService = serv;
            _fileService = fileserv;
            _logger = logger;
        }

        // GET: api/User
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<UserDto>>> Get()
        {
            return Ok(await _userService.GetUsers().ConfigureAwait(false));
        }

        // GET: api/User/5
        [HttpGet("{userId:long}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserDto>> GetUser(long userId)
        {
            var user = await _userService.GetUser(userId).ConfigureAwait(false);
            return user;
        }

        // PUT: api/User/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutUserDto(UserDto userDto)
        {
            await _userService.UpdateUser(userDto).ConfigureAwait(false);
            return NoContent();
        }

        // PUT: api/user/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutUserDto(IFormCollection collection)
        {
            UserDto? userDto = JsonConvert.DeserializeObject<UserDto>(collection["userDto"]);
            if(userDto is null)
            {
                throw new ArgumentNullException(nameof(userDto));
            }
            try
            {
                if (collection.Files.Count > 0)
                {
                    await _fileService.CreateIdUserFolderAsync(userDto.Id);
                    await _fileService.WriteFileAsync(userDto.Id, collection.Files[0]);
                }
                else
                {
                    _fileService.CleanFolderOfAvatar(userDto.Id);
                }
            }
            catch (IOException ex)
            {
                _logger.Log(LogLevel.Warning, ex.ToString());
            }
            await _userService.UpdateUser(userDto).ConfigureAwait(false);
            return Ok(userDto.ToJson());
        }

        // POST: api/User
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserDto>> Create(UserDto userDto)
        {
            var user = await _userService.CreateUser(userDto).ConfigureAwait(false);
            return CreatedAtAction(
                        nameof(Create),
                        new { id = user.Id },
                        user);
        }

        // DELETE: api/User/5
        [HttpDelete("{userId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(long userId)
        {
            await _userService.DeleteUser(userId).ConfigureAwait(false);
            try
            {
                await _fileService.DeleteIdUserFolderAsync(userId);
            }
            catch (IOException ex)
            {
                _logger.Log(LogLevel.Warning, ex.ToString());
            }
            return NoContent();
        }

        protected override void Dispose(bool disposing)
        {
            _userService.Dispose();
            base.Dispose(disposing);
        }
    }
}
