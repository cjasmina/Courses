using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Courses.Models
{
    [Table("Oblasti")]
    public class Oblast : Bazni
    { 
        [Required]
        public string Naziv { get; set; }
    }
}