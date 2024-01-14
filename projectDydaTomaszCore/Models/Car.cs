using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using projectDydaTomaszCore.Models;

namespace projectDydaTomasz.Core.Models
{
    public class Car
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string carId { get; set; }
        public string carBrand { get; set; }
        public string carModel { get; set; }
        public string carProductionYear{ get; set; }
        public string engineCapacity { get; set; }
        public string user{ get; set; }
    }
}
