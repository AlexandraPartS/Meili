using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.Models;
using backend.Interfaces;
using NuGet.Protocol;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PicController : Controller
    {
        IUserService userService;
        UserContext _context;
        IWebHostEnvironment _appEnvironment;
        public PicController(IUserService serv, UserContext context, IWebHostEnvironment appEnvironment)
        {
            _context = context;
            _appEnvironment = appEnvironment;
            userService = serv;
        }

        public IActionResult Index()
        {
            try
            {
                if(_context.Files == null) { return NotFound(); }
                else {

                    string filePath = "/images/";
                    string fullPath0 = AppDomain.CurrentDomain.BaseDirectory + filePath + "/";
                    var p1 = Directory.GetCurrentDirectory();
                    var p2 = Directory.GetParent(p1);
                    string fullPath = p2 + "\\frontend" + _context.Files.FirstOrDefault().Path;
                    Response.SendFileAsync(fullPath);
                    return new EmptyResult(); 
                }
            }
            catch(Exception e)
            {
                return NotFound(e.Message);
            }
        }

        // POST: api/Pic
        [HttpPost]
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
            catch (Exception e)
            {
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
public class Person2
{
    public string id { get; set; }
    public string NickName { get; set; }
    public FileModel avatar { get; set; }
}
public class Person
{
    public string NickName { get; set; }
    public string LastName { get; set; }
    //public IFormFile avatar { get; set; }
    public FileModel avatar { get; set; }
}
