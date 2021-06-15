using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Courses.Models
{
    [Table("Predavanja")]
    public class Predavanje : Bazni
    {
        [Required]
        public string Naziv { get; set; }

        [Required]
        public string Opis { get; set; }

        [Required]
        public DateTime DatumPredavanja { get; set; }

        [ForeignKey(nameof(Models.Kurs))]
        public int KursId { get; set; }
        public Kurs Kurs { get; set; }

        public ICollection<Prisustvo> Prisustva { get; set; }
    }
}
