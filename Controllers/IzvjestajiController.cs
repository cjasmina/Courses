using System.Linq;

using Microsoft.AspNetCore.Mvc;

using Courses.Models;
using Courses.Helpers;
using Courses.Contexts;

using Vereyon.Web;
using Rotativa.AspNetCore;
using Courses.ViewModels;

namespace Courses.Controllers
{
    [Autentifikacija(Uloga.Administrator)]
    public class IzvjestajiController : Controller
    {
        private readonly IFlashMessage _flashMessage;
        private readonly DatabaseContext _databaseContext;

        public IzvjestajiController(DatabaseContext databaseContext, IFlashMessage flashMessage)
        {
            _databaseContext = databaseContext;
            _flashMessage = flashMessage;
        }

        [HttpGet]
        public IActionResult BrojPolaznikaPoKursevima()
        {
            var kursevi = _databaseContext.Kursevi.Select(x => new IzvjestajiBrojPolaznikaPoKursuViewModel
            {
                Naziv = x.Naziv,
                BrojPolaznika = x.Korisnici.Count(y => y.Korisnik.Uloga == Uloga.Polaznik)
            }).OrderByDescending(x => x.BrojPolaznika).ToList();

            return new ViewAsPdf("BrojPolaznikaPoKursevima", kursevi);
        }

        [HttpGet]
        public IActionResult BrojKursevaPoEdukatorima()
        {
            var kursevi = _databaseContext.Korisnici.Where(x => x.Uloga == Uloga.Edukator).Select(x => new IzvjestajiBrojKursevaPoEdukatoruViewModel
            {
                Ime = x.Ime,
                Prezime = x.Prezime,
                BrojKurseva = x.Kursevi.Count()
            }).OrderByDescending(x => x.BrojKurseva).ToList();

            return new ViewAsPdf("BrojKursevaPoEdukatorima", kursevi);
        }
    }
}
