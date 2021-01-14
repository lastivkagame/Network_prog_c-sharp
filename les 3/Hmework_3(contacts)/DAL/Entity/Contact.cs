using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entity
{
    [Table("Contact")]
    public class Contact
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(100)]
        public string SurName { get; set; }

        public int Age { get; set; }

        public string City { get; set; }

        [Required]
        [MaxLength(20)]
        public string Telephone { get; set; }
        public byte[] ImageContact { get; set; }

    }
}
