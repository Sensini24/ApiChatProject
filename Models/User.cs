using System.Text.Json.Serialization;

namespace Chat_Project.Models;

public enum Gender
{
    Masculino,
    Femenino,
    Otro
}

public class User
{
    public int UserId { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }


    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Gender Gender { get; set; }

    // Propiedad de navegación para mensajes enviados
    public ICollection<Message> Messages { get; set; }
    // Propiedad de navegación para chats en los que participa
    public ICollection<ChatParticipant> ChatParticipants { get; set; }
    public ICollection<Contact> Contacts { get; set; }

    //A partir de aqui para crear mensajes de grupos y listar los participantes de los grupos
    public ICollection<GroupParticipants> GroupParticipants { get; set; }
    public ICollection<MessagesGroup> GroupMessages { get; set; }
}