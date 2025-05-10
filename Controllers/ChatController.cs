using Chat_Project.Data;
using Chat_Project.DTOs.ChatDTO;
using Chat_Project.DTOs.ChatParticipantsDTO;
using Chat_Project.DTOs.MessageDTO;
using Chat_Project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Chat_Project.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
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
                    var chat = await _db.Chats.Include(cp => cp.ChatParticipants).FirstOrDefaultAsync(x => x.NameChat == namechat);
                    bool isNewChat = chat == null;

                    if (isNewChat)
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

                    if (isNewChat && chataddDTO.ChatParticipants?.Any() == true)
                    {
                        var newParticipants = chataddDTO.ChatParticipants
                            .Select(cp => new ChatParticipant()
                            {
                                UserId = cp.UserId,
                                ChatId = chat.Id
                            }).ToList();

                        _db.ChatParticipants.AddRange(newParticipants);
                    }

                    var messages = chataddDTO.Messages.Select(m => new Message()
                    {
                        UserId = m.UserId,
                        ChatId = chat.Id,
                        MessageText = m.MessageText,
                        MessageDate = DateTime.Now,
                        IsRead = false,
                        IsDeleted = false,
                    }).ToList();

                    _db.Messages.AddRange(messages);
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
        [HttpGet]
        [Route("getPrivateChat1/{nameChat}")]
        public async Task<ActionResult> Get([FromRoute] string nameChat)
        {
            if (String.IsNullOrEmpty(nameChat))
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Asegurate de enviar un nombre de chat adecuado"
                });
            }
            var chat = _db.Chats.Where(c => c.NameChat == nameChat).FirstOrDefault();
            if (chat == null)
            {
                return NotFound(new
                {
                    success = false,
                    message = "No se encontró un chat con ese nombre"
                });
            }
            return Ok(new
            {
                success = true,
                message = "Chat obtenido exitosamente",
                chat
            });
        }

        [HttpGet]
        [Route("getPrivateChat2/{nameChat}/{filasObtener}")]
        public async Task<ActionResult> GetMessagesLimit([FromRoute] string nameChat, [FromRoute] int filasObtener)
        {
            int filasObtenerConsulta = filasObtener;
            if (String.IsNullOrEmpty(nameChat))
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Asegurate de enviar un nombre de chat adecuado"
                });
            }
            var chat = _db.Messages
                .Include(c => c.Chat)
                .Include(c => c.Chat.ChatParticipants)
                .Where(c => c.Chat.NameChat == nameChat)

                .OrderBy(c => c.Id)
                .Skip(0)
                .Take(filasObtenerConsulta)
                .ToList();


            if (chat == null || chat.Count == 0)
            {
                return Ok(new
                {
                    success = false,
                    message = "No se encontró un chat con ese nombre o no tiene elementos."
                });
            }

            var chatDto = new ChatGetDTO
            {
                Id = chat.First().Chat.Id,
                NameChat = chat.First().Chat.NameChat,
                Messages = chat.Select(x => new MessageGetDTO
                {
                    Id = x.Id,
                    UserId = x.UserId,
                    MessageText = x.MessageText,
                    UserName = _db.Users?.Where(e => e.UserId == x.UserId).Select(x => x.Username).FirstOrDefault(),
                    ChatId = x.ChatId,
                    MessageDate = x.MessageDate,
                    IsRead = x.IsRead,
                    IsDeleted = x.IsDeleted
                }).ToList(),
                ChatParticipants = chat.First().Chat.ChatParticipants.Select(cp => new ChatParticipantsGetDTO
                {
                    Id = cp.Id,
                    UserId = cp.UserId,
                    ChatId = cp.ChatId
                }).ToList()
            };

            //var chat = _db.Chats.FromSqlInterpolated<Chat>
            //    (
            //    $"SELECT * FROM Messages m INNER JOIN Chats ch ON m.ChatId = ch.Id
            //     INNER JOIN 
            //    WHERE ch.NameChat = {nameChat} ORDER BY m.Id ASC OFFSET 0 ROWS FETCH NEXT {filasObtenerConsulta} ROWS ONLY"
            //    );

            if (chat.Count() < filasObtenerConsulta)
            {
                return Ok(new
                {
                    success = true,
                    message = "Ya no hay más chats para ver",
                    chatDto
                });
            }


            return Ok(new
            {
                success = true,
                message = "Chat obtenido exitosamente",
                chatDto
            });
        }

        //[HttpPost]
        //[Route("createChat")]
        //public async Task<ActionResult> CreateChat([FromBody] ChatAddDTO chataddDTO)
        //{
        //    if (chataddDTO == null)
        //    {
        //        return BadRequest(new { success = false, message = "El chat debe tener contenido" });
        //    }

        //    using (var transaction = await _db.Database.BeginTransactionAsync())
        //    {
        //        try
        //        {
        //            var namechat = chataddDTO.NameChat;
        //            var existChat = await _db.Chats.FirstOrDefaultAsync(x => x.NameChat == namechat);

        //            Chat chat;

        //            if (existChat == null)
        //            {
        //                chat = new Chat()
        //                {
        //                    NameChat = chataddDTO.NameChat,
        //                    Messages = new List<Message>(),
        //                    ChatParticipants = new List<ChatParticipant>(),
        //                };

        //                _db.Chats.Add(chat);
        //                await _db.SaveChangesAsync();
        //            }
        //            else
        //            {
        //                chat = existChat;
        //            }

        //            if (chataddDTO.Messages?.Any() == true)
        //            {
        //                var messages = chataddDTO.Messages.Select(m => new Message()
        //                {
        //                    UserId = m.UserId,
        //                    ChatId = chat.Id,
        //                    MessageText = m.MessageText,
        //                    MessageDate = DateTime.UtcNow,
        //                    IsRead = false,
        //                    IsDeleted = false,
        //                }).ToList();

        //                _db.Messages.AddRange(messages);
        //            }

        //            if (chataddDTO.ChatParticipants?.Any() == true)
        //            {
        //                var existingUserIds = _db.ChatParticipants
        //                    .Where(cp => cp.ChatId == chat.Id)
        //                    .Select(cp => cp.UserId)
        //                    .ToHashSet();

        //                var newParticipants = chataddDTO.ChatParticipants
        //                    .Where(cp => !existingUserIds.Contains(cp.UserId))
        //                    .Select(cp => new ChatParticipant()
        //                    {
        //                        UserId = cp.UserId,
        //                        ChatId = chat.Id
        //                    }).ToList();

        //                _db.ChatParticipants.AddRange(newParticipants);
        //            }

        //            await _db.SaveChangesAsync();
        //            await transaction.CommitAsync();

        //            return Ok(new { success = true, message = "El chat ha sido guardado correctamente." });
        //        }
        //        catch (Exception ex)
        //        {
        //            await transaction.RollbackAsync();
        //            return StatusCode(500, new { success = false, message = ex.Message });
        //        }
        //    }
        //}


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
