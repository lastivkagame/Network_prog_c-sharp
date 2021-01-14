using System.ComponentModel.DataAnnotations;

namespace DAL.Entities
{
    public class Contact
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [MaxLength(10)]
        public string Phone { get; set; }
        public string Email { get; set; }

    }
}
