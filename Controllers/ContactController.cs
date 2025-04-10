using System.Security.Claims;
using Chat_Project.Data;
using Chat_Project.DTOs.ContactDTO;
using Chat_Project.Models;
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

        [HttpPost]
        [Route("addContact")]
        public async Task<IActionResult> AddContact([FromBody] ContactAddDTO contactdto)
        {
            if (contactdto == null)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Contacto inválido"
                });
            }

            var currentUser = _db.Users.Where(x => x.UserId == contactdto.UserId).FirstOrDefault();

            if(currentUser == null)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "No se pudo obtener los datos del usuario logeado"
                });
            }

            var newContact1 = new Contact
            {
                UserId = contactdto.UserId,
                ContactUserId = contactdto.ContactUserId,
                NickName = contactdto.NickName,
                DateAdded = DateTime.Now,
                IsFavorite = false,
                IsDeleted = false,
                IsBlocked = false
            };

            await _db.Contacts.AddAsync(newContact1);
            var newContact2 = new Contact
            {
                UserId = contactdto.ContactUserId,
                ContactUserId = contactdto.UserId,
                NickName = currentUser.Username,
                DateAdded = DateTime.Now,
                IsFavorite = false,
                IsDeleted = false,
                IsBlocked = false
            };

            await _db.Contacts.AddAsync(newContact2);

            await _db.SaveChangesAsync();
            return Ok(new
            {
                success = true,
                message = "Contacto guardado",
                contacto = newContact1
            });
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
