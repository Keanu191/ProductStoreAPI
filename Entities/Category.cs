using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WebApplicationDemoS4.Entities
{
    public class Category
    {
        [BsonId]
        [BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
        // setting ID a string to prevent exception objectid is not a valid representation for an int32serializer
        public string? Id { get; set; }

        [BsonElement("name"), BsonRepresentation(BsonType.String)]
        public string? Name { get; set; }

        [BsonElement("products"), BsonRepresentation(BsonType.String)]
        public virtual List<Product>? Products { get; set; }
    }
}
