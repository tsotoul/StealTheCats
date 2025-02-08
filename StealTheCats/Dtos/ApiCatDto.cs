namespace StealTheCatsApi.Dtos
{
    public class ApiCatDto
    {
        public string Id { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string Url { get; set; }
        public byte[] Image { get; set; }
        public IEnumerable<BreedDto> Breeds { get; set; }
    }
}
