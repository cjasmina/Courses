using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Courses.Models
{
    [Table("Prisustva")]
    public class Prisustvo : Bazni
    {
        [ForeignKey(nameof(Models.Predavanje))]
        public int PredavanjeId { get; set; }
        public Predavanje Predavanje { get; set; }

        [ForeignKey(nameof(Models.KursKorisnik))]
        public int KursKorisnikId { get; set; }
        public KursKorisnik KursKorisnik { get; set; }

        public string Napomena { get; set; }

        [Required]
        public bool Prisutan { get; set; }
    }
}