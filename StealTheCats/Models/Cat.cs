using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StealTheCats.Models
{
    public class Cat
    {
        [Key]
        public int Id { get; set; }
        public required string CatId { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public required string Image { get; set; }
        public DateTime Created { get; private set; }
        [NotMapped]
        public List<string> Temperaments { get; set; }
        public List<CatTag> CatTags { get; set; }

        public Cat()
        {
            Created = DateTime.UtcNow;
        }
    }
}
