namespace StealTheCatsApi.Dtos
{
    public class ApiCatDto
    {
        public string Id { get; set; } = String.Empty;
        public int Width { get; set; }
        public int Height { get; set; }
        public string Url { get; set; } = String.Empty;
        public byte[] Image { get; set; } = Array.Empty<byte>();
        public IEnumerable<BreedDto> Breeds { get; set; } = Enumerable.Empty<BreedDto>();
    }
}
