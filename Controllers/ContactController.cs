using System.Security.Claims;
using Chat_Project.Data;
using Chat_Project.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Chat_Project.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ContactController : Controller
    {
        private readonly DataContext _db;

        public ContactController(DataContext db)
        {
            _db = db;
        }

        [HttpGet]
        [Route("getContacts")]
        public ActionResult GetContacts()
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var contacts = _db.Contacts.Where(u=>u.UserId == userId).ToList();

            var contactsDTO = contacts.Select(x => new ContactGetDTO
            {
                ContactId = x.ContactId,
                UserId = x.UserId,
                ContactUserId = x.ContactUserId,
                NickName = x.NickName,
                DateAdded = x.DateAdded,
                IsFavorite = x.IsFavorite,
                IsDeleted = x.IsDeleted,
                IsBlocked = x.IsBlocked
            });

            return Ok(new
            {
                success = true,
                message = "Contactos obtenidos",
                contacts = contactsDTO
            });
        }

        // GET: ContactController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ContactController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ContactController/Create
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

        // GET: ContactController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ContactController/Edit/5
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

        // GET: ContactController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ContactController/Delete/5
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
