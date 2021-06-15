using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Courses.Models
{
    [Table("Drzave")]
    public class Drzava : Bazni
    {
        [Required]
        public string Naziv { get; set; }

        public List<Grad> Gradovi { get; set; }
    }
}