using System.Collections.Generic;

using Courses.Models;

namespace Courses.ViewModels
{
    public class ObavijestiDodavanjeIzmjenaViewModel
    {
        public ObavijestViewModel Obavijest { get; set; }
        public List<TipObavijesti> TipoviObavijesti { get; set; }

        public List<Kurs> Kursevi { get; set; }
    }
}
