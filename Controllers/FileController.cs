using System.Drawing;
using System.Security.Claims;
using Chat_Project.Data;
using Chat_Project.DTOs.FilePrivateChatDTO;
using Chat_Project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Chat_Project.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class FileController : Controller
    {
        private readonly DataContext _db;
        public FileController(DataContext db)
        {
            _db = db;
        }

        [HttpPost]
        [Route("newFile")]
        //public ActionResult Index(List<IFormFile> files)
        public async Task<IActionResult> UploadFile([FromForm] string nameChat,  List<IFormFile> files)
        {
            try
            {
                string pathFiles = $"D:\\Proyectos c#\\GUARDAR ARCHIVOS\\CHATPROJECTDATA\\{nameChat}";
                if (!Directory.Exists(pathFiles))
                {
                    Directory.CreateDirectory(pathFiles);
                }



                var listaNombres = new Dictionary<string,string>();
                var idChat = await _db.Chats.Where(n => n.NameChat == nameChat).Select(i=>i.Id).FirstOrDefaultAsync();
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                foreach (var file in files)
                {
                    string nameFile = file.FileName;
                    decimal sizeInMB = Math.Round((decimal)file.Length / (1024 * 1024), 2);
                    string FinalPath = Path.Combine(pathFiles, nameFile);
                    listaNombres.Add(nameFile, nameChat);
                    //if(System.IO.File.Exists("da")){
                    //    ystem.IO.File.ReplaceS
                    //}


                    var saveFile = new FilePrivateChat()
                    {
                        UserId = userId,
                        ChatId = idChat,
                        FileName = nameFile,
                        FileSize = sizeInMB,
                        UploadDate = DateTime.Now,
                        FileType = file.GetType().ToString(),
                        FileExtension = "",
                        FilePath = FinalPath
                    };
                    await _db.FilePrivateChats.AddAsync(saveFile);
                    SaveFile(file, FinalPath);
                }

                await _db.SaveChangesAsync();
                return Ok(new
                {
                    message = "Archivos guardados",
                    FileNames = listaNombres,

                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        public void SaveFile(IFormFile file, string path)
        {

            using(FileStream fsWrite = new FileStream(path, FileMode.Create))

            using(Stream fsRead = file.OpenReadStream())
            {
                byte[] buffer = new byte[4096];
                int bytes;
                
                while((bytes = fsRead.Read(buffer, 0, buffer.Length)) > 0)
                {
                    fsWrite.Write(buffer, 0, bytes);
                }

            }

        }
    }
}
