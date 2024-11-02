/*
 * 30074191 / Keanu Farro
 * Code used from youtube tutorial: https://www.youtube.com/watch?v=Gxf7zBl5Z64
 */

// Import these two to use BsonId and BsonElement
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WebApplicationDemoS4.Entities
{
    public class Products
    {
        [BsonId]
        [BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
        public int Id { get; set; }

        [BsonElement("categoryId"), BsonRepresentation(BsonType.Int32)]
        public int CategoryId { get; set; }

        [BsonElement("name"), BsonRepresentation(BsonType.String)]
        public string Name { get; set; }

        [BsonElement("storeLocation"), BsonRepresentation(BsonType.String)]
        public string StoreLocation { get; set; }

        [BsonElement("postCode"), BsonRepresentation(BsonType.Int32)]
        public int PostCode { get; set; }

        [BsonElement("price"), BsonRepresentation(BsonType.Decimal128)]
        public decimal Price { get; set; }

        [BsonElement("isAvailable"), BsonRepresentation(BsonType.Boolean)]
        public bool IsAvailable { get; set; }
    }
}
