﻿using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WebApplicationDemoS4.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]

        public int CategoryId { get; set; }

        [Required]

        public string Name { get; set; } = string.Empty;

        [Required]

        public string StoreLocation { get; set; } = string.Empty;

        public int PostCode { get; set; }
        [Required]

        public decimal Price { get; set; }
        [Required]

        public bool IsAvailable { get; set; }

        [JsonIgnore]

        public virtual Category? Category { get; set; }
    }
}
