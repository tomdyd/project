using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MariuszScislyZaliczenie2.Models;

namespace MariuszScislyZaliczenie2.Models
{
    public class Song
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string songId { get; set; }
        public string songTitle { get; set; }
        public string songAuthor { get; set; }
        public string songGenre{ get; set; }
        public string songAlbum { get; set; }
        public string user{ get; set; }
    }
}
