using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Courses.Models
{
    [Table("Obavijesti")]

    public class Obavijest : Bazni
    {
        [Required]
        public byte[] NaslovnaFotografija { get; set; }

        [Required]
        public string Naziv { get; set; }

        [Required]
        public string Opis { get; set; }

        [Required]
        public string KratakOpis { get; set; }

        [Required]
        public DateTime DatumObjave { get; set; }

        [Required]
        public DateTime DatumAzuriranja { get; set; }

        public byte[] Dodatak { get; set; }

        public string DodatakNaziv { get; set; }

        [Required]
        [ForeignKey(nameof(Models.TipObavijesti))]
        public int TipObavijestiId { get; set; }
        public TipObavijesti TipObavijesti { get; set; }


        [ForeignKey(nameof(Models.Kurs))]
        public int? KursId { get; set; }
        public Kurs Kurs { get; set; }
    }
}
