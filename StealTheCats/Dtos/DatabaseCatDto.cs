namespace StealTheCatsApi.Dtos
{
    public class DatabaseCatDto
    {
        public int Id { get; set; }
        public required string CatId { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public byte[] Image { get; set; }
        public DateTime Created { get; set; }
    }
}
