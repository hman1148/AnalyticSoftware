using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AnalyticSoftware.Models
{
    public class User
    {

        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("email")]
        public string Email { get; set; }
        [BsonElement("passwordHash")]
        public string PasswordHash { get; set; }

        [BsonElement("Role")]
        public string Role { get; set; }

        [BsonElement("s3Bucket")]
        public string S3Bucket { get; set; }

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
}
