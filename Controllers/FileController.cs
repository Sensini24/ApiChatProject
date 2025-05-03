using Chat_Project.Data;
using Chat_Project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        public ActionResult Index(List<IFormFile> files)
        {
            try
            {
                var listaNombres = new List<string>();
                foreach (var file in files)
                {
                    listaNombres.Add(file.FileName);
                    //var fileSave = new FilePrivateChat()
                    //{
                    //    UserId = 
                    //} 
                    //_db.FilePrivateChats
                }
                return Ok(new
                {
                    message = "Archivo recibido",
                    FileNames = listaNombres,

                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        // GET: FileController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: FileController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: FileController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: FileController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: FileController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: FileController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: FileController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
