using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Sepet.Core.Models;

namespace Sepet.MongoDb.Models
{
    public class MusteriSepetiEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        
        public int MusteriId { get; set; }

        public List<SepetItem> Items { get; set; } = new List<SepetItem>();
    }
}