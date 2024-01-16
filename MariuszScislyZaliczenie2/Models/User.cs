using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;


namespace MariuszScislyZaliczenie2.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string userId { get; set; }
        public string username { get; set; }
        public string passwordHash { get; set; }
        public string email { get; set; }

        public override string ToString()
        {
            return $"UserID: {userId}\nUsername: {username}\nPassword: {passwordHash}\nEmail: {email}";
        }
    }    
}
