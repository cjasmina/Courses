using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Courses.Models
{
    [Table("Gradovi")]

    public class Grad : Bazni
    {
        [Required]
        public string Naziv { get; set; }

        [Required]
        public string PostanskiBroj { get; set; }

        [Required]
        [ForeignKey(nameof(Drzava))]
        public int DrzavaId { get; set; }

        public Drzava Drzava {get; set; }

        public List<Korisnik> Korisnici { get; set; }
    }
}
