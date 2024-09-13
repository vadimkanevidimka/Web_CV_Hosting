using MongoDB.Bson;
using CVRecognizingService.Domain.ValueObjects.BaseVO;
using MongoDB.Bson.Serialization.Attributes;

namespace CVRecognizingService.Domain.ValueObjects.VODerivatives
{
    public class User : ValueObject, IEquatable<User>
    {
        public User(string name, string email)
        {
            Name = name;
            Email = email;
        }
        [BsonId]
        public ObjectId Id { get; set; } = ObjectId.GenerateNewId();
        public string Name { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;

        public bool Equals(User? other)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != GetType())
            {
                return false;
            }

            var other = (User)obj;

            return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Id;
            yield return Name;
            yield return Email;
        }
    }
}
