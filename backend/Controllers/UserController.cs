using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.Models;
using backend.Interfaces;
using backend.Infrastructure;
using backend.Services;
using NuGet.Protocol;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class UserController : Controller
    {
        private IUserService _userService;
        IWebHostEnvironment _appEnvironment;

        public UserController(IUserService serv, IWebHostEnvironment appEnvironment)
        {
            _userService = serv;
            _appEnvironment = appEnvironment;
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
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutUserDto(IFormCollection collection)
        {
            try
            {
                string avatarPath = null;

                UserDto userDto = new UserDto
                {
                    Id = (long)Convert.ToDouble(collection["Id"]),
                    NickName = collection["NickName"],
                    PhoneNumber = collection["PhoneNumber"],
                    Avatar = avatarPath
                };

                if (collection.Files.Count > 0)
                {
                    IFormFile uploadedFile = collection.Files[0];
                    avatarPath = "/Files/avatar" + Path.GetExtension(uploadedFile.FileName);
                    userDto.Avatar = avatarPath;
                    //delete all before write?//1
                    using (var fileStream = new FileStream(_appEnvironment.WebRootPath + avatarPath, FileMode.Create))
                    {
                        await uploadedFile.CopyToAsync(fileStream);
                    }
                }
                else
                {
                    //todo universal method Delete for any extension//2
                    var fileName = Directory.GetParent(Directory.GetCurrentDirectory()) + "\\frontend\\Files\\avatar.jpg";
                    var fileName2 = Directory.GetParent(Directory.GetCurrentDirectory()) + "\\frontend\\Files\\avatar.png";
                    if (System.IO.File.Exists(fileName))
                    {
                        System.IO.File.Delete(fileName);
                    }
                    if (System.IO.File.Exists(fileName2))
                    {
                        System.IO.File.Delete(fileName2);
                    }
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
