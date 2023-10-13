using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Models;
using backend.Interfaces;
using backend.Infrastructure;
using NuGet.Protocol;
using Newtonsoft.Json;
using System.Xml.Linq;

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
            try
            {
                return Ok(await _userService.GetUsers().ConfigureAwait(false));
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelError(ex.Property, ex.Message);
                return NotFound(ModelState);
            }
        }

        // GET: api/User/5
        [HttpGet("{userId:long}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserDto>> GetUser(long userId)
        {
            try
            {
                var user = await _userService.GetUser(userId).ConfigureAwait(false);
                return user;
            }
            catch(ValidationException ex)
            {
                ModelState.AddModelError(ex.Property, ex.Message);
                return NotFound(ModelState);
            }
            catch(ObjectNotFoundException ex)
            {
                ModelState.AddModelError(ex.Property, ex.Message);
                return NotFound(ModelState);
            }
        }

        // PUT: api/User/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutUserDto(UserDto userDto)
        {
            Console.WriteLine("Usual edit controller");
            if (ModelState.IsValid)
            {
                try
                {
                    await _userService.UpdateUser(userDto).ConfigureAwait(false);
                    return NoContent();
                }
                catch (ValidationException ex)
                {
                    ModelState.AddModelError(ex.Property, ex.Message);
                    return NotFound(ModelState);
                }
                catch (ObjectNotFoundException ex)
                {
                    ModelState.AddModelError(ex.Property, ex.Message);
                    return NotFound(ModelState);
                }
            }
            return NotFound(ModelState);
        }

        // PUT: api/user/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutUserDto(IFormCollection collection)
        {
            Console.WriteLine("IFormCollection edit controller");

            try
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
            catch (ValidationException ex)
            {
                ModelState.AddModelError(ex.Property, ex.Message);
                return NotFound(ModelState);
            }
            catch (ObjectNotFoundException ex)
            {
                ModelState.AddModelError(ex.Property, ex.Message);
                return NotFound(ModelState);
            }
            catch(ArgumentNullException ex) 
            {
                ModelState.AddModelError(ex.ParamName, ex.Message);
                return BadRequest(ModelState);
            }
        }

        // POST: api/User
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserDto>> Create(UserDto userDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userService.CreateUser(userDto).ConfigureAwait(false);
                    return CreatedAtAction(
                                nameof(Create),
                                new { id = user.Id },
                                user);
                }
                catch (ValidationException ex)
                {
                    ModelState.AddModelError(ex.Property, ex.Message);
                    return NotFound(ModelState);
                }
            }
            else
            {
                return NotFound(ModelState);
            }
        }

        // DELETE: api/User/5
        [HttpDelete("{userId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(long userId)
        {
            try
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
            catch(ValidationException ex)
            {
                ModelState.AddModelError(ex.Property, ex.Message);
                return NotFound(ModelState);
            }
        }

        protected override void Dispose(bool disposing)
        {
            _userService.Dispose();
            base.Dispose(disposing);
        }
    }
}
