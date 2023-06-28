using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.Models;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class UserController : ControllerBase
    {
        private readonly UserContext _context;

        public UserController(UserContext context)
        {
            _context = context;
        }

        // GET: api/User
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<UserDto>>> Get()
        {
            if (_context.Users == null)
            {
                return NotFound();
            }
            return await _context.Users.Where(x => x.IsDeleted == false).Select(x => ItemToDTO(x)).ToListAsync().ConfigureAwait(false);
        }

        // GET: api/User/5
        [HttpGet("{userId:long}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<UserDto>> GetUser(long userId)
        {
            if (_context.Users == null)
            {
                return NotFound();
            }
            var user = await _context.Users.FindAsync(userId).ConfigureAwait(false);

            if (user == null || user.IsDeleted) 
            {
                return NotFound();
            }

            return ItemToDTO(user);
        }

        // PUT: api/User/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutUserDto(long id, UserDto userDto)
        {

            if (ModelState.IsValid)
            {
                if (id != userDto.Id)
                {
                    return BadRequest();
                }

                var user = await _context.Users.FindAsync(id).ConfigureAwait(false);

                if (user == null)
                {
                    return NotFound();
                }
                
                _context.Users.Attach(user);
                SetUserDaoModifiedPropValue(user, userDto);

                try
                {
                    await _context.SaveChangesAsync().ConfigureAwait(false);
                }
                catch (DbUpdateConcurrencyException) when (!IsUserExists(id))
                {
                    return NotFound();
                }

                return NoContent();
            }

            return BadRequest(ModelState);

        }

        // POST: api/User
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserDto>> Create([FromBody] UserDto userDto)
        {
            if (_context.Users == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (_context.Users.Any(i => i.PhoneNumber == userDto.PhoneNumber))
                {
                    ModelState.AddModelError(nameof(userDto.PhoneNumber), "The Phone number is already in use."); //To change
                    return BadRequest(ModelState);
                }

                var user = new UserDao(userDto);
                _context.Users.Add(user);
                await _context.SaveChangesAsync().ConfigureAwait(false);

                return CreatedAtAction(
                    nameof(Create),
                    new { id = user.Id }, 
                    ItemToDTO(user));
            }
            else
            return BadRequest(ModelState);
        }

        // DELETE: api/User/5
        [HttpDelete("{userId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(long userId)
        {
            if (_context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(userId).ConfigureAwait(false);
            if (user == null)
            {
                return NotFound();
            }
            
            user.IsDeleted = true;   
            await _context.SaveChangesAsync().ConfigureAwait(false);

            return NoContent();
        }

        private bool IsUserExists(long id)
        {
            return (_context.Users?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private static UserDto ItemToDTO(UserDao user) => new UserDto(user);
            
        private static void SetUserDaoModifiedPropValue(UserDao user, UserDto userDto)
        {
            foreach(var propDao in typeof(UserDao).GetProperties())
            {
                foreach (var prop in typeof(UserDto).GetProperties())
                {
                    if (propDao.Name == prop.Name)
                    {
                        if (prop.GetValue(userDto) != propDao.GetValue(user))
                        {
                            propDao.SetValue(user, prop.GetValue(userDto));
                        }
                    }
                }
            }
        }
    }
}
