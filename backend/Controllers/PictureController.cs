/// <summary>
/// Purpose (for now): TEST controller for working with files: 
/// 1. uploading file to the DB (method AddFile),
/// 2. getting a file on the client (method Index)
/// Test page: pic.html
/// 
/// File structure (in perspective):
/// ../Files/avatar.*
/// ../Files/AlbomName1/fileName.*
/// etc.
/// </summary>

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.Models;
using backend.Interfaces;
using backend.Infrastructure;
using NuGet.Protocol;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PictureController : Controller
    {
        private readonly IUserService _userService;
        private readonly UserContext _context;
        public PictureController(IUserService serv, UserContext context)
        {
            _context = context;
            _userService = serv;
        }

        //Uploads one image to the client from the directory
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Index()
        {
            try
            {
                if(_context.Files == null) 
                {
                    return NotFound(); 
                }
                else 
                {
                    //TEST: get the first element from the DB
                    //rewrite to generalized
                    if (_context.Files.Any())
                    {
                        string localFilePath = _context.Files.FirstOrDefault().Path;
                        string fileName = _context.Files.FirstOrDefault().Name;

                        string fullFilePath = GlobalVariables.FileStoragePath + localFilePath + fileName;
                        Response.SendFileAsync(fullFilePath);
                    }
                    return new EmptyResult(); 
                }
            }
            catch(Exception e)
            {
                return NotFound(e.Message);
            }
        }

        //TEST: receiving multiply data from <form form-data> of client & uploading the file to the "Files" DB
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

                    //File structure (directory) see top of page
                    string localFilePath = ""/*+ "AlbomName\\"*/ ;
                    string fileName = uploadedFile.FileName;
                    string fullFilePath = GlobalVariables.FileStoragePath + localFilePath + fileName;
                    //string fullFilePath = _appEnvironment.WebRootPath + localFilePath + fileName;
                    Console.WriteLine($"_fullFilePath  :  {fullFilePath}");
                    using (var fileStream = new FileStream(fullFilePath, FileMode.Create))
                    {
                        await uploadedFile.CopyToAsync(fileStream);
                    }

                    person.avatar = new FileModel { Name = uploadedFile.FileName, Path = localFilePath };
                    _context.Files.Add(person.avatar);
                    _context.SaveChanges();

                    return Ok(person.ToJson());
                }
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
            return null;
        }

        protected override void Dispose(bool disposing)
        {
            _userService.Dispose();
            _context.Dispose();
            base.Dispose(disposing);
        }
    }
}