using System.ComponentModel.DataAnnotations;

namespace backend.Core.Models
{
    public class Care
    {        
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public required string Phone {  get; set; }
        public required string Brand { get; set; }
        public uint Mileage { get; set; }
        public decimal Price { get; set; }
        public uint YearRelease { get; set; }
        public DateTime CreateAt { get; set; } = DateTime.UtcNow;
        public List<ImageModel> Images { get; set; } = [];
        public required Guid UserID { get; set; }   
        public User User { get; set; }

    }
}
