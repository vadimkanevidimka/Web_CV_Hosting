using MongoDB.Bson;

namespace CVRecognizingService.Domain.Entities
{
    public class User
    {
        public User(string name, string email)
        {
            Name = name;
            Email = email;
        }

        public ObjectId Id { get; private set; } = ObjectId.GenerateNewId();
        public string Name { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;
    }
}
