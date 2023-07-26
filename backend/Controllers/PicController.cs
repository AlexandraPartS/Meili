using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.Models;
using backend.Interfaces;
//using backend.Services;
using backend.Infrastructure;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.ComponentModel.DataAnnotations;
using System.Web;
using NuGet.Protocol;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Produces("application/json")]
    public class PicController : Controller
    {
        IUserService userService;
        UserContext _context;
        IWebHostEnvironment _appEnvironment;
        //public PicController(IUserService serv, IWebHostEnvironment appEnvironment)
        public PicController(UserContext context, IWebHostEnvironment appEnvironment)
        {
            _context = context;
            _appEnvironment = appEnvironment;
        }

        public IActionResult Index()
        {
            //return View(_context.Files.ToList());
            try
            {
                if(_context.Files == null) { return NotFound(); }
                else {
                    //var response = Response;
                    string fullPath = _context.Files.FirstOrDefault().Path + "\\" + _context.Files.FirstOrDefault().Name;
                    Response.SendFileAsync(fullPath);
                    return new EmptyResult(); 
                }
            }
            catch(Exception e)
            {
                Console.WriteLine($"    ERROR : {e.Message}");
                return NotFound(e.Message);
            }
        }

        // POST: api/Pic
        [HttpPost]
        //public async Task<IActionResult> AddFile(IFormFile uploadedFile)
        //public async Task<IActionResult> AddFile(IFormCollection uploadedFile)
        //public async Task<IActionResult> AddFile(string firstName, string LastName, IFormFile avatar)
        public async Task<IActionResult> AddFile(IFormCollection collection)
        {

        
            try
            {
                if (ModelState.IsValid)
                {
                    Person person = new Person { NickName = collection["firstName"], LastName = collection["LastName"] };
                    Console.WriteLine(person.ToJson());





                    IFormFile uploadedFile = collection.Files[0];//avatar
                    string path = "/Files/" + uploadedFile.FileName;//What directory?
                    using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                    {
                        await uploadedFile.CopyToAsync(fileStream);
                    }

                    person.avatar = new FileModel { Name = uploadedFile.FileName, Path = path };
                    _context.Files.Add(person.avatar);
                    _context.SaveChanges();

                    return Ok(person.ToJson());
                }
                
            }
            catch(Exception e)
            {
                Console.WriteLine($"    E R R O R:  {e.Message}");
                return NotFound();
            }
            return null;

        }


        //protected override void Dispose(bool disposing)
        //{
        //    userService.Dispose();
        //    base.Dispose(disposing);
        //}
    }
}
public class Person
{
    public string NickName { get; set; }
    public string LastName { get; set; }
    //public IFormFile avatar { get; set; }
    public FileModel avatar { get; set; }
}
