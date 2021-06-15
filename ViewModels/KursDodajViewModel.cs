using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Courses.ViewModels
{
    public class KursDodajViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Naziv je obavezan")]
        public string Naziv { get; set; }

        [Required(ErrorMessage = "Broj sati je obavezno")]
        public int BrojSati { get; set; }

        [Required(ErrorMessage = "Cijena je obavezna")]
        public int Cijena { get; set; }
        [Required(ErrorMessage = "Datum početka je obavezan")]
        public DateTime DatumPocetka { get; set; }
        [Required(ErrorMessage = "Datum završetka je obavezan")]
        public DateTime DatumZavrsetka { get; set; }

        [Required(ErrorMessage = "Opis je obavezan")]
        public string Opis { get; set; }
        [Required(ErrorMessage = "Oblast je obavezna")]
        public int OblastId { get; set; }
        public List<SelectListItem> OblastStavke { get; set; }

        public IFormFile Ikona { get; set; }
    }
}
