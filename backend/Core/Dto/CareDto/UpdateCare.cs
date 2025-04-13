namespace backend.Core.Dto.CareDto
{
    public class UpdateCare
    {
        public string Title { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public required string Phone { get; set; }

    }
}
