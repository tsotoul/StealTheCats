using System.ComponentModel.DataAnnotations;

namespace StealTheCatsApi.Models
{
    public class CatTag
    {
        [Key]
        public int Id { get; set; }

        public int CatId { get; set; }
        public Cat Cat { get; set; }

        public int TagId { get; set; }
        public Tag Tag { get; set; }
    }
}
