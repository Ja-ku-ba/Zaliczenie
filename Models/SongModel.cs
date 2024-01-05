using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Zaliczenie.Interfaces;

namespace Zaliczenie.Models
{
    public class SongModel : IArt
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("title")]
        public string Title { get; set; }

        [BsonElement("author")]
        public string Author { get; set; }

        [BsonElement("relased")]
        public DateTime Relased { get; set; }

        [BsonElement("rating")]
        public int Rating { get; set; }
    }
}
