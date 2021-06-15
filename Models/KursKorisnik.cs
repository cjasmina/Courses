using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Courses.Models
{
    [Table("KursKorisnici")]
    public class KursKorisnik : Bazni
    {
        [Required]
        [ForeignKey(nameof(Kurs))]
        public int KursId { get; set; }
        public Kurs Kurs { get; set; }

        [Required]
        [ForeignKey(nameof(Korisnik))]
        public int KorisnikId { get; set; }
        public Korisnik Korisnik { get; set; }
    }
}
