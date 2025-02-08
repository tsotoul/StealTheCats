using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StealTheCatsApi.Models
{
    public class Cat
    {
        [Key]
        public int Id { get; set; }
        public required string CatId { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public byte[] Image { get; set; } = Array.Empty<byte>();
        public DateTime Created { get; private set; }
        [NotMapped]
        public List<string> Temperaments { get; set; } = new List<string>();
        public List<CatTag> CatTags { get; set; } = new List<CatTag>();

        public Cat()
        {
            Created = DateTime.UtcNow;
        }
    }
}
