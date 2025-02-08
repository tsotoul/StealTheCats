using System.ComponentModel.DataAnnotations;

namespace StealTheCats.Models
{
    public class Tag
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Created { get; set; }
        public List<CatTag> CatTags { get; set; }

        public Tag()
        {
            Created = DateTime.UtcNow;
        }
    }
}
