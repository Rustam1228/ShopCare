using System.ComponentModel.DataAnnotations;

namespace backend.Core.Models
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }
        [MinLength(5)]
        [MaxLength(20)]
        public required string Login { get; set; }
        [MinLength(5)]
        [MaxLength(20)]
        public required string Password { get; set; }       
        public List<Care> Cares { get; set; } = [];

    }
}
