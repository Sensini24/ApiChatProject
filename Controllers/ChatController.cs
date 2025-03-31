using Chat_Project.Data;
using Chat_Project.DTOs.ChatDTO;
using Chat_Project.DTOs.MessageDTO;
using Chat_Project.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Chat_Project.Controllers
{
    public class ChatController : Controller
    {
        private readonly DataContext _db;
        public ChatController(DataContext db)
        {
            _db = db;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("createChat")]
        public async Task<ActionResult> CreateChat([FromBody] ChatAddDTO chataddDTO)
        {
            if (chataddDTO == null)
            {
                return BadRequest(new { success = false, message = "El chat debe tener contenido" });
            }

            using (var transaction = await _db.Database.BeginTransactionAsync())
            {
                try
                {
                    var namechat = chataddDTO.NameChat;
                    var existChat = await _db.Chats.FirstOrDefaultAsync(x => x.NameChat == namechat);
                    Chat chat;

                    if (existChat == null)
                    {
                        chat = new Chat()
                        {
                            NameChat = chataddDTO.NameChat,
                            Messages = new List<Message>(),
                            ChatParticipants = new List<ChatParticipant>(),
                        };

                        _db.Chats.Add(chat);
                        await _db.SaveChangesAsync();
                    }
                    else
                    {
                        chat = existChat;
                    }

                    if (chataddDTO.Messages?.Any() == true)
                    {
                        var messages = chataddDTO.Messages.Select(m => new Message()
                        {
                            UserId = m.UserId,
                            ChatId = chat.Id,
                            MessageText = m.MessageText,
                            MessageDate = DateTime.UtcNow,
                            IsRead = false,
                            IsDeleted = false,
                        }).ToList();

                        _db.Messages.AddRange(messages);
                    }

                    if (chataddDTO.ChatParticipants?.Any() == true)
                    {
                        var existingUserIds = _db.ChatParticipants
                            .Where(cp => cp.ChatId == chat.Id)
                            .Select(cp => cp.UserId)
                            .ToHashSet();

                        var newParticipants = chataddDTO.ChatParticipants
                            .Where(cp => !existingUserIds.Contains(cp.UserId))
                            .Select(cp => new ChatParticipant()
                            {
                                UserId = cp.UserId,
                                ChatId = chat.Id
                            }).ToList();

                        _db.ChatParticipants.AddRange(newParticipants);
                    }

                    await _db.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return Ok(new { success = true, message = "El chat ha sido guardado correctamente." });
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return StatusCode(500, new { success = false, message = ex.Message });
                }
            }
        }


        // GET: ChatController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ChatController/Create
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

        // GET: ChatController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ChatController/Edit/5
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

        // GET: ChatController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ChatController/Delete/5
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
