using System.Collections.Generic;

using Courses.Models;

namespace Courses.ViewModels
{
    public class KorisniciDodavanjeIzmjenaViewModel
    {
        public KorisnikViewModel Korisnik { get; set; }
        public List<Grad> Gradovi { get; set; }
        public List<OdabraniKursViewModel> Kursevi { get; set; }

        public class OdabraniKursViewModel
        {
            public int KursId { get; set; }
            public string Naziv { get; set; }
            public bool Odabran { get; set; }
        }
    }
}
