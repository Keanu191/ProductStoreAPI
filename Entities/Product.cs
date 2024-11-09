using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WebApplicationDemoS4.Entities
{
    public class Product
    {
        [BsonId]
        [BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
        public int Id { get; set; }
        [Required]

        
        [BsonElement("category_id"), BsonRepresentation(BsonType.ObjectId)]
        public int CategoryId { get; set; }

        [Required]

        [BsonElement("name"), BsonRepresentation(BsonType.String)]
        public string Name { get; set; } = string.Empty;

        [Required]

        [BsonElement("store_location"), BsonRepresentation(BsonType.String)]
        public string StoreLocation { get; set; } = string.Empty;

        [BsonElement("post_code"), BsonRepresentation(BsonType.Int32)]
        public int PostCode { get; set; }
        [Required]

        [BsonElement("price"), BsonRepresentation(BsonType.Decimal128)]
        public decimal Price { get; set; }
        [Required]

        [BsonElement("is_available"), BsonRepresentation(BsonType.Boolean)]
        public bool IsAvailable { get; set; }

        [JsonIgnore]

        [BsonElement("category"), BsonRepresentation(BsonType.String)]
        public virtual Category? Category { get; set; }
    }
}
