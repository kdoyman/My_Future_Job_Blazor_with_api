using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace BlazorApp.Models
{
    public class Customer
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [Required(ErrorMessage = "CompanyName is required.")]
        public string CompanyName { get; set; }
        [Required(ErrorMessage = "ContactName is required.")]
        public string ContactName { get; set; }
        public string Address { get; set; }
        [StringLength(50,ErrorMessage ="You exceeded the length of City name")]
        public string City { get; set; }
        public string Region { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        [Required(ErrorMessage = "Phone is required.")]
        public string Phone { get; set; }
    }
}
