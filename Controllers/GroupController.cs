using System.Security.Claims;
using Chat_Project.Data;
using Chat_Project.DTOs.GroupDTO;
using Chat_Project.DTOs.GroupParticipantsDTO;
using Chat_Project.DTOs.MessageGroupDTO;
using Chat_Project.DTOs.UserDTO;
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
    public class GroupController : Controller
    {
        private readonly DataContext _db;
        public GroupController(DataContext db)
        {
            _db = db;
        }


        [HttpGet]
        [Route("getGroupsByUser")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                var grupoCompleto = _db.Groups
                .Include(c => c.GroupParticipants)
                .Where(gp => gp.GroupParticipants.Any(x => x.UserId == userId)).ToList();


                if (grupoCompleto == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "Guardado con éxito pero no se encontró un resultado que devolver en base de datos."
                    });
                }

                var gruposcompletosdto = grupoCompleto.Select(x => new GroupGetSimpleDTO
                {
                    GroupId = x.GroupId,
                    NameGroup = x.NameGroup,
                    DateCreated = x.DateCreated,
                    IsDeleted = x.IsDeleted,
                    GroupCategory = x.GroupCategory,
                    GroupParticipants = x.GroupParticipants.Select(x => new GroupParticipantsGetDTO
                    {
                        GroupParticipantsId = x.GroupParticipantsId,
                        UserId = x.UserId,
                        GroupId = x.GroupId,
                        DateJoined = x.DateJoined,
                        Rol = x.Rol,
                        isFavorite = x.isFavorite

                    }).ToList()
                });
                return Ok(new
                {
                    success = true,
                    message = "Grupos obtenidos correctamente",
                    groups = gruposcompletosdto
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Ocurrió un error en el servidor.", error = ex.Message });
            }
        }

        [HttpGet]
        [Route("getGroups")]
        public async Task<IActionResult> GetGroups()
        {
            try
            {
                var gruposCompletos = _db.Groups.ToList();

                if (gruposCompletos == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "Guardado con éxito pero no se encontró un resultado que devolver en base de datos."
                    });
                }

                var gruposcompletosdto = gruposCompletos.Select(x => new GroupSearcherdGetDTO
                {
                    GroupId = x.GroupId,
                    NameGroup = x.NameGroup,
                    DateCreated = x.DateCreated,
                    IsDeleted = x.IsDeleted,
                    GroupCategory = x.GroupCategory
                }).ToList();

                return Ok(new
                {
                    success = true,
                    message = "Grupos obtenidos correctamente",
                    groups = gruposcompletosdto
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Ocurrió un error en el servidor.", error = ex.Message });
            }
        }


        [HttpGet]
        [Route("getMessagesByGroup/{groupId}")]
        public async Task<IActionResult> GetMessagesByGroup(int groupId)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                //var messagesGroup = await _db.MessagesGroups.Where(n => n.GroupId == groupId && n.UserId == userId).ToListAsync();
                var messagesGroup = await _db.MessagesGroups.Include(m => m.User).Where(n => n.GroupId == groupId).ToListAsync();

                if (messagesGroup == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "El grupo no existe."
                    });
                }

                if (messagesGroup.Count() == 0)
                {
                    return Ok(new
                    {
                        success = false,
                        message = "No se encontraron mensajes de este grupo."
                    });
                }

                var messagesCompletos = messagesGroup.Select(x => new MessageGroupGetDTO
                {
                    MessagesGroupId = x.MessagesGroupId,
                    UserId = x.UserId,
                    Username = x.User.Username,
                    GroupId = x.GroupId,
                    MessageText = x.MessageText,
                    MessageDate = x.MessageDate
                }).ToList();

                return Ok(new
                {
                    success = true,
                    message = "Mensajes obtenidos correctamente",
                    messages = messagesCompletos
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Ocurrió un error en el servidor.", error = ex.Message });
            }
        }

        // POST: GroupController/Create
        [HttpPost]
        [Route("createGroup")]
        public async Task<IActionResult> Create([FromBody] GroupAddDTO groupaddto)
        {
            try
            {
                if (groupaddto == null)
                {
                    return Ok(new
                    {
                        success = false,
                        message = "Envíe un grupo válido con contenido"
                    });
                }

                var grupo = new Group
                {
                    NameGroup = groupaddto.NameGroup,
                    DateCreated = DateTime.Now,
                    IsDeleted = false,
                    GroupCategory = groupaddto.GroupCategory,
                    GroupParticipants = new List<GroupParticipants>(),
                    GroupMessages = new List<MessagesGroup>()
                };

                await _db.Groups.AddAsync(grupo);
                await _db.SaveChangesAsync();

                var groupParticipants = groupaddto.GroupParticipants.Select(x => new GroupParticipants
                {
                    UserId = x.UserId,
                    GroupId = grupo.GroupId,
                    DateJoined = DateTime.Now,
                    InvitationStatus = "Creator",
                    Rol = "Creator",
                    isFavorite = false
                }).ToList();
                await _db.AddRangeAsync(groupParticipants);
                await _db.SaveChangesAsync();

                var grupoCompleto = await _db.Groups.Where(g => g.GroupId == grupo.GroupId).Include(gp => gp.GroupParticipants).FirstOrDefaultAsync();
                if (grupoCompleto == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "Guardado con éxito pero no se encontró un resultado que devolver en base de datos."
                    });
                }

                var grupocompletodto = new GroupGetDTO()
                {
                    GroupId = grupoCompleto.GroupId,
                    NameGroup = grupoCompleto.NameGroup,
                    DateCreated = grupoCompleto.DateCreated,
                    IsDeleted = grupoCompleto.IsDeleted,
                    GroupCategory = grupoCompleto.GroupCategory,
                    GroupParticipants = grupoCompleto.GroupParticipants.Select(x => new GroupParticipantsGetDTO
                    {
                        GroupParticipantsId = x.GroupParticipantsId,
                        UserId = x.UserId,
                        GroupId = x.GroupId,
                        DateJoined = x.DateJoined,
                        Rol = x.Rol,
                        isFavorite = x.isFavorite

                    }).ToList(),

                    GroupMessages = grupoCompleto.GroupMessages.Select(x => new MessageGroupGetDTO
                    {
                        MessagesGroupId = x.MessagesGroupId,
                        UserId = x.UserId,
                        GroupId = x.GroupId,
                        MessageText = x.MessageText,
                        MessageDate = x.MessageDate

                    }).ToList()
                };
                return Ok(new
                {
                    success = true,
                    message = "Grupo creado correctamente",
                    group = grupocompletodto
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Ocurrió un error en el servidor.", error = ex.Message });
            }
        }

        [HttpPost]
        [Route("saveMessageGroup")]
        public async Task<IActionResult> SaveMessage([FromBody] MessageGroupAddDTO messageGroupBody)
        {
            try
            {
                if (messageGroupBody == null)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Envíe un grupo válido con contenido"
                    });
                }

                var newMessage = new MessagesGroup
                {
                    UserId = messageGroupBody.UserId,
                    GroupId = messageGroupBody.GroupId,
                    MessageText = messageGroupBody.MessageText,
                    MessageDate = DateTime.Now,
                    IsRead = false,
                    IsDeleted = false,
                };

                await _db.MessagesGroups.AddAsync(newMessage);
                await _db.SaveChangesAsync();

                var messageGetDto = new MessageGroupGetDTO()
                {
                    MessagesGroupId = newMessage.MessagesGroupId,
                    UserId = newMessage.UserId,
                    GroupId = newMessage.GroupId,
                    MessageText = newMessage.MessageText,
                    MessageDate = newMessage.MessageDate

                };
                return Ok(new
                {
                    success = true,
                    message = "El mensaje fue guardado correctamente",
                    groupMessage = messageGetDto
                });

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Ocurrió un error en el servidor.", error = ex.Message });
            }
        }

        [HttpGet]
        [Route("searchGroup/{initials}")]
        public async Task<IActionResult> FindGroupByInitials(string initials)
        {
            try
            {
                var groups = await _db.Groups.FromSqlInterpolated($"SELECT * FROM Groups where NameGroup LIKE {initials + "%"}").ToListAsync();
                //var userFound = await _context.Users.Where(x => x.Username.Contains(initials)).ToListAsync();
                var groupDto = groups.Select(x => new GroupSearcherdGetDTO
                {
                    GroupId = x.GroupId,
                    NameGroup = x.NameGroup,
                    DateCreated = x.DateCreated,
                    IsDeleted = x.IsDeleted,
                    GroupCategory = x.GroupCategory
                });


                return Ok(new
                {
                    success = true,
                    message = "Grupos por iniciales obtenidas.",
                    groups = groupDto,
                });
            }
            catch (Exception e)
            {
                return BadRequest(new
                {
                    success = false,
                    e
                });
            }
        }


        [HttpPost]
        [Route("joinGroup")]
        public async Task<IActionResult> JoinGroup([FromBody] GroupParticipantsJoinAddDTO gpdto)
        {
            try
            {
                if (gpdto == null)
                {
                    return NotFound();
                }
                //var userFound = await _context.Users.Where(x => x.Username.Contains(initials)).ToListAsync();
                var newGroupParticipants = new GroupParticipants
                {
                    UserId = gpdto.UserId,
                    GroupId = gpdto.GroupId,
                    DateJoined = DateTime.Now,
                    InvitationStatus = gpdto.InvitationStatus,
                    Rol = "Member",
                    isFavorite = false
                };

                await _db.GroupParticipants.AddAsync(newGroupParticipants);
                await _db.SaveChangesAsync();


                return Ok(new
                {
                    success = true,
                    message = "El usuario se unió correctamente al grupo.",
                    groupParticipant = new GroupParticipantsGetDTO()
                    {
                        GroupParticipantsId = newGroupParticipants.GroupParticipantsId,
                        UserId = newGroupParticipants.UserId,
                        GroupId = newGroupParticipants.GroupId,
                        DateJoined = newGroupParticipants.DateJoined,
                        InvitationStatus = newGroupParticipants.InvitationStatus,
                        Rol = newGroupParticipants.Rol,
                        isFavorite = newGroupParticipants.isFavorite
                    },
                });
            }
            catch (Exception e)
            {
                return BadRequest(new
                {
                    success = false,
                    e
                });
            }
        }
    }
}
