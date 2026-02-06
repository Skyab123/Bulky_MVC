using System.ComponentModel.DataAnnotations;

namespace BulkyWeb.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; } // PK van tabel (adhv data annotatie)
        [Required]
        public string Name { get; set; }

        public int DisplayOrder { get; set; }
    }
}
