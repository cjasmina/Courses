using System.Collections.Generic;

using Courses.Models;

namespace Courses.ViewModels
{
    public class GradoviDodavanjeIzmjenaViewModel
    {
        public GradViewModel Grad { get; set; }
        public List<Drzava> Drzave { get; set; }
    }
}
