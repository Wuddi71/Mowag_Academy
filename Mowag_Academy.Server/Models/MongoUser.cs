using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Mowag_Academy.Server.Models
{
    public class MongoUser
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("username")]
        public string Username { get; set; }

        [BsonElement("passwordHash")]
        public string PasswordHash { get; set; }

        [BsonElement("role")]
        public string Role { get; set; }

        [BsonElement("firstname")]
        public string Firstname { get; set; }

        [BsonElement("lastname")]
        public string Lastname { get; set; }

        [BsonElement("coursedate")]
        public DateTime CourseDate { get; set; }

        [BsonElement("possiblestartingyear")]
        public int PossibleStartingYear { get; set; }
    }
}
