using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace projectDydaTomasz.Core.Models
{
    public class Apartment
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string apartmentId { get; set; }
        public string surface { get; set; }
        public string cost { get; set; }
        public string street { get; set; }
        public string user { get; set; }        
    }
}
