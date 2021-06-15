using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Courses.Models
{
    [Table("Kursevi")]
    public class Kurs : Bazni
    {
        public byte[] Ikona { get; set; }

        [Required]
        public string Naziv { get; set; }

        [Required]
        public string Opis { get; set; }

        [Required]
        public int BrojSati { get; set; }

        [Required]
        public int Cijena { get; set; }

        [Required]
        public DateTime DatumPocetka { get; set; }

        [Required]
        public DateTime DatumZavrsetka { get; set; }

        [Required]
        [ForeignKey(nameof(Oblast))]
        public int OblastId { get; set; }
        public Oblast Oblast { get; set; }

        public ICollection<KursKorisnik> Korisnici { get; set; }
    }
}