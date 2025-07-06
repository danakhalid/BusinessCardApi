using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
namespace BusinessCardApi.Models
{
    public class BusinessCard
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        [NotMapped]
        [JsonIgnore]
        public IFormFile Photo { get; set; }

        public string? PhotoBase64 { get; set; }
        public string Address { get; set; }

    }
}
