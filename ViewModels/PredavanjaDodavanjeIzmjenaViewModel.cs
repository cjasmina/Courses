using System.Collections.Generic;

using Courses.Models;

namespace Courses.ViewModels
{
    public class PredavanjaDodavanjeIzmjenaViewModel
    {
        public PredavanjeViewModel Predavanje { get; set; }
        public List<Kurs> Kursevi { get; set; }
        public List<OdabraniPolaznikViewModel> Polaznici { get; set; }

        public class OdabraniPolaznikViewModel
        {
            public int KursKorisnikId { get; set; }
            public string Ime { get; set; }
            public string Prezime { get; set; }
            public string Napomena { get; set; }

            public bool Prisutan { get; set; }
        }
    }
}
