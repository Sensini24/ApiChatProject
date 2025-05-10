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
        //public async Task<IActionResult> UploadFile([FromForm] string nameChat,  List<IFormFile> files)
        public async Task<IActionResult> UploadFile([FromForm] string nameChat, List<IFormFile> files)
        {
            try
            {

                string pathFiles = $"/home/brandon/Documentos/ARCHIVOS_DE_RPOYECTOS/CHATPROJECTDATA/{nameChat}";

                if (!Directory.Exists(pathFiles))
                {
                    Directory.CreateDirectory(pathFiles);
                }



                var listaNombres = new Dictionary<string, string>();
                var idChat = await _db.Chats.Where(n => n.NameChat == nameChat).Select(i => i.Id).FirstOrDefaultAsync();
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                foreach (var file in files)
                {
                    string nameFile = file.FileName;
                    decimal sizeInMB = Math.Round((decimal)file.Length / (1024 * 1024), 2);
                    string mimeType = file.ContentType;
                    string fileExtension = Path.GetExtension(file.FileName);
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
                        FileType = mimeType,
                        FileExtension = fileExtension,
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

            using (FileStream fsWrite = new FileStream(path, FileMode.Create))

            using (Stream fsRead = file.OpenReadStream())
            {
                byte[] buffer = new byte[4096];
                int bytes;

                while ((bytes = fsRead.Read(buffer, 0, buffer.Length)) > 0)
                {
                    fsWrite.Write(buffer, 0, bytes);
                }

            }

        }

        [HttpGet]
        [Route("dowloadFile/{idFile}")]
        public async Task<IActionResult> DowloadFile(int idFile)
        {
            if (idFile == null || idFile == 0)
            {
                return NotFound(new
                {
                    success = false,
                    message = "Id inválido"
                });
            }

            var fileFounded = await _db.FilePrivateChats.Where(f => f.Id == idFile).FirstOrDefaultAsync();
            if (fileFounded == null)
            {
                return NotFound(new
                {
                    success = false,
                    message = "Archivo no encontrado"
                });
            }
            string fileName = fileFounded.FileName;
            string fileMime = fileFounded.FileType;
            string filePath = fileFounded.FilePath;

            using (MemoryStream ms = new MemoryStream())
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    fs.CopyTo(ms);
                }

                byte[] fileBytes = ms.ToArray();
                ms.Position = 0;

                return File(fileBytes, fileMime, fileName);
            }
        }

        [HttpGet]
        [Route("getFilesChatPrivate/{nameChat}")]
        public async Task<IActionResult> GetFiles(string nameChat)
        {
            if (String.IsNullOrEmpty(nameChat))
            {
                return NotFound(new
                {
                    message = "Asegúrate de enviar un nombre válido",
                    success = false
                });
            }

            int? idChat = await _db.Chats.Where(n => n.NameChat == nameChat).Select(i => (int?)i.Id).FirstOrDefaultAsync();

            if (idChat == null)
            {
                return NotFound(new
                {
                    message = "No se encontró el chat buscado",
                    success = false
                });
            }

            var filesFounded = _db.FilePrivateChats.FromSqlInterpolated($"SELECT fp.* FROM FilePrivateChats fp INNER JOIN Chats ch on fp.ChatId = ch.Id WHERE ch.Id = {idChat}").ToList();

            var fileGetDto = filesFounded.Select(x => new FilePrivateChatGetDTO
            {
                Id = x.Id,
                UserId = x.UserId,
                ChatId = x.ChatId,
                FileName = x.FileName,
                FileSize = x.FileSize,
                UploadDate = x.UploadDate,
                FileType = x.FileType,
                FileExtension = x.FileExtension

            }).ToList();
            return Ok(new
            {
                message = "Archivos encontrados",
                success = true,
                files = fileGetDto
            });
        }
    }
}
