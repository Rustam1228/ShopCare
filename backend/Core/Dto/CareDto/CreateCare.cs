namespace backend.Core.Dto.CareDto
{
    public class CreateCare
    {
        public string Title { get; set; } = string.Empty;
        public required string Phone { get; set; }
        public required string Brand { get; set; }
        public uint Mileage { get; set; }
        public decimal Price { get; set; }
        public uint YearRelease { get; set; }
        public required Guid UserID { get; set; }
    }
}
