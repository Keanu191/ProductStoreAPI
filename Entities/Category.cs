/*
 * 30074191 / Keanu Farro
 * Code used from youtube tutorial: https://www.youtube.com/watch?v=Gxf7zBl5Z64
 */

// import these two to use BsonId and BsonElement
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WebApplicationDemoS4.Entities
{
    public class Category
    {
        [BsonId]
        [BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
        public int Id { get; set; }

        [BsonElement("name"), BsonRepresentation(BsonType.String)]
        public string Name { get; set; }
    }
}
