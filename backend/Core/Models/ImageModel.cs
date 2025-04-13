using System.ComponentModel.DataAnnotations;

namespace backend.Core.Models
{
    public class ImageModel
    {
        public Guid Id { get; set; }        
        public required string ImageName { get; set; }
        public Guid ImageId { get; set; } 
        public Care Care { get; set; }
    }
}
