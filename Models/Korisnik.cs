using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Courses.Models
{
    [Table("Korisnici")]

    public class Korisnik : Bazni
    {
        public byte[] ProfilnaFotografija { get; set; }

        [Required]
        public string Ime { get; set; }

        [Required]
        public string Prezime { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string KorisnickoIme { get; set; }

        [Required]
        public string LozinkaHash { get; set; }

        [Required]
        public string LozinkaSalt { get; set; }

        [Required]
        public Uloga Uloga { get; set; }

        [Required]
        public DateTime DatumRegistracije { get; set; }

        public DateTime? DatumPosljednjePrijave { get; set; }

        [Required]
        [ForeignKey(nameof(Grad))]
        public int GradId { get; set; }
        public Grad Grad { get; set; }

        public ICollection<KursKorisnik> Kursevi { get; set; }
    }
}
