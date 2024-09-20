using CVRecognizingService.Domain.Abstracts;

namespace CVRecognizingService.Domain.Entities;

public class User 
    : Entity, IEntity
{
    public User(string name, string email, string role)
    {
        Name = name;
        Email = email;
        Role = role;
    }

    public User() {}
    public Guid UserId { get; private set; } = Guid.NewGuid();
    public string Name { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string Role { get; private set; } = string.Empty;
}
