using Courses.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Courses.ViewModels
{
    public class KursDodajVM
    {
        public int KursID { get; set; }
        public string Naziv { get; set; }

        public int BrojSati { get; set; }

        public int Cijena { get; set; }
        public DateTime DatumPocetka { get; set; }
        public DateTime DatumZavrsetka { get; set; }

        public string? Opis { get; set; }
        public int OblastID { get; set; }
        public List<SelectListItem> OblastStavke { get; set; }


        public IFormFile Ikonica { get; set; }

       
       
    }
}
